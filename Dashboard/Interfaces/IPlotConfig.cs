using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Interfaces
{
    public interface IPlotConfig
    {
        string TypeName { get; set; }

        string Name { get; set; }

        void OpenConfigEditWindow();

        List<LineSeries> GetSeriesListForPlotSetup();
        
    }
}
