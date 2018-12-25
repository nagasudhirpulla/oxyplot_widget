using ShapeLayersWidget.Converters;
using ShapeLayersWidget.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeLayersWidget.States
{
    public class PolygonState : BaseShapeState, IShapeState
    {
        public string TypeName { get; set; } = typeof(PolygonState).Name;

        public string Name { get; set; } = "Polygon";

        public List<PointState> Points { get; set; } = new List<PointState>();

        public PointState Left { get; set; } = new PointState { X = 0, Y = 0 };

        public Color FillColor { get; set; } = Color.FromRgb(255, 255, 255);

        public Color HoverFillColor { get; set; } = Color.FromRgb(255, 0, 0);

        public Color HoverStrokeColor { get; set; } = Color.FromRgb(255, 255,255);

        public Color StrokeColor { get; set; }

        public Shape CreateShape(IShapeState shapeState)
        {
            Shape shape = null;
            if (shapeState is PolygonState polygonState)
            {
                shape = new Polygon();
                BindingOperations.SetBinding(shape, Polygon.PointsProperty, new Binding("Points") { Source = polygonState, Converter = new PolyPointsConverter() });
            }
            if (shape != null)
            {
                SetShapeProperties(shape, shapeState);
            }
            return shape;
        }
    }
}
