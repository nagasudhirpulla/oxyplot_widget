using Dashboard.Interfaces;
using Dashboard.States;
using Dashboard.Widgets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using Dashboard.Widgets.Oxyplot;
using Dashboard.WidgetLayout;
using System;

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

        public void ChangeWidgetPosition(IWidget widget, WidgetPosition newPosition)
        {            
            LayoutManager.ChangeWidgetPosition(CellsContainer, widget, newPosition);
        }

        //todo hnadle widget position change window in the dashoard itself instead of the cell


        public void AddNewBlankWidget()
        {
            BlankWidget widget = new BlankWidget
            {
                Position = LayoutManager.GetNewWidgetPositon(CellsContainer)
            };
            //todo wire up the event creation handler to the cell so that it can send messages to this dashboard

            LayoutManager.AddDashboardWidgetToContainer(CellsContainer, widget);
        }

        public void AddNewPlotWidget()
        {
            PlotWidget widget = new PlotWidget
            {
                Position = LayoutManager.GetNewWidgetPositon(CellsContainer)
            };
            LayoutManager.AddDashboardWidgetToContainer(CellsContainer, widget);
        }

        private void Changed(object sender, EventArgs eArgs)
        {
            //todo complete seeing https://github.com/nagasudhirpulla/wpf_scada_dashboard/blob/master/WPFScadaDashboard/DashboardUserControls/DashboardUC.xaml.cs
        }
    }
}
