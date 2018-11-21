using Dashboard.Interfaces;
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
        public double Low { get; set; } = 0;
        public double High { get; set; } = 10;
        public DateTime FromTime { get; set; } = DateTime.Now.AddMinutes(-20);
        public DateTime ToTime { get; set; } = DateTime.Now.AddMinutes(-1);
        public TimeSpan TimeResolution { get; set; } = TimeSpan.FromMinutes(1);

        public async Task<List<DataPoint>> FetchData()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            Random random = new Random();
            int numPnts = (int)((ToTime - FromTime).TotalMilliseconds / TimeResolution.TotalMilliseconds);
            DateTime tempTime = FromTime;
            for (int pointIter = 0; pointIter < numPnts; pointIter++)
            {
                double value = random.NextDouble();
                value = Low + value * (High - Low);
                DataPoint dataPoint = new DataPoint(DateTimeAxis.ToDouble(tempTime), value);
                dataPoints.Add(dataPoint);
                tempTime = tempTime + TimeResolution;
            }
            return dataPoints;
        }

        public string GetDisplayText()
        {
            return $"{Low} (Low), {High} (High), {FromTime} to {ToTime}";
        }

        public IMeasurement Clone()
        {
            return new RandomTimeSeriesMeasurement { Low = Low, High = High, FromTime = FromTime, ToTime = ToTime, TimeResolution = TimeResolution };
        }
    }
}
