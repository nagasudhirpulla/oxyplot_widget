using ShapeLayersWidget.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeLayersWidget.Interfaces
{
    public interface IShapeState
    {
        string TypeName { get; set; }
        string Name { get; set; }
        PointState Left { get; set; }
        Color FillColor { get; set; }
        Color HoverFillColor { get; set; }
        Color StrokeColor { get; set; }
        Color HoverStrokeColor { get; set; }
        Shape CreateShape(IShapeState shapeState);
    }
}
