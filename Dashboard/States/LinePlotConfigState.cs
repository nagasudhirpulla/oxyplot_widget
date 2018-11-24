using Dashboard.Interfaces;
using Dashboard.Widgets.Oxyplot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.States
{
    public class LinePlotConfigState : IPlotConfigState
    {
        public string TypeName { get; set; } = typeof(LinePlotConfig).Name;
        public LinePlotConfig LinePlotConfig { get; set; }
    }
}
