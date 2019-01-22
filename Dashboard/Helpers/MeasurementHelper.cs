using Dashboard.Interfaces;
using Dashboard.Measurements.PMUMeasurement;
using Dashboard.Measurements.PspMeasurement;
using Dashboard.Measurements.RandomTimeSeriesMeasurement;
using Dashboard.Measurements.ScadaMeasurement;
using Dashboard.UserControls.VariableTimePicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Helpers
{
    public class MeasurementHelper
    {
        public static StartEndVarTime GetStartEndTimes(IMeasurement meas)
        {
            StartEndVarTime time = new StartEndVarTime();
            if (meas is RandomTimeSeriesMeasurement randomMeas)
            {
                time.StartTime = randomMeas.FromTime.Clone();
                time.EndTime = randomMeas.ToTime.Clone();
            }
            else if (meas is ScadaMeasurement scadaMeas)
            {
                time.StartTime = scadaMeas.StartTime.Clone();
                time.EndTime = scadaMeas.EndTime.Clone();
            }
            else if (meas is PMUMeasurement pmuMeas)
            {
                time.StartTime = pmuMeas.StartTime.Clone();
                time.EndTime = pmuMeas.EndTime.Clone();
            }
            else if (meas is PspMeasurement pspMeas)
            {
                time.StartTime = pspMeas.StartTime.Clone();
                time.EndTime = pspMeas.EndTime.Clone();
            }
            else
            {
                return null;
            }
            return time;
        }
    }

    public class StartEndVarTime
    {
        public VariableTime StartTime { get; set; }
        public VariableTime EndTime { get; set; }
    }
}
