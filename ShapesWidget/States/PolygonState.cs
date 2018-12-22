using ShapesWidget.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesWidget.States
{
    public class PolygonState: IShapeState
    {
        public string TypeName { get; set; } = typeof(PolygonState).Name;

        public string Name { get; set; } = "Polygon";

        public List<PointState> Points { get; set; } = new List<PointState>();

    }
}
