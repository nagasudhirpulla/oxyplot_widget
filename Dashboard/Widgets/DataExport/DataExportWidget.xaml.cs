using Dashboard.Interfaces;
using Dashboard.States;
using Microsoft.Win32;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
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

namespace Dashboard.Widgets.DataExport
{
    /// <summary>
    /// Interaction logic for DataExportWidget.xaml
    /// </summary>
    public partial class DataExportWidget : UserControl, IWidget, INotifyPropertyChanged
    {
        public DataExportWidget()
        {
            InitializeComponent();
            DataContext = DataExportWidgetVM;
            SetupDataView();
        }

        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Send Messages to Dashboard using this event handler
        public Action<EventArgs> Changed { get; set; }

        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(e);
        }

        public DataExportWidgetVM DataExportWidgetVM { get; set; } = new DataExportWidgetVM();

        private DataExportConfig mDataExportConfig = new DataExportConfig();

        public async Task RefreshData()
        {
            for (int seriesIter = 0; seriesIter < mDataExportConfig.SeriesConfigs.Count; seriesIter++)
            {
                List<DataPoint> points = await mDataExportConfig.SeriesConfigs[seriesIter].FetchData(false);
                DataExportWidgetVM.ReplaceSeriesPoints(seriesIter, points);
                DataExportView.ItemsSource = DataExportWidgetVM.DataDisplayTable.DefaultView;
            }
        }

        public async Task DoCleanUpForDeletion()
        {
            await Task.Yield();
            // todo ask each measurement to clean up the resources and opened connections
        }

        public void OpenConfigWindow()
        {
            //mPlotFetcher.OpenConfigEditWindow();
            mDataExportConfig.OpenConfigEditWindow();
            SetupDataView();
            RefreshData();
        }

        public IWidgetState GenerateState()
        {
            DataExportWidgetState state = new DataExportWidgetState();

            // Generate the IPlotConfig state
            state.DataExportConfig_ = mDataExportConfig;

            return state;
        }

        public void SetState(IWidgetState state)
        {
            if (state is DataExportWidgetState widgetState)
            {
                if (widgetState.DataExportConfig_ is DataExportConfig plotConfig)
                {
                    mDataExportConfig = plotConfig;
                    SetupDataView();
                }
                else { Console.WriteLine("Inflation rejected since non DataExportConfig given for inflation..."); }
            }
            else { Console.WriteLine("Inflation rejected since non DataExportWidgetState given for inflation..."); }
        }

        private void SetupDataView()
        {
            DataExportWidgetVM.ClearSeries();
            // Add all the series to the PlotViewModel
            for (int seriesIter = 0; seriesIter < mDataExportConfig.SeriesConfigs.Count; seriesIter++)
            {
                DataExportWidgetVM.AddNewSeries(mDataExportConfig.SeriesConfigs[seriesIter].Name, null);
            }
            DataExportView.ItemsSource = DataExportWidgetVM.DataDisplayTable.DefaultView;
        }

        private async void ExportText_Click(object sender, RoutedEventArgs e)
        {
            // get the filepath
            string filename = $"plotdata_{DateTime.Now.ToString("dd_MMM_yy_HH_mm_ss")}.csv";
            SaveFileDialog savefileDialog = new SaveFileDialog
            {
                // set a default file name
                FileName = filename,
                // set filters - this can be done in properties as well
                Filter = "csv Files (*.csv)|*.csv|All files (*.*)|*.*"
            };
            if (savefileDialog.ShowDialog() == true)
            {
                await Task.Yield();
                DataTable dt = DataExportWidgetVM.GetPlotDataTable();
                string seperator = ",";
                try
                {
                    StringBuilder sb = new StringBuilder();
                    // create the headers line
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sb.Append(dt.Columns[i]);
                        if (i < dt.Columns.Count - 1)
                            sb.Append(seperator);
                    }
                    sb.AppendLine();
                    // create the data string
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dr[i].GetType() == typeof(DateTime))
                            {
                                sb.Append(((DateTime)dr[i]).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                            }
                            else
                            {
                                sb.Append(dr[i].ToString());
                            }

                            if (i < dt.Columns.Count - 1)
                                sb.Append(seperator);
                        }
                        sb.AppendLine();
                    }
                    File.WriteAllText(savefileDialog.FileName, sb.ToString());
                    MessageBox.Show("Saved the updated Plot data!!!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.ToString());
                }
            }
        }
    }
}
