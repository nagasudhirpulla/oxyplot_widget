using Dashboard.DataFetchers.RandomDataFetcher;
using Dashboard.Interfaces;
using Dashboard.States;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyplotWidget.PlotWidget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Dashboard.Widgets.Oxyplot
{
    /// <summary>
    /// Interaction logic for OxyplotWidget.xaml
    /// </summary>
    public partial class PlotWidget : UserControl, IWidget, INotifyPropertyChanged
    {
        public PlotWidget()
        {
            InitializeComponent();
            DataContext = this;
            PlotViewModel.CreateXYAxes();
            SetupPlotView();
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

        public PlotViewModel PlotViewModel { get; set; } = new PlotViewModel();
        //private IPlotFetcher mPlotFetcher = new RandomPlotDataFetcher();
        private LinePlotConfig mLinePlotConfig = new LinePlotConfig();

        private void SetupPlotView()
        {
            //set the x axis as datetime if necessary
            if (mLinePlotConfig.Appearance.IsXAxisDateTime)
            {
                PlotViewModel.MakeXAxisDateTime();
                // settimg the axis time format string
                PlotViewModel.SetXAxisStringFormat(mLinePlotConfig.Appearance.AxisTimeFormat);
            }
            else
            {
                PlotViewModel.MakeXAxisLinear();
            }

            PlotViewModel.SetXAxisFontSize(mLinePlotConfig.Appearance.XLabelFontSize);
            PlotViewModel.SetYAxisFontSize(mLinePlotConfig.Appearance.YLabelFontSize);

            PlotViewModel.ClearSeries();
            List<LineSeries> seriesList = mLinePlotConfig.GetSeriesListForPlotSetup();

            // Add all the series to the PlotViewModel
            for (int seriesIter = 0; seriesIter < seriesList.Count; seriesIter++)
            {
                PlotViewModel.AddNewSeries(seriesList[seriesIter]);
            }
            PlotViewModel.SetPlotBackground(Helpers.OxyUtility.ConvertColorToOxyColor(mLinePlotConfig.Appearance.BackgroundColor));
            PlotViewModel.SetPlotTextColor(Helpers.OxyUtility.ConvertColorToOxyColor(mLinePlotConfig.Appearance.TextColor));
            PlotViewModel.SetPlotAxesTickColor(Helpers.OxyUtility.ConvertColorToOxyColor(mLinePlotConfig.Appearance.ForegroundColor));

            PlotViewModel.SetPlotMajorAxesLineColor(Helpers.OxyUtility.ConvertColorToOxyColor(mLinePlotConfig.Appearance.MajorAxesLineColor));

        }

        public async Task RefreshData()
        {
            //int waitTime = 300;
            //Random rnd = new Random();
            //List<DataPoint> points = new List<DataPoint> {
            //    new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddHours(rnd.Next(-13, -1))), 5),
            //    new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddHours(rnd.Next(-13, -1))), 10),
            //    new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddHours(rnd.Next(-13, -1))), 64),
            //    };
            //int seriesIndex = PlotViewModel.LinePlotModel.Series.Count - 1;
            //for (int iter = 0; iter < points.Count; iter++)
            //{
            //    await Task.Delay(waitTime);
            //    // Add point in series
            //    PlotViewModel.AddPointInLineSeries(seriesIndex, points[iter]);
            //}
            //PlotViewModel.MakeXAxisDateTime();
            //PlotViewModel.SetXAxisStringFormat("dd-MMM-yyyy");
            for (int seriesIter = 0; seriesIter < PlotViewModel.GetSeriesCount(); seriesIter++)
            {
                bool isTimeSeriesDataExpected = mLinePlotConfig.Appearance.IsXAxisDateTime;
                List<DataPoint> points = await mLinePlotConfig.SeriesConfigs[seriesIter].FetchData(isTimeSeriesDataExpected);
                PlotViewModel.ReplacePointsInLineSeries(seriesIter, points);
            }
        }

        public async Task DoCleanUpForDeletion()
        {
            // todo ask each measurement to clean up the resources and opened connections
        }

        public void OpenConfigWindow()
        {
            //mPlotFetcher.OpenConfigEditWindow();
            mLinePlotConfig.OpenConfigEditWindow();
            SetupPlotView();
            RefreshData();
        }

        public IWidgetState GenerateState()
        {
            OxyPlotWidgetState state = new OxyPlotWidgetState();

            // Generate the IPlotConfig state
            state.PlotConfig = mLinePlotConfig;

            return state;
        }

        public void SetState(IWidgetState state)
        {
            if (state is OxyPlotWidgetState widgetState)
            {
                if (widgetState.PlotConfig is LinePlotConfig plotConfig)
                {
                    mLinePlotConfig = plotConfig;
                    SetupPlotView();
                }
                else { Console.WriteLine("Inflation rejected since non LinePlotConfig given for inflation..."); }
            }
            else { Console.WriteLine("Inflation rejected since non OxyPlotWidgetState given for inflation..."); }
        }

        private void ResetZoom_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PlotViewModel.ResetZoom();
        }

        private async void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = PlotViewModel.GetPlotDataTable();
            
            Microsoft.Office.Interop.Excel.Application excel = null;
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.Worksheet ws = null;
            //Microsoft.Office.Interop.Excel.Range rng = null;
            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                wb = excel.Workbooks.Add();
                ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet;
                for (int Idx = 0; Idx < dt.Columns.Count; Idx++)
                {
                    ws.Range["A1"].Offset[0, Idx].Value = dt.Columns[Idx].ColumnName;
                }
                for (int Idx = 0; Idx < dt.Rows.Count; Idx++)
                {   
                   // Add the whole row at once
                    ws.Range["A2"].Offset[Idx].Resize[1, dt.Columns.Count].Value =
                    dt.Rows[Idx].ItemArray;
                }
                excel.Visible = true;
                wb.Activate();
            }
            catch (COMException ex)
            {
                MessageBox.Show("Error accessing Excel: " + ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }
    }
}
