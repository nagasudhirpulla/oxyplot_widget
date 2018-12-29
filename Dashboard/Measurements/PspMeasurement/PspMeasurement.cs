using Dashboard.Helpers;
using Dashboard.Interfaces;
using Dashboard.UserControls.VariableTimePicker;
using Dashboard.Widgets.Oxyplot;
using Mono.Web;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 Using Uri builder to build query strings  - https://stackoverflow.com/questions/17096201/build-query-string-for-system-net-httpclient-get
 Using HttpClient to do http get requests - https://blog.jayway.com/2012/03/13/httpclient-makes-get-and-post-very-simple/
     */
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

        public async Task<List<DataPoint>> FetchDataAsync(TimeShift timeShift)
        {
            return await FetchHelper.FetchData(StartTime.GetTime(), EndTime.GetTime(), MaxFetchSize, FetchData);
        }

        public async Task<List<DataPoint>> FetchData(DateTime startTime, DateTime endTime)
        {
            var builder = new UriBuilder("http://10.2.100.56")
            {
                Port = 7001
            };
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["label"] = MeasLabel;
            query["from_time"] = StartTime.GetTime().ToString("yyyyMMdd");
            query["to_time"] = "yyyyMMdd";
            builder.Query = query.ToString();
            string url = builder.ToString();
            return new List<DataPoint>();
        }

        public string GetDisplayText()
        {
            return $"{MeasName} ({MeasLabel}), {StartTime.GetTime().ToString()} - {EndTime.GetTime().ToString()}";
        }
    }
}
