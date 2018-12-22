using ShapesWidget.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesWidget.States
{
    class RectangleState : IShapeState
    {
        public string TypeName { get; set; } = typeof(RectangleState).Name;

        public string Name { get; set; } = "Rectangle";

        public PointState Left { get; set; } = new PointState();

        public double LengthX { get; set; } = 1;

        public double LengthY { get; set; } = 1;
    }
}
