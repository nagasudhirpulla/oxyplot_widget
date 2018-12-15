using Dashboard.UserControls.VariableTimePicker;
using Dashboard.Widgets.Oxyplot;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Interfaces
{
    public interface IMeasurement
    {
        string TypeName { get; set; }
        Task<List<DataPoint>> FetchData(TimeShift timeShift);
        Task<List<DataPoint>> FetchData(VariableTime startTime, VariableTime endTime);
        string GetDisplayText();
        IMeasurement Clone();
    }
}
