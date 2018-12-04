using Dashboard.Scheduler;
using Dashboard.UserControls.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dashboard.EditorWindows
{
    /// <summary>
    /// Interaction logic for DashboardAutoFetchEditWindow.xaml
    /// </summary>
    public partial class DashboardAutoFetchEditWindow : Window
    {
        public AutoFetchConfigVM FetchConfigVM { get; set; }
        public DashboardAutoFetchEditWindow(DashboardAutoFetchState autoFetchState)
        {
            InitializeComponent();
            FetchConfigVM = new AutoFetchConfigVM(autoFetchState);
            DataContext = FetchConfigVM;
            SetupEditorUC(FetchConfigVM.AutoFetchState_.SchedulerState);
        }

        public DashboardAutoFetchState DashboardAutoFetchState { get { return FetchConfigVM.AutoFetchState_; } }

        private void SetupEditorUC(SchedulerState schedulerState)
        {
            SchedulerEditContainer.Children.Add(new SchedulerStateEditUC(schedulerState));
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Changes ?", "Confirm Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            else
            {
                DialogResult = true;
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }

    public class AutoFetchConfigVM
    {
        public DashboardAutoFetchState AutoFetchState_ { get; set; }

        public AutoFetchConfigVM(DashboardAutoFetchState state)
        {
            AutoFetchState_ = state.Clone();
        }

        public bool IsDominatingFetch
        {
            get { return AutoFetchState_.IsDominatingSchedule; }
            set
            {
                AutoFetchState_.IsDominatingSchedule = value;
            }
        }
    }

}
