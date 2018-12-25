using ShapeLayersWidget.Converters;
using ShapeLayersWidget.Interfaces;
using ShapeLayersWidget.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ShapeLayersWidget
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
            LayerCanvas.Children.Clear();

            LayerState = layerState;

            // Add all the layer state shapes to the canvas
            for (int shapeIter = 0; shapeIter < LayerState.ShapeStates.Count; shapeIter++)
            {
                IShapeState shapeState = LayerState.ShapeStates[shapeIter];
                AddShape(shapeState);
            }

        }

        public void AddShape(IShapeState shapeState)
        {
            Shape shape = shapeState.CreateShape(shapeState);
            if (shape!=null)
            {
                LayerCanvas.Children.Add(shape);
            }
        }
        
    }
}
