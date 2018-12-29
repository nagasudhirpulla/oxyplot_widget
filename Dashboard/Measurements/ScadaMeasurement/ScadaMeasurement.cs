using Dashboard.Interfaces;
using Dashboard.UserControls.VariableTimePicker;
using Dashboard.Widgets.Oxyplot;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InStep.eDNA.EzDNAApiNet;
using System.Threading.Tasks;
using OxyPlot.Axes;

namespace Dashboard.Measurements.ScadaMeasurement
{
    public class ScadaMeasurement : IMeasurement
    {
        public const string FetchStrategySnap = "snap";
        public const string FetchStrategyAverage = "average";
        public const string FetchStrategyMax = "max";
        public const string FetchStrategyMin = "min";
        public const string FetchStrategyRaw = "raw";

        public VariableTime StartTime { get; set; } = new VariableTime { AbsoluteTime = DateTime.Now.AddMinutes(-10) };
        public VariableTime EndTime { get; set; } = new VariableTime { AbsoluteTime = DateTime.Now.AddMinutes(-9) };
        public TimeSpan MaxFetchSize { get; set; } = TimeSpan.FromDays(1);
        public string MeasId { get; set; } = "Scada Measurement Id";
        public string MeasName { get; set; } = "Scada Meas name";
        public string FetchStrategy { get; set; } = FetchStrategyAverage;
        public int FetchPeriodicitySecs { get; set; } = 60;

        public string TypeName { get; set; } = typeof(ScadaMeasurement).Name;

        public string GetDisplayText()
        {
            return $"{MeasName} ({MeasId}), {StartTime.GetTime().ToString()} - {EndTime.GetTime().ToString()}";
        }

        public IMeasurement Clone()
        {
            return new ScadaMeasurement { StartTime = StartTime, EndTime = EndTime, MeasId = MeasId, MeasName = MeasName, FetchStrategy = FetchStrategy, FetchPeriodicitySecs = FetchPeriodicitySecs };
        }

        public async Task<List<DataPoint>> FetchDataAsync(TimeShift timeShift)
        {
            return await FetchData(StartTime.GetTime(), EndTime.GetTime());
        }

        public async Task<List<DataPoint>> FetchData(DateTime startTime, DateTime endTime)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            List<ScadaPointResult> dataResults = FetchHistoricalPointData(startTime, endTime);
            if (dataResults != null)
            {
                for (int resIter = 0; resIter < dataResults.Count; resIter++)
                {
                    DateTime dataTime = dataResults[resIter].ResultTime_;

                    DataPoint dataPoint = new DataPoint(DateTimeAxis.ToDouble(dataTime), dataResults[resIter].Val_);
                    dataPoints.Add(dataPoint);
                }
            }
            return dataPoints;
        }

        public List<ScadaPointResult> FetchHistoricalPointData(DateTime startTime, DateTime endTime)
        {
            try
            {
                int nret = 0;
                uint s = 0;
                double dval = 0;
                DateTime timestamp = DateTime.Now;
                string status = "";
                TimeSpan period = TimeSpan.FromSeconds(FetchPeriodicitySecs);
                // History request initiation
                if (FetchStrategy == FetchStrategyRaw)
                { nret = History.DnaGetHistRaw(MeasId, startTime, endTime, out s); }
                else if (FetchStrategy == FetchStrategySnap)
                { nret = History.DnaGetHistSnap(MeasId, startTime, endTime, period, out s); }
                else if (FetchStrategy == FetchStrategyAverage)
                { nret = History.DnaGetHistAvg(MeasId, startTime, endTime, period, out s); }
                else if (FetchStrategy == FetchStrategyMin)
                { nret = History.DnaGetHistMin(MeasId, startTime, endTime, period, out s); }
                else if (FetchStrategy == FetchStrategyMax)
                { nret = History.DnaGetHistMax(MeasId, startTime, endTime, period, out s); }

                // Get history values
                List<ScadaPointResult> historyResults = new List<ScadaPointResult>();
                while (nret == 0)
                {
                    nret = History.DnaGetNextHist(s, out dval, out timestamp, out status);
                    if (status != null)
                    {
                        historyResults.Add(new ScadaPointResult(dval, status, timestamp));
                    }
                }
                return historyResults;
            }
            catch (Exception e)
            {
                // Todo send this to console printing of the dashboard
                Console.WriteLine($"Error while fetching history data of point {MeasId} ({MeasName})");
                Console.WriteLine($"The exception is {e}");
            }
            return null;
        }
    }

    public class ScadaPointResult
    {
        // Vale of the data point
        public double Val_ { get; set; }

        // Data Quality of the result
        public string DataQuality_ { get; set; }

        // Result TimeStamp
        public DateTime ResultTime_ { get; set; }

        // Result units
        public string Units_ { get; set; }

        public ScadaPointResult(double val, string DataQuality, DateTime ResultTime)
        {
            Val_ = val;
            DataQuality_ = DataQuality;
            ResultTime_ = ResultTime;
        }
    }
}
