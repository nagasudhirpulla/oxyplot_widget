using Dashboard.Interfaces;
using Dashboard.States;
using Dashboard.Widgets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using Dashboard.Widgets.Oxyplot;
using Dashboard.WidgetLayout;
using System;
using Dashboard.EditorWindows;
using System.Windows;

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

        private List<IWidgetContainer> Widgets { get; set; }

        public DashboardState DashboardState { get; set; }

        private DashboardLayoutManager LayoutManager = new DashboardLayoutManager();

        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void ChangeWidgetPosition(IWidgetContainer widget, WidgetPosition newPosition)
        {
            LayoutManager.ChangeWidgetPosition(CellsContainer, widget, newPosition);
        }

        //todo hnadle widget position change window in the dashoard itself instead of the cell


        public void AddNewBlankWidget()
        {
            WidgetFrame widgetFrame = new WidgetFrame
            {
                Position = LayoutManager.GetNewWidgetPositon(CellsContainer)
            };
            widgetFrame.SetWidget(new BlankWidget());

            LayoutManager.AddDashboardWidgetToContainer(CellsContainer, widgetFrame, Changed);
        }

        public void AddNewPlotWidget()
        {
            PlotWidget widget = new PlotWidget
            {
                Position = LayoutManager.GetNewWidgetPositon(CellsContainer)
            };
            LayoutManager.AddDashboardWidgetToContainer(CellsContainer, widget, Changed);
        }

        private void Changed(object sender, EventArgs eArgs)
        {
            //todo complete seeing https://github.com/nagasudhirpulla/wpf_scada_dashboard/blob/master/WPFScadaDashboard/DashboardUserControls/DashboardUC.xaml.cs
            if (sender is IWidgetContainer widget)
            {
                if (eArgs is CellPosChangeReqArgs cellPosChangeArgs)
                {
                    if (cellPosChangeArgs != null && widget != null)
                    {
                        WidgetPositionEditorWindow positionEditor = new WidgetPositionEditorWindow(widget.Position);
                        positionEditor.ShowDialog();
                        if (positionEditor.DialogResult == true)
                        {
                            WidgetPosition newWidgetPosition = positionEditor.WidgetPosition;
                            Console.WriteLine($"Setting New position to {newWidgetPosition.Row}, {newWidgetPosition.Column}, {newWidgetPosition.RowSpan}, {newWidgetPosition.ColSpan}");
                            ChangeWidgetPosition(widget, newWidgetPosition);
                        }
                    }
                }
            }
        }
    }
}
