using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dashboard.Helpers
{
    public class OxyUtility
    {
        public static OxyColor ConvertColorToOxyColor(Color color)
        {
            OxyColor oxyColor = OxyColor.FromArgb(color.A, color.R, color.G, color.B);
            return oxyColor;
        }
    }
}
