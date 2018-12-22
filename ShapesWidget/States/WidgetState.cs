using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShapeLayersWidget.States
{
    public class WidgetState
    {
        public string Name { get; set; } = "Shape Widget";
        public Color BackgroundColor { get; set; } = Color.FromRgb(0, 0, 0);
        private double ZoomX_ = 1;
        private double ZoomY_ = 1;
        public double OffsetX { get; set; } = 0;
        public double OffsetY { get; set; } = 0;

        public double ZoomX
        {
            get { return ZoomX_; }
            set
            {
                if (value > 0) { ZoomX_ = value; }
            }
        }

        public double ZoomY
        {
            get { return ZoomY_; }
            set
            {
                if (value > 0) { ZoomY_ = value; }
            }
        }

        public List<LayerState> LayerStates = new List<LayerState>();

    }
}
