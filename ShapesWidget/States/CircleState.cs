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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeLayersWidget.States
{
    public class CircleState : BaseShapeState, IShapeState
    {
        public string TypeName { get; set; } = typeof(CircleState).Name;

        public string Name { get; set; } = "Circle";

        public PointState Center { get; set; } = new PointState();

        public double Radius { get; set; } = 1;

        public Color FillColor { get; set; } = Color.FromRgb(100, 100, 100);

        public Color HoverFillColor { get; set; } = Color.FromRgb(100, 0, 0);

        public Color StrokeColor { get; set; }

        public Color HoverStrokeColor { get; set; }

        public PointState Left
        {
            get { return new PointState { X = Center.X - Radius, Y = Center.Y - Radius }; }
            set { Center.X = value.X + Radius; Center.Y = value.Y + Radius; }
        }

        public double Diameter { get { return 2 * Radius; } set { Radius = 0.5 * value; } }
        public string DiameterVariableName = "Diameter";

        public Shape CreateShape(IShapeState shapeState)
        {
            Shape shape = null;
            if (shapeState is CircleState circleState)
            {
                shape = new Ellipse();
                //Binding diameterBinding = new Binding { Source = circleState, Path = new PropertyPath("Diameter") };
                Binding diameterBinding = new Binding(DiameterVariableName) { Source = circleState };
                //Binding diameterBinding = new Binding { Source = circleState.Diameter };
                BindingOperations.SetBinding(shape, FrameworkElement.HeightProperty, diameterBinding);
                BindingOperations.SetBinding(shape, FrameworkElement.WidthProperty, diameterBinding);
            }
            if (shape != null)
            {
                SetShapeProperties(shape, shapeState);
            }
            return shape;
        }
    }
}
