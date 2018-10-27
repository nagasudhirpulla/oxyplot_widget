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
        public void AddDashboardWidgetToContainer(Grid container, IWidget widget, Action<object, EventArgs> changed)
        {
            EnsureWidgetPositon(container, widget.Position);
            container.Children.Add(widget as UserControl);
            widget.Changed += new EventHandler<EventArgs>(changed);
        }

        public void ChangeWidgetPosition(Grid container, IWidget widget, WidgetPosition newPosition)
        {
            EnsureWidgetPositon(container, newPosition);
            widget.Position = newPosition;
            RemoveEmptyRowsAndColumns(container);
        }

        private WidgetPosition FindMaxContainerRowColumnDefinitions(Grid container)
        {
            // find max row and column definitions in the container
            return new WidgetPosition { Row = container.RowDefinitions.Count - 1, Column = container.ColumnDefinitions.Count - 1 };
        }

        private WidgetPosition FindMaxContainerChildPoisition(Grid container)
        {
            // find the maximum child position in a container
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
            WidgetPosition newPosition = FindMaxContainerChildPoisition(container);
            newPosition.Row += 1;
            if (newPosition.Column == -1)
            {
                newPosition.Column = 0;
            }
            return newPosition;
        }

        private void EnsureWidgetPositon(Grid container, WidgetPosition position)
        {
            WidgetPosition maxPosition = FindMaxContainerRowColumnDefinitions(container);
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

        public void RemoveEmptyRowsAndColumns(Grid container)
        {
            // Deleting the rows and column at the end that are empty
            WidgetPosition maxChildPosition = FindMaxContainerChildPoisition(container);
            WidgetPosition maxChildDefinitions = FindMaxContainerRowColumnDefinitions(container);
            // finding the row surplus
            int rowSurplus = maxChildDefinitions.Row - maxChildPosition.Row;
            for (int iter = 0; iter < rowSurplus; iter++)
            {
                container.RowDefinitions.RemoveAt(0);
            }
            // finding the column surplus
            int columnSurplus = maxChildDefinitions.Column - maxChildPosition.Column;
            for (int iter = 0; iter < columnSurplus; iter++)
            {
                container.ColumnDefinitions.RemoveAt(0);
            }
            //todo add feature to remove empty rows and columns in between also
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
