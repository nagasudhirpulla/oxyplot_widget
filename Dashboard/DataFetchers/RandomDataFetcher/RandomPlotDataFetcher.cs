using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using Dashboard.Widgets.Oxyplot;
using Dashboard.EditorWindows;

namespace Dashboard.DataFetchers.RandomDataFetcher
{
    public class RandomPlotDataFetcher : IPlotFetcher
    {
        public RandomPlotFetchConfig Config { get; set; } = new RandomPlotFetchConfig();

        public List<DataPoint> FetchData(int seriesIndex)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            if (Config.Bounds.Count <= seriesIndex)
            {
                return dataPoints;
            }
            int min = Config.Bounds[seriesIndex].Item1;
            int max = Config.Bounds[seriesIndex].Item2;
            int numPoints = Config.Bounds[seriesIndex].Item3;
            Random random = new Random();
            for (int pointIter = 0; pointIter < numPoints; pointIter++)
            {
                dataPoints.Add(new DataPoint(pointIter, random.Next(min, max)));
            }
            return dataPoints;
        }

        public List<LineSeries> GetSeriesForSetup()
        {
            List<LineSeries> seriesList = new List<LineSeries>();
            for (int boundsIter = 0; boundsIter < Config.Bounds.Count; boundsIter++)
            {
                seriesList.Add(new LineSeries { Title = $"Series {boundsIter}" });
            }
            return seriesList;
        }

        public void OpenConfigEditWindow()
        {
            RandomPlotFetchConfigWindow configWindow = new RandomPlotFetchConfigWindow(Config.Bounds);
            configWindow.ShowDialog();
            if (configWindow.DialogResult == true)
            {
                Config.Bounds = configWindow.GetConfigTuples();
            }
        }
    }
}
