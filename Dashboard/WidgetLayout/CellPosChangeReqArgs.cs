using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.WidgetLayout
{
    public class CellPosChangeReqArgs : EventArgs
    {
        public const string POS_EDIT_WIN = "POS_EDIT_WIN";
        public const string POS_UP = "POS_UP";
        public const string POS_DOWN = "POS_DOWN";
        public const string POS_LEFT = "POS_LEFT";
        public const string POS_RIGHT = "POS_RIGHT";

        public CellPosChangeMsgType MessageType { get; set; } = CellPosChangeMsgType.POS_EDIT_WIN;

        public CellPosChangeReqArgs()
        {
        }

        public CellPosChangeReqArgs(CellPosChangeMsgType msgType)
        {            
            MessageType = msgType;
        }
    }

    public enum CellPosChangeMsgType
    {
        POS_EDIT_WIN = 0, POS_UP, POS_DOWN, POS_LEFT, POS_RIGHT
    }
}
