using Dashboard.Interfaces;
using Dashboard.WidgetLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dashboard.UserControls.Dashboard
{
    public class DashboardLayoutManager
    {
        public void AddDashboardWidgetToContainer(Grid container, IWidget widget)
        {
            EnsureWidgetPositon(container, widget.Position);
            container.Children.Add(widget as UserControl);
        }

        private WidgetPosition FindMaxContainerPoisition(Grid container)
        {
            // find max rows and columns in the container
            int maxRows = 0;
            int maxCols = 0;
            for (int iter = 0; iter < container.Children.Count; iter++)
            {
                if (container.Children[iter] is IWidget)
                {
                    IWidget widget = container.Children[iter] as IWidget;
                    int rowCountReq = widget.Position.Row + widget.Position.RowSpan;
                    int colCountReq = widget.Position.Column + widget.Position.ColSpan;
                    if (rowCountReq > maxRows)
                    {
                        maxRows = rowCountReq;
                    }
                    if (colCountReq > maxCols)
                    {
                        maxCols = colCountReq;
                    }
                }
            }

            return new WidgetPosition { Row = maxRows - 1, Column = maxCols - 1 };
        }

        public WidgetPosition GetNewWidgetPositon(Grid container)
        {
            WidgetPosition newPosition = FindMaxContainerPoisition(container);
            newPosition.Row += 1;
            if (newPosition.Column == -1)
            {
                newPosition.Column = 0;
            }
            return newPosition;
        }

        private void EnsureWidgetPositon(Grid container, WidgetPosition position)
        {
            WidgetPosition maxPosition = FindMaxContainerPoisition(container);
            // find max rows and columns in the container
            int maxRows = maxPosition.Row + 1;
            int maxCols = maxPosition.Column + 1;

            for (int iter = 0; iter < container.Children.Count; iter++)
            {
                if (container.Children[iter] is IWidget)
                {
                    IWidget widget = container.Children[iter] as IWidget;
                    int rowCountReq = widget.Position.Row + widget.Position.RowSpan;
                    int colCountReq = widget.Position.Column + widget.Position.ColSpan;
                    if (rowCountReq > maxRows)
                    {
                        maxRows = rowCountReq;
                    }
                    if (colCountReq > maxCols)
                    {
                        maxCols = colCountReq;
                    }
                }
            }

            // Add adequate number of row and column definitions
            // Find rows deficit
            int rowDeficit = position.Row + position.RowSpan - maxRows;
            int colDeficit = position.Column + position.ColSpan - maxCols;

            if (rowDeficit > 0)
            {
                // add deficit rows
                for (int i = 0; i < rowDeficit; i++)
                {
                    container.RowDefinitions.Add(GetNewRowDefinition());
                }

            }

            if (colDeficit > 0)
            {
                // add deficit columns
                for (int i = 0; i < colDeficit; i++)
                {
                    container.ColumnDefinitions.Add(GetNewColDefinition());
                }

            }
        }

        private RowDefinition GetNewRowDefinition()
        {
            RowDefinition rowDef = new RowDefinition();
            // Add default Row Definition settings here if desired
            return rowDef;
        }

        private ColumnDefinition GetNewColDefinition()
        {
            ColumnDefinition colDef = new ColumnDefinition();
            // Add default Row Definition settings here if desired
            return colDef;
        }
    }
}
