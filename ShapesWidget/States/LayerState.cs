using ShapesWidget.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesWidget.States
{
    public class LayerState
    {
        public string Name { get; set; } = "Layer Name";

        public bool IsVisible { get; set; } = true;

        public List<IShapeState> ShapeStates { get; set; } = new List<IShapeState>();
    }
}
