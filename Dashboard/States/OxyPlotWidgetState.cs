using Dashboard.Interfaces;
using Dashboard.Widgets.Oxyplot;
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
        public IPlotConfigState PlotConfigState { get; set; }
    }
}
