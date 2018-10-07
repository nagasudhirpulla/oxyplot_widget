using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.WidgetLayout
{
    public class WidgetDimension
    {
        int Height { get; set; }
        int Width { get; set; }
        int MinHeight { get; set; }
        int MinWidth { get; set; }
        DimensionEnum HeightMode { get; set; } = DimensionEnum.Variable;
        DimensionEnum WidthMode { get; set; } = DimensionEnum.Variable;
    }
}
