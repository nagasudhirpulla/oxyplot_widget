using Dashboard.Interfaces;
using Dashboard.Measurements.RandomMeasurement;
using OxyPlot;
using OxyPlot.Axes;
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
        public TimeShift DisplayTimeShift { get; set; } = new TimeShift();
        public IMeasurement Measurement { get; set; } = new RandomMeasurement();
        public async Task<List<DataPoint>> FetchData(bool applyTimeShift)
        {
            // fetch the data
            List<DataPoint> dataPoints = await Measurement.FetchData();
            //check if time shift is zero
            bool isTimeShiftZero = false;
            if (DisplayTimeShift.Years == 0 && DisplayTimeShift.Months == 0 && DisplayTimeShift.Days == 0 && DisplayTimeShift.Hours == 0 && DisplayTimeShift.Minutes == 0 && DisplayTimeShift.Seconds == 0)
            {
                isTimeShiftZero = true;
            }
            // Now do the time shifting
            if (applyTimeShift && isTimeShiftZero == false)
            {
                for (int dataPointIter = 0; dataPointIter < dataPoints.Count; dataPointIter++)
                {
                    DataPoint point = dataPoints[dataPointIter];
                    DateTime pointTime = DateTimeAxis.ToDateTime(point.X);
                    pointTime = pointTime.AddYears(DisplayTimeShift.Years).AddMonths(DisplayTimeShift.Months).AddDays(DisplayTimeShift.Days).AddHours(DisplayTimeShift.Hours).AddMinutes(DisplayTimeShift.Minutes).AddSeconds(DisplayTimeShift.Seconds);
                    dataPoints[dataPointIter] = new DataPoint(DateTimeAxis.ToDouble(pointTime), dataPoints[dataPointIter].Y);
                }
            }
            return dataPoints;
        }

        public string GetDisplayText()
        {
            return Measurement.GetDisplayText();
        }

        public LineSeriesConfig Clone()
        {
            LineSeriesConfig config = new LineSeriesConfig { Name = Name, Appearance = Appearance.Clone(), Measurement = Measurement.Clone(), DisplayTimeShift = DisplayTimeShift.Clone() };
            return config;
        }
    }

    public class LinePlotAppearance
    {
        public Color BackgroundColor { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ForegroundColor { get; set; } = Color.FromRgb(255, 255, 255);
        public Color TextColor { get; set; } = Color.FromRgb(255, 255, 255);
        public bool IsXAxisDateTime { get; set; } = true;
        public string AxisTimeFormat { get; set; } = "dd-MMM-yyyy";
        //todo set time format option visibility as per the IsXAxisDateTime value

        public LinePlotAppearance Clone()
        {
            return new LinePlotAppearance
            {
                BackgroundColor = BackgroundColor,
                ForegroundColor = ForegroundColor,
                TextColor = TextColor,
                IsXAxisDateTime = IsXAxisDateTime,
                AxisTimeFormat = AxisTimeFormat
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

    public class TimeShift
    {
        public int Years { get; set; } = 0;
        public int Months { get; set; } = 0;
        public int Days { get; set; } = 0;
        public int Hours { get; set; } = 0;
        public int Minutes { get; set; } = 0;
        public int Seconds { get; set; } = 0;

        public TimeShift Clone()
        {
            TimeShift timeShift = new TimeShift
            {
                Years = Years,
                Months = Months,
                Days = Days,
                Hours = Hours,
                Minutes = Minutes,
                Seconds = Seconds
            };
            return timeShift;
        }
    }
}
