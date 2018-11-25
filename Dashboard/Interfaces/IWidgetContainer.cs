using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.WidgetLayout;

namespace Dashboard.Interfaces
{
    public interface IWidgetContainer
    {
        Task RefreshData();
        void OpenConfigWindow();
        void DoWidgetCleanUp();

        WidgetPosition Position { get; set; }
        WidgetDimension Dimension { get; set; }
        WidgetAppearance WidgetAppearance { get; set; }

        // Send Messages to Dashboard using this event handler
        event EventHandler<EventArgs> Changed;

        // Required for JSON serialization
        IWidgetContainerState GenerateState();

        // Required for JSON deserialization
        void SetState(IWidgetContainerState state);
    }
}
