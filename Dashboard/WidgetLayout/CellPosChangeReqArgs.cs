using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.WidgetLayout
{
    public class CellPosChangeReqArgs : EventArgs
    {
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
        POS_EDIT_WIN = 0, POS_UP, POS_DOWN, POS_LEFT, POS_RIGHT, POS_DELETE
    }
}
