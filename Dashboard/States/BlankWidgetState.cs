using Dashboard.Interfaces;
using Dashboard.Widgets;

namespace Dashboard.States
{
    public class BlankWidgetState:IWidgetState
    {
        public string TypeName { get; set; } = typeof(BlankWidgetState).Name;
    }
}