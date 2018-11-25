using Dashboard.Interfaces;
using Dashboard.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.States
{
    public class DashboardState
    {
        // Dimensions
        public int InitHeight { get; set; } = 800;
        public int InitWidth { get; set; } = 800;
        public bool IsDimensionsLocked { get; set; } = false;
        [JsonConverter(typeof(WidgetContainerStateConverter))]
        public List<IWidgetContainerState> WidgetContainerStates { get; set; } = new List<IWidgetContainerState>();
    }
}
