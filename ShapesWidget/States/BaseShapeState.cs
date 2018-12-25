using ShapeLayersWidget.Converters;
using ShapeLayersWidget.Interfaces;
using System;
using System.Collections.Generic;
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
    public class BaseShapeState
    {
        public void SetShapeProperties(Shape shape, IShapeState shapeState)
        {
            BindingOperations.SetBinding(shape, Canvas.LeftProperty, new Binding { Source = shapeState.Left.X });
            BindingOperations.SetBinding(shape, Canvas.TopProperty, new Binding { Source = shapeState.Left.Y });
            BindingOperations.SetBinding(shape, Shape.FillProperty, new Binding { Source = shapeState.FillColor, Converter = new ColorToBrushConverter() });
            BindingOperations.SetBinding(shape, Shape.StrokeProperty, new Binding { Source = shapeState.StrokeColor, Converter = new ColorToBrushConverter() });

            void MouseChangeAction(object sender, MouseEventArgs e)
            {
                if (sender is Shape shapeObj)
                {
                    if (shapeObj.IsMouseOver)
                    {
                        BindingOperations.SetBinding(shape, Shape.FillProperty, new Binding { Source = shapeState.HoverFillColor, Converter = new ColorToBrushConverter() });
                        BindingOperations.SetBinding(shape, Shape.StrokeProperty, new Binding { Source = shapeState.HoverStrokeColor, Converter = new ColorToBrushConverter() });
                    }
                    else
                    {
                        BindingOperations.SetBinding(shape, Shape.FillProperty, new Binding { Source = shapeState.FillColor, Converter = new ColorToBrushConverter() });
                        BindingOperations.SetBinding(shape, Shape.StrokeProperty, new Binding { Source = shapeState.StrokeColor, Converter = new ColorToBrushConverter() });
                    }
                }
            }
            shape.MouseEnter += MouseChangeAction;
            shape.MouseLeave += MouseChangeAction;
        }
    }
}
