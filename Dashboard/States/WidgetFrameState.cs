using Dashboard.Interfaces;
using Dashboard.JsonConverters;
using Dashboard.WidgetLayout;
using Dashboard.Widgets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.States
{
    public class WidgetFrameState : IWidgetContainerState
    {
        public string TypeName { get; set; } = typeof(WidgetFrameState).Name;
        public WidgetPosition Position { get; set; } = new WidgetPosition();
        public WidgetDimension Dimension { get; set; } = new WidgetDimension();
        public WidgetAppearance WidgetAppearance { get; set; } = new WidgetAppearance();
        public WidgetContainerAutoFetchState WidgetContainerAutoFetchState_ { get; set; } = new WidgetContainerAutoFetchState();
        [JsonConverter(typeof(WidgetStateConverter))]
        public IWidgetState WidgetState { get; set; }
    }
}
