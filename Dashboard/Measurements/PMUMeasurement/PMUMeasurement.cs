using Dashboard.Interfaces;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMUDataLayer;
using PMUDataLayer.DataExchangeClasses;
using PMUDataLayer.Config;
using OxyPlot.Axes;
using Dashboard.Widgets.Oxyplot;

namespace Dashboard.Measurements.PMUMeasurement
{
    public class PMUMeasurement : IMeasurement
    {
        public DateTime StartTime { get; set; } = DateTime.Now.AddSeconds(-5);
        public DateTime EndTime { get; set; } = DateTime.Now.AddSeconds(-5);
        public int MeasId { get; set; } = 4924;
        public string MeasName { get; set; } = "Meas name";

        public async Task<List<DataPoint>> FetchData(TimeShift timeShift)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            // using data layer for fetching data
            HistoryDataAdapter adapter = new HistoryDataAdapter(new ConfigurationManagerJSON());
            List<int> measIds = new List<int> { MeasId };
            Dictionary<object, List<PMUDataStructure>> res = await adapter.GetDataAsync(StartTime, EndTime, measIds, true, false, 25);

            // check if result has one key since we queried for only one key
            if (res.Keys.Count == 1)
            {
                // todo check the measId also

                List<PMUDataStructure> dataResults = res.Values.ElementAt(0);
                for (int resIter = 0; resIter < dataResults.Count; resIter++)
                {
                    DateTime dataTime = dataResults[resIter].TimeStamp;
                    if (timeShift != null)
                    {
                        dataTime = TimeShift.DoShifting(dataTime, timeShift);
                    }
                    DataPoint dataPoint = new DataPoint(DateTimeAxis.ToDouble(dataResults[resIter].TimeStamp), dataResults[resIter].Value[0]);
                    dataPoints.Add(dataPoint);
                }
            }
            return dataPoints;
        }

        public string GetDisplayText()
        {
            return $"{MeasName} ({MeasId}), {StartTime.ToString()} - {EndTime.ToString()}";
        }

        public IMeasurement Clone()
        {
            return new PMUMeasurement { StartTime = StartTime, EndTime = EndTime, MeasId = MeasId, MeasName = MeasName };
        }
    }
}
