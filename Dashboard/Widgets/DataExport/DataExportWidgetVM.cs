using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Dashboard.Widgets.DataExport
{
    public class DataExportWidgetVM : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        private DataTable mDataDisplayTable;
        public DataTable DataDisplayTable { get { return mDataDisplayTable; } set { mDataDisplayTable = value; OnPropertyChanged("DataDisplayTable"); } }
        private string xAxisColName = "xAxis";
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Send Messages to Parent using this event handler
        public event EventHandler<EventArgs> Changed;
        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public DataExportWidgetVM()
        {
            // do nothing
        }

        public DataTable GetPlotDataTable()
        {
            return DataDisplayTable;
        }

        public void ReplaceSeriesPoints(int seriesIndex, List<DataPoint> points)
        {
            if (seriesIndex < DataDisplayTable.Columns.Count - 1 && seriesIndex >= 0)
            {
                for (int pntIter = 0; pntIter < points.Count; pntIter++)
                {
                    string colName = DataDisplayTable.Columns[seriesIndex + 1].ColumnName;
                    // search for the row with xAxis value as the point x value
                    // DataRow row = DataDisplayTable.Select($"xAxis={points[pntIter].X}").FirstOrDefault();
                    if (DataDisplayTable.Rows.Contains(points[pntIter].X))
                    {
                        DataRow row = DataDisplayTable.Rows.Find(points[pntIter].X);
                        row[colName] = points[pntIter].Y;
                    }
                    else
                    {
                        DataRow row = DataDisplayTable.NewRow();
                        row[xAxisColName] = points[pntIter].X;
                        row[colName] = points[pntIter].Y;
                        DataDisplayTable.Rows.Add(row);
                    }
                }
                //OnPropertyChanged("DataDisplayTable");
            }
        }

        public void ClearSeries()
        {
            DataDisplayTable = new DataTable();
            AddNewSeries(xAxisColName, null);
            //Set the Primary Key Column.
            DataDisplayTable.PrimaryKey = new DataColumn[] { DataDisplayTable.Columns[xAxisColName] };
        }

        public void AddNewSeries(string colName, Type type)
        {
            Type colType = type;
            if (type == null)
            {
                colType = typeof(double);
            }
            // We have to add a new column to the datatable by this name
            List<string> colNames = new List<string>();
            foreach (DataColumn column in DataDisplayTable.Columns)
            {
                colNames.Add(column.ColumnName);
            }
            for (int i = 1; colNames.IndexOf(colName) != -1; i++)
            {
                colName = $"{colName}_{i}";
            }
            // Add new column to datatable - https://stackoverflow.com/questions/2312966/add-new-column-and-data-to-datatable-that-already-contains-data-c-sharp
            DataDisplayTable.Columns.Add(colName, colType);
            //OnPropertyChanged("DataDisplayTable");
        }
    }
}
