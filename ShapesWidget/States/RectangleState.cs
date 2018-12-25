using ShapeLayersWidget.Converters;
using ShapeLayersWidget.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeLayersWidget.States
{
    class RectangleState : BaseShapeState, IShapeState
    {
        public string TypeName { get; set; } = typeof(RectangleState).Name;

        public string Name { get; set; } = "Rectangle";

        public PointState Left { get; set; } = new PointState();

        public double LengthX { get; set; } = 1;

        public double LengthY { get; set; } = 1;

        public Color FillColor { get; set; } = Color.FromRgb(100,100,100);

        public Color HoverFillColor { get; set; } = Color.FromRgb(100,0,0);

        public Color StrokeColor { get; set; }

        public Color HoverStrokeColor { get; set; }

        public Shape CreateShape(IShapeState shapeState)
        {
            Shape shape = null;
            if (shapeState is RectangleState rectState)
            {
                shape = new Rectangle();
                BindingOperations.SetBinding(shape, FrameworkElement.WidthProperty, new Binding("LengthX") { Source = rectState });
                BindingOperations.SetBinding(shape, FrameworkElement.HeightProperty, new Binding("LengthY") { Source = rectState });
            }
            if (shape != null)
            {
                SetShapeProperties(shape, shapeState);
            }
            return shape;
        }
    }
}
