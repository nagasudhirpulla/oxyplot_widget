using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataFetchers.RandomDataFetcher
{
    public class RandomPlotFetchConfig
    {
        public List<Tuple<int, int, int>> Bounds { get; set; } = new List<Tuple<int, int, int>> { new Tuple<int, int, int>(0, 10, 30) };
    }
}
