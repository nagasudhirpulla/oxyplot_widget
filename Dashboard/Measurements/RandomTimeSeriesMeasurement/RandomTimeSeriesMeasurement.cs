using Dashboard.Helpers;
using Dashboard.Interfaces;
using Dashboard.UserControls.VariableTimePicker;
using Dashboard.Widgets.Oxyplot;
using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Measurements.RandomTimeSeriesMeasurement
{
    public class RandomTimeSeriesMeasurement : IMeasurement
    {
        public string TypeName { get; set; } = typeof(RandomTimeSeriesMeasurement).Name;
        public double Low { get; set; } = 0;
        public double High { get; set; } = 10;
        public VariableTime FromTime { get; set; } = new VariableTime { AbsoluteTime = DateTime.Now.AddMinutes(-20) };
        public VariableTime ToTime { get; set; } = new VariableTime { AbsoluteTime = DateTime.Now.AddMinutes(-1) };
        public TimeSpan MaxFetchSize { get; set; } = TimeSpan.FromDays(1);
        public TimeSpan TimeResolution { get; set; } = TimeSpan.FromMinutes(1);
        public static Random Random { get; set; } = new Random();

        public async Task<List<DataPoint>> FetchDataAsync(TimeShift timeShift)
        {
            return await FetchData(FromTime.GetTime(), ToTime.GetTime());
        }

        public async Task<List<DataPoint>> FetchDataNonBatch(DateTime startTime, DateTime endTime)
        {
            await Task.Yield();
            List<DataPoint> dataPoints = new List<DataPoint>();
            DateTime fromTime = startTime;
            DateTime toTime = endTime;

            DateTime tempTime = startTime;
            for (int pointIter = 0; tempTime < toTime; pointIter++)
            {
                DataPoint dataPoint = new DataPoint(DateTimeAxis.ToDouble(tempTime), GenerateRandomValue());
                dataPoints.Add(dataPoint);
                tempTime = tempTime + TimeResolution;
            }

            if (dataPoints.Count > 0 && dataPoints.Last().X != DateTimeAxis.ToDouble(toTime))
            {
                // If toTime is missing, add it add to the results
                dataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(toTime), GenerateRandomValue()));
            }
            return dataPoints;
        }

        double GenerateRandomValue()
        {
            double value = Random.NextDouble();
            value = Low + value * (High - Low);
            return value;
        }

        public async Task<List<DataPoint>> FetchDataOld(DateTime fromTime, DateTime toTime)
        {
            // Remove this since we moved this to FetchHelper
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

        public async Task<List<DataPoint>> FetchData(DateTime fromTime, DateTime toTime)
        {
            return await FetchHelper.FetchData(fromTime, toTime, MaxFetchSize, FetchDataNonBatch);
        }

            public string GetDisplayText()
        {
            return $"{Low} (Low), {High} (High), {FromTime.GetTime()} to {ToTime.GetTime()}";
        }

        public IMeasurement Clone()
        {
            return new RandomTimeSeriesMeasurement { Low = Low, High = High, FromTime = FromTime.Clone(), ToTime = ToTime.Clone(), TimeResolution = TimeResolution, MaxFetchSize = MaxFetchSize };
        }
    }
}
