using ShapeLayersWidget.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ShapeLayersWidget.States
{
    class RectangleState : IShapeState
    {
        public string TypeName { get; set; } = typeof(RectangleState).Name;

        public string Name { get; set; } = "Rectangle";

        public PointState Left { get; set; } = new PointState();

        public double LengthX { get; set; } = 1;

        public double LengthY { get; set; } = 1;

        public Shape CreateShape(IShapeState shapeState)
        {
            Shape shape = null;
            if (shapeState is RectangleState rectState)
            {
                shape = new Rectangle();
                BindingOperations.SetBinding(shape, FrameworkElement.WidthProperty, new Binding { Source = rectState.LengthX });
                BindingOperations.SetBinding(shape, FrameworkElement.HeightProperty, new Binding { Source = rectState.LengthY });
            }
            if (shape != null)
            {
                BindingOperations.SetBinding(shape, Canvas.LeftProperty, new Binding { Source = shapeState.Left.X });
                BindingOperations.SetBinding(shape, Canvas.TopProperty, new Binding { Source = shapeState.Left.Y });
            }
            return shape;
        }
    }
}
