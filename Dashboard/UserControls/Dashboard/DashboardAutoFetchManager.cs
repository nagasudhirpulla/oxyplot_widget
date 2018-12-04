using Dashboard.EditorWindows;
using Dashboard.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Dashboard.UserControls.Dashboard
{
    public class DashboardAutoFetchManager
    {
        private DispatcherTimer FetchTimer_ { get; set; } = new DispatcherTimer();
        private DashboardAutoFetchState DashboardAutoFetchState_ { get; set; }
        public DashboardAutoFetchState DashboardAutoFetchState
        {
            get { return DashboardAutoFetchState_; }
            set
            { DashboardAutoFetchState_ = value; ReconfigureTimer(); }
        }
        public Action<object, EventArgs> Fetch_Timer_Tick { get; set; }

        public DashboardAutoFetchManager(Action<object, EventArgs> handler, DashboardAutoFetchState state)
        {
            Fetch_Timer_Tick = handler;
            DashboardAutoFetchState_ = state;
            DoInitialWireUp();
        }

        private void DoInitialWireUp()
        {
            FetchTimer_.Tick += (o, e) => Fetch_Timer_Tick(o, e);
            FetchTimer_.Interval = TimeSpan.FromHours(1);
            ReconfigureTimer();
        }

        public void ReconfigureTimer()
        {
            // Check if scheduling is enabled in config
            if (DashboardAutoFetchState_.SchedulerState.Mode == ScheduleMode.Periodic)
            {
                UpdateSchedulerPeriod();
                StartScheduler();
            }
            else
            {
                StopScheduler();
            }
        }

        public void StartScheduler()
        {
            StopScheduler();
            FetchTimer_.Start();
            Fetch_Timer_Tick(null, null);
        }

        public void StopScheduler()
        {
            // Stop the fetch timer if active
            if (FetchTimer_.IsEnabled)
            {
                FetchTimer_.Stop();
            }
        }

        public void UpdateSchedulerPeriod()
        {
            // Get the periodicity of the timer
            FetchTimer_.Interval = DashboardAutoFetchState_.GetTimerPeriod();
        }

        public DashboardAutoFetchState OpenDashboardAutoFetchConfigWindow(DashboardAutoFetchState state)
        {
            DashboardAutoFetchEditWindow editWindow = new DashboardAutoFetchEditWindow(state);
            editWindow.ShowDialog();
            if (editWindow.DialogResult == true)
            {
                DashboardAutoFetchState newState = editWindow.DashboardAutoFetchState;
                return newState;
            }
            return null;
        }
    }
}
