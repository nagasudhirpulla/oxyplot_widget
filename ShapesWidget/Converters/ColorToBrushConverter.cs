using System;
using System.Windows.Data;
using System.Windows.Media;

namespace ShapeLayersWidget.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Color color)
            {
                return new SolidColorBrush(color);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                return brush.Color;
            }
            return null;
        }
    }
}
