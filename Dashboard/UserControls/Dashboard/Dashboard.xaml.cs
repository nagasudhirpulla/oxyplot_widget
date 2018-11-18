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
            Console.WriteLine($"Setting New position to {newPosition.Row}, {newPosition.Column}, {newPosition.RowSpan}, {newPosition.ColSpan}");
            LayoutManager.ChangeWidgetPosition(CellsContainer, widget, newPosition);
        }

        public void DeleteWidget(IWidgetContainer widget)
        {
            LayoutManager.DeleteWidgetFromContainer(CellsContainer, widget);
        }
        
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
            WidgetFrame widgetFrame = new WidgetFrame
            {
                Position = LayoutManager.GetNewWidgetPositon(CellsContainer)
            };
            widgetFrame.SetWidget(new PlotWidget());
            LayoutManager.AddDashboardWidgetToContainer(CellsContainer, widgetFrame, Changed);
        }

        private void Changed(object sender, EventArgs eArgs)
        {
            // https://github.com/nagasudhirpulla/wpf_scada_dashboard/blob/master/WPFScadaDashboard/DashboardUserControls/DashboardUC.xaml.cs
            if (sender is IWidgetContainer widget)
            {
                if (eArgs is CellPosChangeReqArgs cellPosChangeArgs)
                {
                    if (cellPosChangeArgs != null && widget != null)
                    {
                        if (cellPosChangeArgs.MessageType == CellPosChangeMsgType.POS_DOWN)
                        {
                            WidgetPosition newWidgetPosition = new WidgetPosition(widget.Position);
                            newWidgetPosition.Row += 1;
                            ChangeWidgetPosition(widget, newWidgetPosition);
                        }
                        if (cellPosChangeArgs.MessageType == CellPosChangeMsgType.POS_UP)
                        {
                            WidgetPosition newWidgetPosition = new WidgetPosition(widget.Position);
                            newWidgetPosition.Row -= 1;
                            ChangeWidgetPosition(widget, newWidgetPosition);
                        }
                        if (cellPosChangeArgs.MessageType == CellPosChangeMsgType.POS_LEFT)
                        {
                            WidgetPosition newWidgetPosition = new WidgetPosition(widget.Position);
                            newWidgetPosition.Column -= 1;
                            ChangeWidgetPosition(widget, newWidgetPosition);
                        }
                        if (cellPosChangeArgs.MessageType == CellPosChangeMsgType.POS_RIGHT)
                        {
                            WidgetPosition newWidgetPosition = new WidgetPosition(widget.Position);
                            newWidgetPosition.Column += 1;
                            ChangeWidgetPosition(widget, newWidgetPosition);
                        }
                        if (cellPosChangeArgs.MessageType == CellPosChangeMsgType.POS_DELETE)
                        {
                            DeleteWidget(widget);
                        }
                        if (cellPosChangeArgs.MessageType == CellPosChangeMsgType.POS_EDIT_WIN)
                        {
                            WidgetPositionEditorWindow positionEditor = new WidgetPositionEditorWindow(widget.Position);
                            positionEditor.ShowDialog();
                            if (positionEditor.DialogResult == true)
                            {
                                WidgetPosition newWidgetPosition = positionEditor.WidgetPosition;
                                ChangeWidgetPosition(widget, newWidgetPosition);
                            }
                        }
                    }
                }

                if (eArgs is CellMessageArgs cellMessageArgs)
                {
                    if (cellMessageArgs != null && widget != null && cellMessageArgs.Message != null)
                    {
                        if (cellMessageArgs.Message == CellMessageArgs.ConfigWindowOpenRequest)
                        {
                            widget.OpenConfigWindow();
                        }
                    }
                }
            }
        }
    }
}
