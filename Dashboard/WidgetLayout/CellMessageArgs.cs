using System;

namespace Dashboard.WidgetLayout
{
    public class CellMessageArgs : EventArgs
    {
        public static string ConfigWindowOpenRequest = "ConfigWindowOpenRequest";
        public string Message { get; set; }
        public CellMessageArgs()
        {
        }
    }
}
