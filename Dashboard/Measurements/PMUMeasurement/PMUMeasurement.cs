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
using Dashboard.Helpers;

namespace Dashboard.Measurements.PMUMeasurement
{
    public class PMUMeasurement : IMeasurement
    {
        public VariableTime StartTime { get; set; } = new VariableTime { AbsoluteTime = DateTime.Now.AddMinutes(-10) };
        public VariableTime EndTime { get; set; } = new VariableTime { AbsoluteTime = DateTime.Now.AddMinutes(-9) };
        public TimeSpan MaxFetchSize { get; set; } = TimeSpan.FromDays(1);
        public int MeasId { get; set; } = 4924;
        public string MeasName { get; set; } = "Meas name";
        // The data resolution should not be more than this. If this zero, then give raw data.
        public TimeSpan MaxResolution { get; set; } = TimeSpan.FromMilliseconds(40);
        public string TypeName { get; set; } = typeof(PMUMeasurement).Name;

        public async Task<List<DataPoint>> FetchData(TimeShift timeShift)
        {
            return await FetchHelper.FetchData(StartTime.GetTime(), EndTime.GetTime(), MaxFetchSize, FetchData);
            //return await FetchData(StartTime.GetTime(), EndTime.GetTime());
        }

        public string GetDisplayText()
        {
            return $"{MeasName} ({MeasId}), {StartTime.GetTime().ToString()} - {EndTime.GetTime().ToString()}";
        }

        public IMeasurement Clone()
        {
            return new PMUMeasurement { StartTime = StartTime, EndTime = EndTime, MeasId = MeasId, MeasName = MeasName, MaxFetchSize = MaxFetchSize, MaxResolution = MaxResolution };
        }

        public static void OpenSettingsWindow()
        {
            PMUSettingsEditWindow positionEditor = new PMUSettingsEditWindow();
            positionEditor.ShowDialog();
            if (positionEditor.DialogResult == true)
            {
                // since it is saved in file, do nothing
            }
        }

        public async Task<List<DataPoint>> FetchData(DateTime startTime, DateTime endTime)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            // using data layer for fetching data
            ConfigurationManagerJSON configManager = new ConfigurationManagerJSON();
            configManager.Initialize();
            HistoryDataAdapter adapter = new HistoryDataAdapter(configManager);
            List<int> measIds = new List<int> { MeasId };
            Dictionary<object, List<PMUDataStructure>> res = await adapter.GetDataAsync(startTime, endTime, measIds, true, false, 25);

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
                    DataPoint dataPoint = new DataPoint(DateTimeAxis.ToDouble(dataTime), dataResults[resIter].Value[0]);
                    dataPoints.Add(dataPoint);
                }

                // Create dataPoints based on the fetch strategy and max Resolution
                dataPoints = FetchHelper.GetDataPointsWithGivenMaxSampleInterval(dataPoints, MaxResolution);
            }
            return dataPoints;
        }
    }
}
