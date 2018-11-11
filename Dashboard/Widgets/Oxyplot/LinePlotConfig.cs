using Dashboard.Interfaces;
using Dashboard.Measurements.RandomMeasurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dashboard.Widgets.Oxyplot
{
    public class LinePlotConfig : IPlotConfig
    {
        public LinePlotAppearance Appearance { get; set; } = new LinePlotAppearance();
        public List<LineSeriesConfig> SeriesConfigs { get; set; } = new List<LineSeriesConfig>();
        public void OpenConfigEditWindow()
        {
            LinePlotConfigEditWindow configEditWindow = new LinePlotConfigEditWindow(new LinePlotConfig { Appearance = Appearance, SeriesConfigs = SeriesConfigs });
            configEditWindow.ShowDialog();
            // todo deal with the dialog result
        }
    }

    public class LineSeriesConfig
    {
        public LineSeriesAppearance Appearance { get; set; } = new LineSeriesAppearance();
        public IMeasurement Measurement { get; set; } = new RandomMeasurement();

        public string GetDisplayText()
        {
            return Measurement.GetDisplayText();
        }
    }

    public class LinePlotAppearance
    {
        public Color BackgroundColor { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ForegroundColor { get; set; } = Color.FromRgb(255, 255, 255);
        public Color TextColor { get; set; } = Color.FromRgb(255, 255, 255);
    }

    public class LineSeriesAppearance
    {
        public Color Color { get; set; } = Color.FromRgb(0, 0, 255);
    }
}
