using ShapeLayersWidget.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ShapeLayersWidget.Converters
{
    public class PolyPointsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PointCollection points = new PointCollection();
            if (value is List<PointState> pointStates)
            {
                for (int pntIter = 0; pntIter < pointStates.Count; pntIter++)
                {
                    PointState pntState = pointStates[pntIter];
                    points.Add(new Point(pntState.X, pntState.Y));
                }
            }
            return points;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<PointState> pointStates = new List<PointState>();
            if (value is List<Point> points)
            {
                for (int pntIter = 0; pntIter < points.Count; pntIter++)
                {
                    Point pntState = points[pntIter];
                    pointStates.Add(new PointState { X = pntState.X, Y = pntState.Y });
                }
            }
            return pointStates;
        }
    }
}
