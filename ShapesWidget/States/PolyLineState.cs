using ShapeLayersWidget.Converters;
using ShapeLayersWidget.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeLayersWidget.States
{
    public class PolyLineState : BaseShapeState, IShapeState
    {
        public string TypeName { get; set; } = typeof(PolyLineState).Name;

        public string Name { get; set; } = "PolyLine";

        public List<PointState> Points { get; set; } = new List<PointState>();

        public PointState Left { get; set; } = new PointState { X = 0, Y = 0 };

        public Color FillColor { get; set; }

        public Color HoverFillColor { get; set; }

        public Color StrokeColor { get; set; } = Color.FromRgb(0, 255, 255);

        public Color HoverStrokeColor { get; set; } = Color.FromRgb(255, 255, 255);

        public Shape CreateShape(IShapeState shapeState)
        {
            Shape shape = null;
            if (shapeState is PolyLineState polyLineState)
            {
                shape = new Polyline();
                BindingOperations.SetBinding(shape, Polyline.PointsProperty, new Binding { Source = polyLineState.Points, Converter = new PolyPointsConverter() });
            }
            if (shape != null)
            {
                SetShapeProperties(shape, shapeState);
            }
            return shape;
        }
    }
}
