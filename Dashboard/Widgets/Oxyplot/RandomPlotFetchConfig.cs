using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Widgets.Oxyplot
{
    public class RandomPlotFetchConfig
    {
        public List<Tuple<int, int>> Bounds { get; set; } = new List<Tuple<int, int>> { new Tuple<int, int>(0, 10) };
        public int NumPointsInEachSeries { get; set; } = 15;
    }
}
