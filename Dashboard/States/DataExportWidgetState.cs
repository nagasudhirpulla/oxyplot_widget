using Dashboard.Interfaces;
using Dashboard.JsonConverters;
using Dashboard.Widgets.DataExport;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.States
{
    public class DataExportWidgetState : IWidgetState
    {
        public string TypeName { get; set; } = typeof(DataExportWidgetState).Name;

        public DataExportConfig DataExportConfig_ { get; set; }
    }
}
