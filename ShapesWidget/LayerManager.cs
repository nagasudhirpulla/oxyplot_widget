using ShapesWidget.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ShapesWidget
{
    public class LayerManager
    {
        private Canvas LayerCanvas;
        public LayerState LayerState { get; set; } = new LayerState();
        public List<Shape> Shapes { get; set; } = new List<Shape>();

        public LayerManager(Canvas canvas)
        {
            SetCanvas(canvas);
        }

        public LayerManager(Canvas canvas, LayerState layerState)
        {
            SetCanvas(canvas);
            SetState(layerState);
        }

        public void SetCanvas(Canvas canvas)
        {
            LayerCanvas = canvas;
        }

        public void SetState(LayerState layerState)
        {
            // Clear all the current shapes
            Shapes.Clear();

            LayerState = layerState;

            // Add all the layer state shapes to the canvas and bind the shape states to layerstate
            for (int shapeIter = 0; shapeIter < LayerState.ShapeStates.Count; shapeIter++)
            {


            }
        }

    }
}
