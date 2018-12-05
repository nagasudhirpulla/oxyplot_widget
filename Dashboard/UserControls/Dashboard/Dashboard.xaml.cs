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
using Newtonsoft.Json;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using Dashboard.Measurements.PMUMeasurement;

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
            AutoFetchManager = new DashboardAutoFetchManager(Fetch_Timer_Tick, DashboardState.AutoFetchState);
        }

        private List<IWidgetContainer> Widgets { get; set; }

        public DashboardState DashboardState { get; set; } = new DashboardState();

        private DashboardLayoutManager LayoutManager = new DashboardLayoutManager();

        public void Fetch_Timer_Tick(object sender, EventArgs e)
        {
            RefreshAllWidgets();
        }

        private DashboardAutoFetchManager AutoFetchManager { get; set; }

        private void UpdateDashboardAutoFetchState(DashboardAutoFetchState state)
        {
            DashboardState.AutoFetchState = state;
            UpdateDashboardAutoFetchState();
        }

        private void UpdateDashboardAutoFetchState()
        {
            AutoFetchManager.DashboardAutoFetchState = DashboardState.AutoFetchState;
        }

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

        private DashboardState GenerateState()
        {
            DashboardState state = DashboardState;

            // Create WidgetContainerStates and append to list
            DashboardState.WidgetContainerStates.Clear();
            for (int widgetContIter = 0; widgetContIter < CellsContainer.Children.Count; widgetContIter++)
            {
                IWidgetContainer container = (IWidgetContainer)CellsContainer.Children[widgetContIter];
                IWidgetContainerState containerState = container.GenerateState();
                DashboardState.WidgetContainerStates.Add(containerState);
            }

            return state;
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenDashBoard();
        }

        private void OpenDashBoard()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // openFileDialog.Multiselect = true;
            openFileDialog.Filter = "dash files (*.dash)|*.dash|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileNames[0];
                OpenFileName(filename);
            }
        }

        public void OpenFileName(string str)
        {
            if (str != null)
            {
                // Generate the dashboard state
                DashboardState dashboardState = JsonConvert.DeserializeObject<DashboardState>(File.ReadAllText(str));
                Console.WriteLine($"Dashboard State \"{dashboardState.Name}\" loaded");

                // Clean up the Dashboard by deleting all the widget containers
                DeleteAllWidgets();

                // Create WidgetFrames based on the Dashboard state
                SetState(dashboardState);
            }
        }

        public void DeleteAllWidgets()
        {
            int totalWidgetContainerCount = CellsContainer.Children.Count;
            for (int widgetContIter = 0; widgetContIter < totalWidgetContainerCount; widgetContIter++)
            {
                // Delete the container. All CellsContainer children are expected to WidgetContainers.
                LayoutManager.DeleteWidgetFromContainer(CellsContainer, (IWidgetContainer)CellsContainer.Children[0]);
            }
        }

        public void SetState(DashboardState state)
        {
            DashboardState = state;
            // Create WidgetContainers one by one from DashbaordState and add them to the CellsContainer
            for (int containerIter = 0; containerIter < state.WidgetContainerStates.Count; containerIter++)
            {
                // Create a new WidgetContainer
                IWidgetContainer widgetContainer;
                IWidgetContainerState containerState = state.WidgetContainerStates[containerIter];
                if (containerState is WidgetFrameState)
                {
                    widgetContainer = new WidgetFrame();
                }
                else
                {
                    widgetContainer = new WidgetFrame();
                }

                // Set the WidgetContainerState
                widgetContainer.SetState(state.WidgetContainerStates[containerIter]);

                // Add the WidgetContainer to the Dashboard
                LayoutManager.AddDashboardWidgetToContainer(CellsContainer, widgetContainer, Changed);
            }
            UpdateDashboardAutoFetchState();
        }

        private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // https://stackoverflow.com/questions/4682915/defining-menuitem-shortcuts
            Save_Dashboard();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save_Dashboard();
        }

        private void Save_Dashboard()
        {
            DashboardState state = GenerateState();
            // get the filename
            string filename = DashboardState.Name;
            string jsonText = JsonConvert.SerializeObject(DashboardState, Formatting.Indented);
            SaveFileDialog savefileDialog = new SaveFileDialog
            {
                // set a default file name
                FileName = filename,
                // set filters - this can be done in properties as well
                Filter = "dash Files (*.dash)|*.dash|All files (*.*)|*.*"
            };
            if (savefileDialog.ShowDialog() == true)
            {
                File.WriteAllText(savefileDialog.FileName, jsonText);
                Console.WriteLine("Saved the updated dashboard file!!!");
            }
            //if (MessageBox.Show("Save this Dashboard?", "Save", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            //{

            //}
        }

        private void Preferences_Click(object sender, RoutedEventArgs e)
        {
            // todo open dashboard preferenes window
        }

        private void NewWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FetchBtn_Click(object sender, RoutedEventArgs e)
        {
            AutoFetchManager.StartScheduler();
        }

        private void RefreshAllWidgets()
        {
            // Refresh data of all widgetContainers
            for (int containerIter = 0; containerIter < CellsContainer.Children.Count; containerIter++)
            {
                IWidgetContainer container = (IWidgetContainer)CellsContainer.Children[containerIter];
                container.RefreshData();
            }
        }

        private void FetchStopBtn_Click(object sender, RoutedEventArgs e)
        {
            AutoFetchManager.StopScheduler();
        }

        private void AddBlankWidget_Click(object sender, RoutedEventArgs e)
        {
            AddNewBlankWidget();
        }

        private void AddPlotWidget_Click(object sender, RoutedEventArgs e)
        {
            AddNewPlotWidget();
        }

        private void PMUSettings_Click(object sender, RoutedEventArgs e)
        {
            PMUMeasurement.OpenSettingsWindow();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Exit_Dashboard();
        }

        private void Exit_Dashboard()
        {
            Window parent = Window.GetWindow(this);
            parent.Close();
        }

        private void FetchSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            DashboardAutoFetchState state = AutoFetchManager.OpenDashboardAutoFetchConfigWindow(DashboardState.AutoFetchState);
            if (state != null)
            {
                UpdateDashboardAutoFetchState(state);
            }
        }
                
    }
}
