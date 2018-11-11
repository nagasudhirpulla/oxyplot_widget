using Dashboard.Interfaces;
using Dashboard.Measurements.RandomMeasurement;
using OxyPlot.Series;
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
        public string Name { get; set; } = "Default";

        public List<LineSeries> GetSeriesListForPlotSetup()
        {
            List<LineSeries> seriesList = new List<LineSeries>();
            for (int iter = 0; iter < SeriesConfigs.Count; iter++)
            {
                seriesList.Add(new LineSeries { Title = $"{SeriesConfigs[iter].Name}", Color = Helpers.OxyUtility.ConvertColorToOxyColor(SeriesConfigs[iter].Appearance.Color) });
            }
            return seriesList;
        }

        public LinePlotConfig Clone()
        {
            LinePlotConfig linePlotConfig = new LinePlotConfig { Name = Name, Appearance = Appearance.Clone() };
            linePlotConfig.SeriesConfigs = (from config in SeriesConfigs select config.Clone()).ToList();
            return linePlotConfig;
        }

        public void OpenConfigEditWindow()
        {
            LinePlotConfigEditWindow configEditWindow = new LinePlotConfigEditWindow(this);
            configEditWindow.ShowDialog();
            if (configEditWindow.DialogResult == true)
            {
                Name = configEditWindow.editorVM.mLinePlotConfig.Name;
                Appearance = configEditWindow.editorVM.mLinePlotConfig.Appearance;
                SeriesConfigs = configEditWindow.editorVM.mLinePlotConfig.SeriesConfigs;
            }
        }
    }

    public class LineSeriesConfig
    {
        public string Name { get; set; } = "Default";
        public LineSeriesAppearance Appearance { get; set; } = new LineSeriesAppearance();
        public IMeasurement Measurement { get; set; } = new RandomMeasurement();

        public string GetDisplayText()
        {
            return Measurement.GetDisplayText();
        }

        public LineSeriesConfig Clone()
        {
            LineSeriesConfig config = new LineSeriesConfig { Name = Name, Appearance = Appearance.Clone(), Measurement = Measurement.Clone() };
            return config;
        }
    }

    public class LinePlotAppearance
    {
        public Color BackgroundColor { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ForegroundColor { get; set; } = Color.FromRgb(255, 255, 255);
        public Color TextColor { get; set; } = Color.FromRgb(255, 255, 255);

        public LinePlotAppearance Clone()
        {
            return new LinePlotAppearance
            {
                BackgroundColor = BackgroundColor,
                ForegroundColor = ForegroundColor,
                TextColor = TextColor
            };
        }
    }

    public class LineSeriesAppearance
    {
        public Color Color { get; set; } = Color.FromRgb(0, 0, 255);

        public LineSeriesAppearance Clone()
        {
            return new LineSeriesAppearance
            {
                Color = Color
            };
        }
    }
}
