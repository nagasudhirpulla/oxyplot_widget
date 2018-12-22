using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesWidget.Interfaces
{
    public interface IShapeState
    {
        string TypeName { get; set; }
        string Name { get; set; }
    }
}
