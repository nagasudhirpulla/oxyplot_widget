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
        public WidgetPositionEditorWindow(WidgetPosition position)
        {
            InitializeComponent();
            mWidgetPositionVM = new WidgetPositionEditorVM(position);
            DataContext = mWidgetPositionVM;
        }

        private WidgetPositionEditorVM mWidgetPositionVM;

        public WidgetPosition WidgetPosition { get { return mWidgetPositionVM.WidgetPosition; } }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Save Changes ?", "Save Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
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
            this.Close();
        }
    }

    public class WidgetPositionEditorVM
    {
        public WidgetPositionEditorVM()
        {
            mWidgetPosition = new WidgetPosition();
        }

        public WidgetPositionEditorVM(WidgetPosition position)
        {
            mWidgetPosition = new WidgetPosition(position);
        }

        private WidgetPosition mWidgetPosition;

        public WidgetPosition WidgetPosition { get { return mWidgetPosition; } }

        public string RowString
        {
            get { return mWidgetPosition.Row.ToString(); }
            set
            {
                // check if value is a number
                bool isNumeric = int.TryParse(value, out int integerInput);
                if (isNumeric && integerInput >= 0)
                {
                    mWidgetPosition.Row = integerInput;
                }
            }
        }

        public string RowSpanString
        {
            get { return mWidgetPosition.RowSpan.ToString(); }
            set
            {
                // check if value is a number
                bool isNumeric = int.TryParse(value, out int integerInput);
                if (isNumeric && integerInput >= 0)
                {
                    mWidgetPosition.RowSpan = integerInput;
                }
            }
        }

        public string ColumnString
        {
            get { return mWidgetPosition.Column.ToString(); }
            set
            {
                // check if value is a number
                bool isNumeric = int.TryParse(value, out int integerInput);
                if (isNumeric && integerInput >= 0)
                {
                    mWidgetPosition.Column = integerInput;
                }
            }
        }

        public string ColSpanString
        {
            get { return mWidgetPosition.ColSpan.ToString(); }
            set
            {
                // check if value is a number
                bool isNumeric = int.TryParse(value, out int integerInput);
                if (isNumeric && integerInput >= 0)
                {
                    mWidgetPosition.ColSpan = integerInput;
                }
            }
        }
    }
}
