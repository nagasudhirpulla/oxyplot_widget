using ShapeLayersWidget.Converters;
using ShapeLayersWidget.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeLayersWidget.States
{
    public class BaseShapeState : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void SetShapeProperties(Shape shape, IShapeState shapeState)
        {
            BindingOperations.SetBinding(shape, Canvas.LeftProperty, new Binding("Left.X") { Source = shapeState });
            BindingOperations.SetBinding(shape, Canvas.TopProperty, new Binding("Left.Y") { Source = shapeState });
            BindingOperations.SetBinding(shape, Shape.FillProperty, new Binding("FillColor") { Source = shapeState, Converter = new ColorToBrushConverter() });
            BindingOperations.SetBinding(shape, Shape.StrokeProperty, new Binding("StrokeColor") { Source = shapeState, Converter = new ColorToBrushConverter() });

            void MouseChangeAction(object sender, MouseEventArgs e)
            {
                if (sender is Shape shapeObj)
                {
                    if (shapeObj.IsMouseOver)
                    {
                        BindingOperations.SetBinding(shape, Shape.FillProperty, new Binding("HoverFillColor") { Source = shapeState, Converter = new ColorToBrushConverter() });
                        BindingOperations.SetBinding(shape, Shape.StrokeProperty, new Binding("HoverStrokeColor") { Source = shapeState, Converter = new ColorToBrushConverter() });
                    }
                    else
                    {
                        BindingOperations.SetBinding(shape, Shape.FillProperty, new Binding("FillColor") { Source = shapeState, Converter = new ColorToBrushConverter() });
                        BindingOperations.SetBinding(shape, Shape.StrokeProperty, new Binding("StrokeColor") { Source = shapeState, Converter = new ColorToBrushConverter() });
                    }
                }
            }
            shape.MouseEnter += MouseChangeAction;
            shape.MouseLeave += MouseChangeAction;
            shape.DataContext = shapeState;
        }
    }
}
