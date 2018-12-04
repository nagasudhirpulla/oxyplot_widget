using Dashboard.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.UserControls.Dashboard
{
    public class DashboardAutoFetchState
    {
        public SchedulerState SchedulerState { get; set; } = new SchedulerState();
        public bool IsDominatingSchedule { get; set; } = false;

        public DashboardAutoFetchState Clone()
        {
            DashboardAutoFetchState state = new DashboardAutoFetchState
            {
                SchedulerState = SchedulerState.Clone(),
                IsDominatingSchedule = IsDominatingSchedule
            };
            return state;
        }
    }
}
