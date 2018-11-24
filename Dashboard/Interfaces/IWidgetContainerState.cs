using Dashboard.WidgetLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Interfaces
{
    public interface IWidgetContainerState
    {
        string TypeName { get; set; }
        WidgetPosition Position { get; set; }
        WidgetDimension Dimension { get; set; }
        WidgetAppearance WidgetAppearance { get; set; }
    }
}
