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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dashboard.Scheduler
{
    /// <summary>
    /// Interaction logic for SchedulerStateEditUC.xaml
    /// </summary>
    public partial class SchedulerStateEditUC : UserControl
    {
        // https://stackoverflow.com/questions/6145888/how-to-bind-an-enum-to-a-combobox-control-in-wpf

        public SchedulerConfigVM ConfigVM_;
        public SchedulerStateEditUC(SchedulerState autoFetchState)
        {
            InitializeComponent();
            ConfigVM_ = new SchedulerConfigVM(autoFetchState);
            DataContext = ConfigVM_;
        }
    }

    public class SchedulerConfigVM
    {
        public SchedulerState AutoFetchState_;

        public SchedulerConfigVM(SchedulerState state)
        {
            AutoFetchState_ = state;
        }

        public ScheduleMode Mode { get { return AutoFetchState_.Mode; } set { AutoFetchState_.Mode = value; } }

        public TimeSpan Periodicity { get { return AutoFetchState_.FetchPeriodicity; } set { AutoFetchState_.FetchPeriodicity = value; } }

    }
}
