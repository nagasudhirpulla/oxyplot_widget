using Dashboard.Interfaces;
using Dashboard.States;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dashboard.Widgets
{
    /// <summary>
    /// Interaction logic for BlankWidget.xaml
    /// </summary>
    public partial class BlankWidget : UserControl, IWidget
    {
        public BlankWidget()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Send Messages to Dashboard using this event handler
        public Action<EventArgs> Changed { get; set; }

        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(e);
        }

        public async Task RefreshData()
        {
            // do nothing
            await Task.Delay(1);
        }

        public async Task DoCleanUpForDeletion()
        {
            // do nothing
        }

        public void OpenConfigWindow()
        {
            // do nothing
        }

        public IWidgetState GenerateState()
        {
            BlankWidgetState state = new BlankWidgetState();

            // currently nothing is stored in the state

            return state;
        }

        public void SetState(IWidgetState state)
        {
            if (state is BlankWidgetState widgetState)
            {
                // nothing is currentlt stored in state, hence do nothing
            }
            else
            {
                Console.WriteLine("Inflation rejected since non BlankWidgetState given for inflation...");
            }
        }
    }
}
