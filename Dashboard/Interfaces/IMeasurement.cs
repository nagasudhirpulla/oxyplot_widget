﻿using Dashboard.UserControls.VariableTimePicker;
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
        Task<List<DataPoint>> FetchDataAsync(TimeShift timeShift);
        Task<List<DataPoint>> FetchData(DateTime startTime, DateTime endTime);
        string GetDisplayText();
        IMeasurement Clone();
    }
}
