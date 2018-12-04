using Dashboard.Helpers.EnumHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Scheduler
{
    public class SchedulerState
    {
        public TimeSpan FetchPeriodicity { get; set; } = TimeSpan.FromSeconds(5);
        public ScheduleMode Mode { get; set; } = ScheduleMode.Disabled;

        public SchedulerState Clone()
        {
            return new SchedulerState { FetchPeriodicity = FetchPeriodicity, Mode = Mode };
        }
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ScheduleMode
    {
        [Description("Off")]
        Disabled,
        [Description("Periodic")]
        Periodic
    }
}
