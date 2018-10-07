using Dashboard.Interfaces;
using Dashboard.States;
using Dashboard.Widgets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace Dashboard.UserControls.Dashboard
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl, INotifyPropertyChanged
    {
        //todo rename CellsContianer as widgets container
        public Dashboard()
        {
            InitializeComponent();
            DoInitialStuff();
            DataContext = this;
        }

        private void DoInitialStuff()
        {
           
        }

        private List<IWidget> Widgets { get; set; }

        public DashboardState DashboardState { get; set; }

        private DashboardLayoutManager LayoutManager = new DashboardLayoutManager();

        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void AddNewBlankWidget()
        {
            BlankWidget widget = new BlankWidget
            {
                Position = LayoutManager.GetNewWidgetPositon(CellsContainer)
            };
            LayoutManager.AddDashboardWidgetToContainer(CellsContainer, widget);
        }
    }
}
