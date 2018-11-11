using Dashboard.DataFetchers.RandomDataFetcher;
using Dashboard.Interfaces;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyplotWidget.PlotWidget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
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
        private IPlotFetcher mPlotFetcher = new RandomPlotDataFetcher();
        private LinePlotConfig mLinePlotConfig = new LinePlotConfig();

        private void SetupPlotView()
        {
            PlotViewModel.ClearSeries();
            List<LineSeries> seriesList = mLinePlotConfig.GetSeriesListForPlotSetup();
            for (int seriesIter = 0; seriesIter < seriesList.Count; seriesIter++)
            {
                PlotViewModel.AddNewSeries(seriesList[seriesIter]);
            }
            PlotViewModel.SetPlotBackground(Helpers.OxyUtility.ConvertColorToOxyColor(mLinePlotConfig.Appearance.BackgroundColor));
            PlotViewModel.SetPlotTextColor(Helpers.OxyUtility.ConvertColorToOxyColor(mLinePlotConfig.Appearance.TextColor));
            PlotViewModel.SetPlotAxesTickColor(Helpers.OxyUtility.ConvertColorToOxyColor(mLinePlotConfig.Appearance.ForegroundColor));
            //todo incorporate in plotConfig separately
            //PlotViewModel.SetPlotMajorAxesLineColor(Helpers.OxyUtility.ConvertColorToOxyColor(mLinePlotConfig.Appearance.ForegroundColor));
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
                List<DataPoint> points = await mLinePlotConfig.SeriesConfigs[seriesIter].Measurement.FetchData();
                PlotViewModel.ReplacePointsInLineSeries(seriesIter, points);
            }
        }

        public async Task DoCleanUpForDeletion()
        {
            // do nothing
        }

        public void OpenConfigWindow()
        {
            //mPlotFetcher.OpenConfigEditWindow();
            mLinePlotConfig.OpenConfigEditWindow();
            SetupPlotView();
            RefreshData();
        }
    }
}
