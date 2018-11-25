using Dashboard.Interfaces;
using Dashboard.JsonConverters;
using Dashboard.Widgets.Oxyplot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.States
{
    public class OxyPlotWidgetState : IWidgetState
    {
        public string TypeName { get; set; } = typeof(PlotWidget).Name;

        [JsonConverter(typeof(PlotConfigConverter))]
        public IPlotConfig PlotConfig { get; set; }
    }
}
