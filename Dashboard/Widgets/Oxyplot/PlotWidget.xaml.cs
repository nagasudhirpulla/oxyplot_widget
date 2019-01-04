using Dashboard.Interfaces;
using Dashboard.States;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyplotWidget.PlotWidget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
            await Task.Yield();
            DataTable dt = PlotViewModel.GetPlotDataTable();

            Microsoft.Office.Interop.Excel.Application excel = null;
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.Worksheet ws = null;
            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                wb = excel.Workbooks.Add();
                ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet;
                for (int Idx = 0; Idx < dt.Columns.Count; Idx++)
                {
                    ws.Range["A1"].Offset[0, Idx].Value = dt.Columns[Idx].ColumnName;
                }
                /*
                for (int Idx = 0; Idx < dt.Rows.Count; Idx++)
                {
                    // Add the whole row at once
                    ws.Range["A2"].Offset[Idx].Resize[1, dt.Columns.Count].Value =
                    dt.Rows[Idx].ItemArray;
                }
                */
                Microsoft.Office.Interop.Excel.Range top = ws.Cells[2, 1];
                Microsoft.Office.Interop.Excel.Range bottom = ws.Cells[dt.Rows.Count + 1, dt.Columns.Count];
                Microsoft.Office.Interop.Excel.Range all = ws.get_Range(top, bottom);
                object[,] arrayDT = new object[dt.Rows.Count, dt.Columns.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                    for (int j = 0; j < dt.Columns.Count; j++)
                        arrayDT[i, j] = dt.Rows[i][j];
                all.Value2 = arrayDT;
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
                DataTable dt = PlotViewModel.GetPlotDataTable();
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
