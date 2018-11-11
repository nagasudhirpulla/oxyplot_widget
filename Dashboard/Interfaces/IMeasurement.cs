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
        Task<List<DataPoint>> FetchData();
        string GetDisplayText();
        IMeasurement Clone();
    }
}
