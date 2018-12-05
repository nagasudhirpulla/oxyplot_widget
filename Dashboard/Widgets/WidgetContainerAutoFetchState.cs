using Dashboard.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Widgets
{
    public class WidgetContainerAutoFetchState
    {
        public SchedulerState SchedulerState { get; set; } = new SchedulerState();

        public TimeSpan GetTimerPeriod()
        {
            TimeSpan span = SchedulerState.FetchPeriodicity;
            return span;
        }

        public WidgetContainerAutoFetchState Clone()
        {
            WidgetContainerAutoFetchState state = new WidgetContainerAutoFetchState
            {
                SchedulerState = SchedulerState.Clone()
            };
            return state;
        }
    }
}
