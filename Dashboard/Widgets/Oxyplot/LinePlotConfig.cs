using Dashboard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dashboard.Widgets.Oxyplot
{
    public class LinePlotConfig : IPlotConfig
    {
        public LinePlotAppearance Appearance { get; set; }
        public List<LineSeriesConfig> SeriesConfigs { get; set; }
        public void OpenConfigEditWindow()
        {
            throw new NotImplementedException();
        }
    }

    public class LineSeriesConfig
    {
        public LineSeriesAppearance Appearance { get; set; }
        public List<IMeasurement> Measurements { get; set; }
    }

    public class LinePlotAppearance
    {
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }
        public Color TextColor { get; set; }
    }

    public class LineSeriesAppearance
    {
        public Color Color { get; set; }
    }
}
