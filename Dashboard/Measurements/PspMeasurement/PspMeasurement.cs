using Dashboard.Helpers;
using Dashboard.Interfaces;
using Dashboard.UserControls.VariableTimePicker;
using Dashboard.Widgets.Oxyplot;
using Mono.Web;
using OxyPlot;
using OxyPlot.Axes;
using PspDataLayer;
using PspDataLayer.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPoint = OxyPlot.DataPoint;

namespace Dashboard.Measurements.PspMeasurement
{
    public class PspMeasurement : IMeasurement
    {
        public VariableTime StartTime { get; set; } = new VariableTime { AbsoluteTime = DateTime.Now.AddDays(-10) };
        public VariableTime EndTime { get; set; } = new VariableTime { AbsoluteTime = DateTime.Now.AddDays(-1) };
        public TimeSpan MaxFetchSize { get; set; } = TimeSpan.FromDays(600);
        public string MeasLabel { get; set; } = "gujarat_thermal_mu";
        public string MeasName { get; set; } = "Gujarat Thermal MU";
        // The data resolution should not be more than this. If this zero, then give raw data.
        public TimeSpan MaxResolution { get; set; } = TimeSpan.FromMilliseconds(0);
        public SamplingStrategy SamplingStrategy { get; set; } = SamplingStrategy.Average;
        public string TypeName { get; set; } = typeof(PspMeasurement).Name;

        public IMeasurement Clone()
        {
            return new PspMeasurement { StartTime = StartTime, EndTime = EndTime, MeasLabel = MeasLabel, MeasName = MeasName, MaxFetchSize = MaxFetchSize, MaxResolution = MaxResolution, SamplingStrategy = SamplingStrategy };
        }

        public static void OpenSettingsWindow()
        {
            PspSettingsEditWindow pspSettingsEditor = new PspSettingsEditWindow();
            pspSettingsEditor.ShowDialog();
            if (pspSettingsEditor.DialogResult == true)
            {
                // since it is saved in file, do nothing
            }
        }

        public async Task<List<DataPoint>> FetchDataAsync(TimeShift timeShift)
        {
            return await FetchHelper.FetchData(StartTime.GetTime(), EndTime.GetTime(), MaxFetchSize, FetchData);
        }

        public async Task<List<DataPoint>> FetchData(DateTime startTime, DateTime endTime)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            // using data layer for fetching data
            ConfigurationManagerJSON configManager = new ConfigurationManagerJSON();
            configManager.Initialize();
            PspDataAdapter adapter = new PspDataAdapter { ConfigurationManager = configManager };
            Dictionary<string, List<PspDataLayer.DataPoint>> res = await adapter.GetDataAsync(startTime, endTime, MeasLabel);

            // check if result has one key since we queried for only one key
            if (res.Keys.Count == 1)
            {
                // todo check the measId also

                List<PspDataLayer.DataPoint> dataResults = res.Values.ElementAt(0);
                for (int resIter = 0; resIter < dataResults.Count; resIter++)
                {
                    DateTime dataTime = dataResults[resIter].Time;
                    // convert the time from utc to local
                    //dataTime = DateTime.SpecifyKind((TimeZoneInfo.ConvertTime(dataTime, TimeZoneInfo.Utc, TimeZoneInfo.Local)), DateTimeKind.Local);
                    DataPoint dataPoint = new DataPoint(DateTimeAxis.ToDouble(dataTime), dataResults[resIter].Value);
                    dataPoints.Add(dataPoint);
                }

                // Create dataPoints based on the fetch strategy and max Resolution
                dataPoints = FetchHelper.GetDataPointsWithGivenMaxSampleInterval(dataPoints, MaxResolution, SamplingStrategy, DateTimeAxis.ToDouble(startTime));
            }
            return dataPoints;
        }

        public string GetDisplayText()
        {
            return $"{MeasName} ({MeasLabel}), {StartTime.GetTime().ToString()} - {EndTime.GetTime().ToString()}";
        }
    }
}
