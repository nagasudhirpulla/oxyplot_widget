using Dashboard.Interfaces;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Measurements.RandomMeasurement
{
    public class RandomMeasurement : IMeasurement
    {
        public double Low { get; set; }
        public double High { get; set; }
        public int NumPnts { get; set; }

        public async Task<List<DataPoint>> FetchData()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            Random random = new Random();
            for (int pointIter = 0; pointIter < NumPnts; pointIter++)
            {
                double value = random.NextDouble();
                value = Low + value * (High - Low);
                dataPoints.Add(new DataPoint(pointIter, value));
            }
            return dataPoints;
        }
    }
}
