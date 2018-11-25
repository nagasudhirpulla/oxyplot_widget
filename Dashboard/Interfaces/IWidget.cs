using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Interfaces
{
    public interface IWidget
    {
        Action<EventArgs> Changed { get; set; }

        Task RefreshData();
        void OpenConfigWindow();

        // do additional freeing up of resources before deletion of this widget
        Task DoCleanUpForDeletion();

        // Required for JSON serialization
        IWidgetState GenerateState();
    }
}
