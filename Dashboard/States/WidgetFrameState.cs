using Dashboard.Interfaces;
using Dashboard.WidgetLayout;
using Dashboard.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.States
{
    public class WidgetFrameState : IWidgetContainerState
    {
        public string TypeName { get; set; } = typeof(WidgetFrame).Name;
        public WidgetPosition Position { get; set; }
        public WidgetDimension Dimension { get; set; }
        public WidgetAppearance WidgetAppearance { get; set; }
        public IWidgetState WidgetState { get; set; }
    }
}
