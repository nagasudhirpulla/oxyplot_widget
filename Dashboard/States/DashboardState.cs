using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.States
{
    public class DashboardState
    {
        // Dimensions
        public int InitHeight { get; set; }
        public int InitWidth { get; set; }
        public bool IsDimensionsLocked { get; set; } = false;
    }
}
