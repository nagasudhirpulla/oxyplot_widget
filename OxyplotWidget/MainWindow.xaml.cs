using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyplotWidget.PlotWidget;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace OxyplotWidget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PlotWidget.PlotViewModel.Title = "Main Window Plot Test";
            PlotWidget.PlotViewModel.AddNewSeries(new LineSeries
            {
                Title = "First"
            });
            DoPlotWidgetTests();
        }

        public async void DoPlotWidgetTests()
        {
            await TestDateTimeSeriesPlot();
            //TestSineFunctionPlot();
            //await TestPlotExtraPoints();
            //await TestPlotColorFeatures();
            //await TestPlotDelayedPoints();
            //await TestPlotMillionPoints();
        }

        public void TestSineFunctionPlot()
        {
            PlotWidget.PlotViewModel.AddNewSeries(new FunctionSeries(Math.Cos, 0, 50, 0.1, "cos(x)"));
        }

        public async Task TestDateTimeSeriesPlot()
        {
            PlotWidget.PlotViewModel.AddNewSeries(new LineSeries
            {
                Title = "TimeSeries"
            });
            int waitTime = 300;
            List<DataPoint> points = new List<DataPoint> {
                new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddHours(-1)), 5),
                new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddHours(-2)), 10),
                new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddHours(-3)), 64),
                };
            int seriesIndex = PlotWidget.PlotViewModel.LinePlotModel.Series.Count - 1;
            for (int iter = 0; iter < points.Count; iter++)
            {
                await Task.Delay(waitTime);
                // Add point in series
                PlotWidget.PlotViewModel.AddPointInLineSeries(seriesIndex, points[iter]);
            }
            PlotWidget.PlotViewModel.MakeXAxisDateTime();
            PlotWidget.PlotViewModel.SetXAxisStringFormat("dd-MMM-yyyy");
        }

        public async Task TestPlotExtraPoints()
        {
            int waitTime = 30;
            List<DataPoint> points = new List<DataPoint> { new DataPoint(10, 5), new DataPoint(84, 16), new DataPoint(60, 34), new DataPoint(50, 94) };
            for (int iter = 0; iter < points.Count; iter++)
            {
                await Task.Delay(waitTime);
                // Add point in series
                PlotWidget.PlotViewModel.AddPointInLineSeries(0, points[iter]);
            }

            await Task.Delay(waitTime);
            // Add multiple points in series
            PlotWidget.PlotViewModel.AddPointsInLineSeries(0, new List<DataPoint> { new DataPoint(24, 60), new DataPoint(57, 18) });

            await Task.Delay(waitTime);
            // Replace all the points in series
            PlotWidget.PlotViewModel.ReplacePointsInLineSeries(0, new List<DataPoint> { new DataPoint(1, 1), new DataPoint(3, 2) });
        }

        public async Task TestPlotDelayedPoints()
        {
            int waitTime = 2;

            double min = 10;
            double max = 60;
            double maxMinDiff = max - min;

            // Clear all the points in series
            PlotWidget.PlotViewModel.ClearPointsInLineSeries(0);

            Random random = new Random();
            for (int iter = 0; iter < 1000; iter++)
            {
                await Task.Delay(waitTime);
                // Add point in series
                PlotWidget.PlotViewModel.AddPointInLineSeries(0, new DataPoint(iter, min + maxMinDiff * random.NextDouble()));
            }
        }

        public async Task TestPlotMillionPoints()
        {
            await Task.Delay(1);
            double min = 10;
            double max = 60;
            double maxMinDiff = max - min;
            Random random = new Random();

            // Clear all the points in series
            PlotWidget.PlotViewModel.ClearPointsInLineSeries(0);

            // generate millions points list
            List<DataPoint> points = new List<DataPoint>();
            for (int iter = 0; iter < 1000000; iter++)
            {
                points.Add(new DataPoint(iter, min + maxMinDiff * random.NextDouble()));
            }

            // Add points in series
            PlotWidget.PlotViewModel.AddPointsInLineSeries(0, points);
        }

        public async Task TestPlotColorFeatures()
        {
            int waitTime = 200;

            await Task.Delay(waitTime);
            // change the plot line color
            PlotWidget.PlotViewModel.SetSeriesLineColor(0, OxyColor.FromRgb(0, 0, 255));

            await Task.Delay(waitTime);
            // change the plot background color
            PlotWidget.PlotViewModel.SetPlotBackground(OxyColor.FromRgb(10, 10, 10));

            await Task.Delay(waitTime);
            // change plot axes tick color
            PlotWidget.PlotViewModel.SetPlotAxesTickColor(OxyColor.FromRgb(200, 200, 10));

            await Task.Delay(waitTime);
            // change plot major axes line color
            PlotWidget.PlotViewModel.SetPlotMajorAxesLineColor(OxyColor.FromRgb(200, 50, 50));

            await Task.Delay(waitTime);
            // change plot minor axes line color
            PlotWidget.PlotViewModel.SetPlotMinorAxesLineColor(OxyColor.FromArgb(150, 255, 255, 255));

            await Task.Delay(waitTime);
            // change plot major axes line sty;e
            PlotWidget.PlotViewModel.SetPlotMajorAxesLineStyle(LineStyle.LongDashDot);

            await Task.Delay(waitTime);
            // change plot minor axes line style
            PlotWidget.PlotViewModel.SetPlotMinorAxesLineStyle(LineStyle.None);

            await Task.Delay(waitTime);
            // change plot text color
            PlotWidget.PlotViewModel.SetPlotTextColor(OxyColor.FromRgb(10, 200, 200));

        }
    }
}

//http://blog.bartdemeyer.be/2013/03/creating-graphs-in-wpf-using-oxyplot/
//https://www.programering.com/a/MDM3YjMwATU.html