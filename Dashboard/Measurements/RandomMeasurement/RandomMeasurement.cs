using Dashboard.Interfaces;
using Dashboard.UserControls.VariableTimePicker;
using Dashboard.Widgets.Oxyplot;
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
        public string TypeName { get; set; } = typeof(RandomMeasurement).Name;
        public double Low { get; set; } = 0;
        public double High { get; set; } = 10;
        public int NumPnts { get; set; } = 30;

        public async Task<List<DataPoint>> FetchData(TimeShift timeShift)
        {
            return await FetchData(null, null);
        }

        public async Task<List<DataPoint>> FetchData(VariableTime startTime, VariableTime endTime)
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

        public string GetDisplayText()
        {
            return $"{Low} (Low), {High} (High), {NumPnts} (Number of points)";
        }

        public IMeasurement Clone()
        {
            return new RandomMeasurement { Low = Low, High = High, NumPnts = NumPnts };
        }
        
    }
}
