using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Helpers
{
    class FetchHelper
    {
        public static async Task<List<DataPoint>> FetchData(DateTime fromTime, DateTime toTime, TimeSpan MaxFetchSize, Func<DateTime, DateTime, Task<List<DataPoint>>> FetchDataNonBatch)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            DateTime fetchStartTime = fromTime;
            DateTime fetchEndTime = fromTime;
            do
            {
                // derive fetch start and fetch end times
                fetchStartTime = fetchEndTime;
                fetchEndTime = fetchStartTime + MaxFetchSize;

                if (fetchStartTime.Equals(fetchEndTime))
                {
                    // When batch interval is zero, we will get data in a single fetch
                    fetchEndTime = toTime;
                }
                if (fetchEndTime > toTime)
                {
                    // Do not fetch above toTime
                    fetchEndTime = toTime;
                }

                // get the data batch
                List<DataPoint> tempDataPoints = await FetchDataNonBatch(fetchStartTime, fetchEndTime);

                // if this iteration is not the first iteration, remove the first sample from this data point list, since it was the last sample of the previous data point list
                if (fetchStartTime > fromTime)
                {
                    tempDataPoints.RemoveAt(0);
                }

                // add the batch result to the final result
                dataPoints.AddRange(tempDataPoints);
            } while (fetchEndTime < toTime);
            return dataPoints;
        }
    }
}
