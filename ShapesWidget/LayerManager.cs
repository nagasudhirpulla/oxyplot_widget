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
            InflateState(layerState);
        }

        public void SetCanvas(Canvas canvas)
        {
            LayerCanvas = canvas;
        }

        public void InflateState(LayerState layerState)
        {
            // Clear all the current shapes
            LayerCanvas.Children.Clear();

            LayerState = layerState;

            // Add all the layer state shapes to the canvas
            for (int shapeIter = 0; shapeIter < LayerState.ShapeStates.Count; shapeIter++)
            {
                InflateShapeState(LayerState.ShapeStates[shapeIter]);
            }

        }

        public void InflateShapeState(IShapeState shapeState)
        {
            Shape shape = shapeState.CreateShape(shapeState);
            if (shape != null)
            {
                LayerCanvas.Children.Add(shape);
            }
        }

        public IShapeState GetShape(int shapeIndex)
        {
            if (shapeIndex >= 0 && shapeIndex < LayerState.ShapeStates.Count)
            {
                // todo remove the shape also
                return LayerState.ShapeStates[shapeIndex];
            }
            return null;
        }

        public void AddShape(IShapeState shapeState)
        {
            LayerState.ShapeStates.Add(shapeState);
            InflateShapeState(shapeState);
        }

        public void InvalidateShape(int shapeIndex)
        {
            if (shapeIndex >= 0 && shapeIndex < LayerState.ShapeStates.Count)
            {
                ((BaseShapeState)LayerState.ShapeStates[shapeIndex]).OnPropertyChanged(String.Empty);
            }
        }

        public void RemoveShape(IShapeState shapeState)
        {
            int shapeIndex = LayerState.ShapeStates.IndexOf(shapeState);
            RemoveShape(shapeIndex);
        }

        public void RemoveShape(int shapeIndex)
        {
            if (shapeIndex >= 0 && shapeIndex < LayerState.ShapeStates.Count)
            {
                // todo remove the shape also
                LayerState.ShapeStates[shapeIndex] = null;
                LayerState.ShapeStates.RemoveAt(shapeIndex);
            }
        }

    }
}
