using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxyplotWidget.PlotWidget
{
    public class PlotViewModel : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Send Messages to Parent using this event handler
        public event EventHandler<EventArgs> Changed;
        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        public void CreateXYAxes()
        {
            _linePlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            _linePlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public PlotViewModel()
        {
            _linePlotModel = new PlotModel();
            //_linePlotModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

        }
        public void RefreshPlot()
        {
            _linePlotModel.InvalidatePlot(true);
            //OnPropertyChanged("LinePlotModel");
        }

        /// <summary>
        /// Plot Model for Oxyplot
        /// </summary>
        private PlotModel _linePlotModel;
        public PlotModel LinePlotModel
        {
            get { return _linePlotModel; }
            set
            {
                _linePlotModel = value;
                RefreshPlot();
            }
        }

        public string Title
        {
            set
            {
                _linePlotModel.Title = value;
                RefreshPlot();
            }
        }

        public void AddNewSeries(Series series)
        {
            _linePlotModel.Series.Add(series);
            RefreshPlot();
        }

        public void ClearSeries()
        {
            _linePlotModel.Series.Clear();
            RefreshPlot();
        }

        public int GetSeriesCount()
        {
            return _linePlotModel.Series.Count;
        }

        public void ResetZoom()
        {
            _linePlotModel.ResetAllAxes();
            RefreshPlot();
        }

        /// <summary>
        /// Add a new point to an existing line series
        /// </summary>
        /// <param name="seriesIndex"></param>
        /// <param name="dataPoint"></param>
        public void AddPointInLineSeries(int seriesIndex, DataPoint dataPoint)
        {
            if (seriesIndex < _linePlotModel.Series.Count && seriesIndex >= 0 && _linePlotModel.Series[seriesIndex] is LineSeries)
            {
                (_linePlotModel.Series[seriesIndex] as LineSeries).Points.Add(dataPoint);
                RefreshPlot();
            }
        }

        /// <summary>
        /// Add set of points to an existing line series
        /// </summary>
        /// <param name="seriesIndex"></param>
        /// <param name="dataPoints"></param>
        public void AddPointsInLineSeries(int seriesIndex, List<DataPoint> dataPoints)
        {
            if (seriesIndex < _linePlotModel.Series.Count && seriesIndex >= 0 && _linePlotModel.Series[seriesIndex] is LineSeries)
            {
                (_linePlotModel.Series[seriesIndex] as LineSeries).Points.AddRange(dataPoints);
                RefreshPlot();
            }
        }

        /// <summary>
        /// Replace points in Line series with the supplied points
        /// </summary>
        /// <param name="seriesIndex"></param>
        /// <param name="dataPoints"></param>
        public void ReplacePointsInLineSeries(int seriesIndex, List<DataPoint> dataPoints)
        {
            if (seriesIndex < _linePlotModel.Series.Count && seriesIndex >= 0 && _linePlotModel.Series[seriesIndex] is LineSeries)
            {
                (_linePlotModel.Series[seriesIndex] as LineSeries).Points.RemoveAll(d => true);
                (_linePlotModel.Series[seriesIndex] as LineSeries).Points.AddRange(dataPoints);
                RefreshPlot();
            }
        }

        public void ClearPointsInLineSeries(int seriesIndex)
        {
            if (seriesIndex < _linePlotModel.Series.Count && seriesIndex >= 0 && _linePlotModel.Series[seriesIndex] is LineSeries)
            {
                (_linePlotModel.Series[seriesIndex] as LineSeries).Points.RemoveAll(d => true);
                RefreshPlot();
            }
        }

        /// <summary>
        /// Change the line color of a series
        /// </summary>
        /// <param name="seriesIndex"></param>
        /// <param name="oxyColor"></param>
        public void SetSeriesLineColor(int seriesIndex, OxyColor oxyColor)
        {
            if (seriesIndex < _linePlotModel.Series.Count && seriesIndex >= 0 && _linePlotModel.Series[seriesIndex] is LineSeries)
            {
                (_linePlotModel.Series[0] as LineSeries).Color = oxyColor;
                RefreshPlot();
            }
        }

        /// <summary>
        /// Changes the plot background color
        /// </summary>
        /// <param name="oxyColor"></param>
        public void SetPlotBackground(OxyColor oxyColor)
        {
            _linePlotModel.Background = oxyColor;
            RefreshPlot();
        }

        /// <summary>
        /// Changes the plot text color
        /// </summary>
        /// <param name="oxyColor"></param>
        public void SetPlotTextColor(OxyColor oxyColor)
        {
            _linePlotModel.TextColor = oxyColor;
            RefreshPlot();
        }

        /// <summary>
        /// Changes the plot axes color
        /// </summary>
        /// <param name="oxyColor"></param>
        public void SetPlotAxesTickColor(OxyColor oxyColor)
        {
            for (int iter = 0; iter < _linePlotModel.Axes.Count; iter++)
            {
                _linePlotModel.Axes[iter].TicklineColor = oxyColor;
            }
            RefreshPlot();
        }

        public void SetPlotMajorAxesLineColor(OxyColor oxyColor)
        {
            for (int iter = 0; iter < _linePlotModel.Axes.Count; iter++)
            {
                if (_linePlotModel.Axes[iter].MajorGridlineStyle == LineStyle.None)
                {
                    _linePlotModel.Axes[iter].MajorGridlineStyle = LineStyle.Solid;
                }
                _linePlotModel.Axes[iter].MajorGridlineColor = oxyColor;
            }
            RefreshPlot();
        }

        public void SetPlotMinorAxesLineColor(OxyColor oxyColor)
        {
            for (int iter = 0; iter < _linePlotModel.Axes.Count; iter++)
            {
                if (_linePlotModel.Axes[iter].MinorGridlineStyle == LineStyle.None)
                {
                    _linePlotModel.Axes[iter].MinorGridlineStyle = LineStyle.Solid;
                }
                _linePlotModel.Axes[iter].MinorGridlineColor = oxyColor;
            }
            RefreshPlot();
        }

        public void SetPlotMajorAxesLineStyle(LineStyle lineStyle)
        {
            for (int iter = 0; iter < _linePlotModel.Axes.Count; iter++)
            {
                _linePlotModel.Axes[iter].MajorGridlineStyle = lineStyle;
            }
            RefreshPlot();
        }

        public void SetPlotMinorAxesLineStyle(LineStyle lineStyle)
        {
            for (int iter = 0; iter < _linePlotModel.Axes.Count; iter++)
            {
                _linePlotModel.Axes[iter].MinorGridlineStyle = lineStyle;
            }
            RefreshPlot();
        }

        public void MakeXAxisDateTime()
        {
            for (int iter = 0; iter < _linePlotModel.Axes.Count; iter++)
            {
                if (_linePlotModel.Axes[iter].Position == AxisPosition.Bottom)
                {
                    _linePlotModel.Axes[iter] = new DateTimeAxis() { StringFormat = "M/d" };
                }
            }
            RefreshPlot();
        }

        public void SetXAxisStringFormat(string stringFormat)
        {
            for (int iter = 0; iter < _linePlotModel.Axes.Count; iter++)
            {
                if (_linePlotModel.Axes[iter].Position == AxisPosition.Bottom)
                {
                    _linePlotModel.Axes[iter].StringFormat = stringFormat;
                }
            }
            RefreshPlot();
        }

        public void MakeXAxisLinear()
        {
            for (int iter = 0; iter < _linePlotModel.Axes.Count; iter++)
            {
                if (_linePlotModel.Axes[iter].Position == AxisPosition.Bottom)
                {
                    _linePlotModel.Axes[iter] = new LinearAxis() { Position = AxisPosition.Bottom };
                }
            }
            RefreshPlot();
        }

        public void SetXAxisFontSize(double fontSize)
        {
            for (int iter = 0; iter < _linePlotModel.Axes.Count; iter++)
            {
                if (_linePlotModel.Axes[iter].Position == AxisPosition.Bottom)
                {
                    _linePlotModel.Axes[iter].FontSize = fontSize;
                }
            }
            RefreshPlot();
        }

        public void SetYAxisFontSize(double fontSize)
        {
            for (int iter = 0; iter < _linePlotModel.Axes.Count; iter++)
            {
                if (_linePlotModel.Axes[iter].Position == AxisPosition.Left)
                {
                    _linePlotModel.Axes[iter].FontSize = fontSize;
                }
            }
            RefreshPlot();
        }
    }
}

/*
 * Line series examples for oxyplot
 * https://github.com/ylatuya/oxyplot/blob/master/Source/Examples/ExampleLibrary/Examples/LineSeriesExamples.cs 
 * 
 * Plot Axes sample codes
 * https://searchcode.com/codesearch/view/28446353/
     */
