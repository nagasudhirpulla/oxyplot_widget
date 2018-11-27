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
using Dashboard.UserControls.VariableTimePicker;

namespace Dashboard.Measurements.PMUMeasurement
{
    public class PMUMeasurement : IMeasurement
    {
        public VariableTime StartTime { get; set; } = new VariableTime { AbsoluteTime = DateTime.Now.AddMinutes(-10) };
        public VariableTime EndTime { get; set; } = new VariableTime { AbsoluteTime = DateTime.Now.AddMinutes(-9) };
        public int MeasId { get; set; } = 4924;
        public string MeasName { get; set; } = "Meas name";
        public string TypeName { get; set; } = typeof(PMUMeasurement).Name;

        public async Task<List<DataPoint>> FetchData(TimeShift timeShift)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            // using data layer for fetching data
            HistoryDataAdapter adapter = new HistoryDataAdapter(new ConfigurationManagerJSON());
            List<int> measIds = new List<int> { MeasId };
            Dictionary<object, List<PMUDataStructure>> res = await adapter.GetDataAsync(StartTime.GetTime(), EndTime.GetTime(), measIds, true, false, 25);

            // check if result has one key since we queried for only one key
            if (res.Keys.Count == 1)
            {
                // todo check the measId also

                List<PMUDataStructure> dataResults = res.Values.ElementAt(0);
                for (int resIter = 0; resIter < dataResults.Count; resIter++)
                {
                    DateTime dataTime = dataResults[resIter].TimeStamp;
                    // convert the time from utc to local
                    dataTime = DateTime.SpecifyKind((TimeZoneInfo.ConvertTime(dataTime, TimeZoneInfo.Utc, TimeZoneInfo.Local)), DateTimeKind.Local);
                    if (timeShift != null)
                    {
                        dataTime = TimeShift.DoShifting(dataTime, timeShift);
                    }
                    DataPoint dataPoint = new DataPoint(DateTimeAxis.ToDouble(dataTime), dataResults[resIter].Value[0]);
                    dataPoints.Add(dataPoint);
                }
            }
            return dataPoints;
        }

        public string GetDisplayText()
        {
            return $"{MeasName} ({MeasId}), {StartTime.GetTime().ToString()} - {EndTime.GetTime().ToString()}";
        }

        public IMeasurement Clone()
        {
            return new PMUMeasurement { StartTime = StartTime, EndTime = EndTime, MeasId = MeasId, MeasName = MeasName };
        }
    }
}
