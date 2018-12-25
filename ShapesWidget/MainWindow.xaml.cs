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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShapeLayersWidget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ShapesWidget shapesWidget;
        public MainWindow()
        {
            InitializeComponent();
            AddMapHolder();
        }

        private void AddMapHolder()
        {
            WidgetState widgetState = CreateTestWidgetState();
            WidgetViewModel model = new WidgetViewModel(widgetState);
            shapesWidget = new ShapesWidget(model);
            MapContainer.Children.Add(shapesWidget);
            
            //shapesWidget.RemoveShape(0,0);
        }

        private WidgetState CreateTestWidgetState()
        {
            WidgetState widgetState = new WidgetState();
            LayerState layerState = new LayerState();
            layerState.ShapeStates.Add(new CircleState { Radius = 100, Center = new PointState { X = 300, Y = 200 } });
            layerState.ShapeStates.Add(new CircleState { Radius = 25, Center = new PointState { X = 100, Y = 100 } });
            layerState.ShapeStates.Add(new RectangleState { Left = new PointState { X = 100, Y = 100 }, FillColor = Color.FromRgb(241, 45, 84), LengthX = 25, LengthY = 254 });
            layerState.ShapeStates.Add(new RectangleState { Left = new PointState { X = 15, Y = 10 }, FillColor = Color.FromRgb(45, 84, 241), LengthX = 254, LengthY = 54 });
            layerState.ShapeStates.Add(new PolygonState
            {
                Left = new PointState { X = 0, Y = 0 },
                FillColor = Color.FromRgb(45, 84, 241),
                Points = new List<PointState> {
                new PointState { X = 0, Y = 0 }, new PointState { X = 500, Y = 0 },
                new PointState { X = 15, Y = 300 }, new PointState { X = 500, Y = 100 } }
            });
            layerState.ShapeStates.Add(new PolyLineState
            {
                Left = new PointState { X = 100, Y = 100 },
                Points = new List<PointState> {
                new PointState { X = 0, Y = 0 }, new PointState { X = 100, Y = 0 }, new PointState { X = 100, Y = 100 } }
            });
            widgetState.LayerStates.Add(layerState);

            return widgetState;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IShapeState shapeState = shapesWidget.GetShape(0, 0);
            shapeState.Left = new PointState { X = 700, Y = 0 };
            shapeState.FillColor = Color.FromRgb(0,255,255);
            ((CircleState)shapeState).Diameter = 40;

            shapesWidget.InvalidateShape(0, 0);
        }
    }
}
