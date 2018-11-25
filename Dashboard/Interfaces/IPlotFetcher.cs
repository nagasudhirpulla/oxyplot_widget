using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Widgets.Oxyplot
{
    public interface IPlotFetcher
    {
        //not used
        List<LineSeries> GetSeriesForSetup();
        List<DataPoint> FetchData(int seriesIndex);
        void OpenConfigEditWindow();
    }
}
