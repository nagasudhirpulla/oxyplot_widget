using Dashboard.EditorWindows;
using Dashboard.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Dashboard.Widgets
{
    public class WidgetContainerAutoFetchManager
    {
        private DispatcherTimer FetchTimer_ { get; set; } = new DispatcherTimer();
        private WidgetContainerAutoFetchState WidgetContainerAutoFetchState_ { get; set; }
        public WidgetContainerAutoFetchState WidgetContainerAutoFetchState
        {
            get { return WidgetContainerAutoFetchState_; }
            set
            { WidgetContainerAutoFetchState_ = value; ReconfigureTimer(); }
        }
        public Func<object, EventArgs, Task> Fetch_Timer_TickAsync { get; set; }

        public WidgetContainerAutoFetchManager(Func<object, EventArgs, Task> handler, WidgetContainerAutoFetchState state)
        {
            Fetch_Timer_TickAsync = handler;
            WidgetContainerAutoFetchState_ = state;
            DoInitialWireUp();
        }

        private void DoInitialWireUp()
        {
            FetchTimer_.Tick += (o, e) => Fetch_Timer_TickAsync(o, e);
            FetchTimer_.Interval = TimeSpan.FromHours(1);
            ReconfigureTimer();
        }

        public void ReconfigureTimer()
        {
            // Check if scheduling is enabled in config
            if (WidgetContainerAutoFetchState_.SchedulerState.Mode == ScheduleMode.Periodic)
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
            Fetch_Timer_TickAsync(null, null);
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
            FetchTimer_.Interval = WidgetContainerAutoFetchState_.GetTimerPeriod();
        }

        public WidgetContainerAutoFetchState OpenWidgetContainerAutoFetchConfigWindow(WidgetContainerAutoFetchState state)
        {
            WidgetContainerAutoFetchEditWindow editWindow = new WidgetContainerAutoFetchEditWindow(state);
            editWindow.ShowDialog();
            if (editWindow.DialogResult == true)
            {
                WidgetContainerAutoFetchState newState = editWindow.WidgetContainerAutoFetchState;
                return newState;
            }
            return null;
        }
    }
}
