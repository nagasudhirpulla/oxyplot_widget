using ShapesWidget.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesWidget.States
{
    public class CircleState: IShapeState
    {
        public string TypeName { get; set; } = typeof(CircleState).Name;

        public string Name { get; set; } = "Circle";

        public PointState Center { get; set; } = new PointState();

        public double Radius { get; set; } = 1;

    }
}
