using Dashboard.Interfaces;
using Dashboard.JsonConverters;
using Dashboard.UserControls.Dashboard;
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
        public string Name = "Dashboard";
        // Dimensions
        public int InitHeight { get; set; } = 600;
        public int InitWidth { get; set; } = 1000;
        public bool IsDimensionsLocked { get; set; } = false;
        [JsonConverter(typeof(WidgetContainerStateConverter))]
        public List<IWidgetContainerState> WidgetContainerStates { get; set; } = new List<IWidgetContainerState>();
        public DashboardAutoFetchState AutoFetchState { get; set; } = new DashboardAutoFetchState();

        public DashboardState GenerateSettings()
        {
            DashboardState state = new DashboardState
            {
                Name = Name,
                InitHeight = InitHeight,
                InitWidth = InitWidth,
                IsDimensionsLocked = IsDimensionsLocked
            };

            return state;
        }

        public void SetSettings(DashboardState state)
        {
            Name = state.Name;
            InitWidth = state.InitWidth;
            InitHeight = state.InitHeight;
            IsDimensionsLocked = state.IsDimensionsLocked;
        }
    }
}
