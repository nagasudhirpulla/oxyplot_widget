using Dashboard.WidgetLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dashboard.EditorWindows
{
    /// <summary>
    /// Interaction logic for WidgetPositionEditorWindow.xaml
    /// </summary>
    public partial class WidgetPositionEditorWindow : Window
    {
        public WidgetPositionEditorWindow(WidgetPosition position, WidgetDimension dimension)
        {
            InitializeComponent();
            mWidgetPositionVM = new WidgetPositionEditorVM(position, dimension);
            DataContext = mWidgetPositionVM;
        }

        private WidgetPositionEditorVM mWidgetPositionVM;

        public WidgetPosition WidgetPosition { get { return mWidgetPositionVM.WidgetPosition; } }
        public WidgetDimension WidgetDimension { get { return mWidgetPositionVM.WidgetDimension; } }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Apply Changes ?", "Apply Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            else
            {
                DialogResult = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

    public class WidgetPositionEditorVM
    {
        public WidgetPositionEditorVM()
        {
            mWidgetPosition = new WidgetPosition();
            mWidgetDimension = new WidgetDimension();
        }

        public WidgetPositionEditorVM(WidgetPosition position, WidgetDimension dimension)
        {
            mWidgetPosition = new WidgetPosition(position);
            mWidgetDimension = new WidgetDimension(dimension);
        }

        private WidgetPosition mWidgetPosition;
        private WidgetDimension mWidgetDimension;

        public WidgetPosition WidgetPosition { get { return mWidgetPosition; } }
        public WidgetDimension WidgetDimension { get { return mWidgetDimension; } }

        public int Row { get { return mWidgetPosition.Row; } set { mWidgetPosition.Row = value; } }
        public int RowSpan { get { return mWidgetPosition.RowSpan; } set { mWidgetPosition.RowSpan = value; } }
        public int Column { get { return mWidgetPosition.Column; } set { mWidgetPosition.Column = value; } }
        public int ColSpan { get { return mWidgetPosition.ColSpan; } set { mWidgetPosition.ColSpan = value; } }
        public int MinWidth { get { return mWidgetDimension.MinWidth; } set { mWidgetDimension.MinWidth = value; } }
        public int MinHeight { get { return mWidgetDimension.MinHeight; } set { mWidgetDimension.MinHeight = value; } }
    }
}
