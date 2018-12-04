using Dashboard.EditorWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.UserControls.Dashboard
{
    public class DashboardAutoFetchManager
    {
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
