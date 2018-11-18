using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.WidgetLayout
{
    public class WidgetPosition
    {
        public WidgetPosition() { }

        public WidgetPosition(WidgetPosition position)
        {
            Row = position.Row;
            RowSpan = position.RowSpan;
            Column = position.Column;
            ColSpan = position.ColSpan;
        }

        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; } = 1;
        public int ColSpan { get; set; } = 1;

        public WidgetPosition GetLegitPosition()
        {
            WidgetPosition legitPos = new WidgetPosition(this);
            if (Row < 0) { legitPos.Row = 0; }
            if (Column < 0) { legitPos.Column = 0; }
            if (RowSpan < 1) { legitPos.RowSpan = 1; }
            if (ColSpan < 1) { legitPos.ColSpan = 1; }
            return legitPos;
        }
    }
}
