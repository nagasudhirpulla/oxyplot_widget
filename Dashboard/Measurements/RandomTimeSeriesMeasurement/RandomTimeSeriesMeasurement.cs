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

        public async Task<List<DataPoint>> FetchData(TimeShift timeShift)
        {
            return await FetchData(FromTime, ToTime);
        }

        public async Task<List<DataPoint>> FetchData(VariableTime startTime, VariableTime endTime)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            DateTime fromTime = startTime.GetTime();
            DateTime toTime = endTime.GetTime();

            DateTime tempTime = startTime.GetTime();
            for (int pointIter = 0; tempTime < toTime; pointIter++)
            {
                double value = Random.NextDouble();
                value = Low + value * (High - Low);
                DataPoint dataPoint = new DataPoint(DateTimeAxis.ToDouble(tempTime), value);
                dataPoints.Add(dataPoint);
                tempTime = tempTime + TimeResolution;
            }
            return dataPoints;
        }

        public string GetDisplayText()
        {
            return $"{Low} (Low), {High} (High), {FromTime.GetTime()} to {ToTime.GetTime()}";
        }

        public IMeasurement Clone()
        {
            return new RandomTimeSeriesMeasurement { Low = Low, High = High, FromTime = FromTime, ToTime = ToTime, TimeResolution = TimeResolution, MaxFetchSize = MaxFetchSize };
        }
    }
}
