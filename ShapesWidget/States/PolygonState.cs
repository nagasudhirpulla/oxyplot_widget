using ShapeLayersWidget.Converters;
using ShapeLayersWidget.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ShapeLayersWidget.States
{
    public class PolygonState : IShapeState
    {
        public string TypeName { get; set; } = typeof(PolygonState).Name;

        public string Name { get; set; } = "Polygon";

        public List<PointState> Points { get; set; } = new List<PointState>();

        public PointState Left { get; set; } = new PointState { X = 0, Y = 0 };

        public Shape CreateShape(IShapeState shapeState)
        {
            Shape shape = null;
            if (shapeState is PolygonState polygonState)
            {
                shape = new Polygon();
                BindingOperations.SetBinding(shape, Polygon.PointsProperty, new Binding { Source = polygonState.Points, Converter = new PolyPointsConverter() });
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
