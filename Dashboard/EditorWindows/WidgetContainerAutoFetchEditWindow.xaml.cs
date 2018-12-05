using Dashboard.Scheduler;
using Dashboard.Widgets;
using System.Windows;

namespace Dashboard.EditorWindows
{
    /// <summary>
    /// Interaction logic for WidgetContainerAutoFetchEditWindow.xaml
    /// </summary>
    public partial class WidgetContainerAutoFetchEditWindow : Window
    {
        public WidgetContainerAutoFetchConfigVM FetchConfigVM { get; set; }
        public WidgetContainerAutoFetchEditWindow(WidgetContainerAutoFetchState autoFetchState)
        {
            InitializeComponent();
            FetchConfigVM = new WidgetContainerAutoFetchConfigVM(autoFetchState);
            DataContext = FetchConfigVM;
            SetupEditorUC(FetchConfigVM.AutoFetchState_.SchedulerState);
        }

        public WidgetContainerAutoFetchState WidgetContainerAutoFetchState { get { return FetchConfigVM.AutoFetchState_; } }

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

    public class WidgetContainerAutoFetchConfigVM
    {
        public WidgetContainerAutoFetchState AutoFetchState_ { get; set; }

        public WidgetContainerAutoFetchConfigVM(WidgetContainerAutoFetchState state)
        {
            AutoFetchState_ = state.Clone();
        }
    }
}
