using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.WidgetLayout
{
    public class WidgetDimension
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int MinHeight { get; set; } = 100;
        public int MinWidth { get; set; } = 100;
        public DimensionEnum HeightMode { get; set; } = DimensionEnum.Variable;
        public DimensionEnum WidthMode { get; set; } = DimensionEnum.Variable;

        public WidgetDimension(WidgetDimension dimension)
        {
            Height = dimension.Height;
            Width = dimension.Width;
            MinHeight = dimension.MinHeight;
            MinWidth = dimension.MinWidth;
            HeightMode = dimension.HeightMode;
            WidthMode = dimension.WidthMode;
        }

        public WidgetDimension() { }
    }
}
