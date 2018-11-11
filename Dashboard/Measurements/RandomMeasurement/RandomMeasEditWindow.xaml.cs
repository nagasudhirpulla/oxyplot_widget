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

namespace Dashboard.Measurements.RandomMeasurement
{
    /// <summary>
    /// Interaction logic for RandomMeasEditWindow.xaml
    /// </summary>
    public partial class RandomMeasEditWindow : Window
    {
        public MeasEditorVM editorVM;
        public RandomMeasEditWindow(RandomMeasurement measurement)
        {
            InitializeComponent();
            editorVM = new MeasEditorVM(measurement);
            DataContext = editorVM;
        }

        private void OkBtnClick(object sender, RoutedEventArgs e)
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

        private void CancelBtnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }

    public class MeasEditorVM
    {
        public MeasEditorVM(RandomMeasurement measurement)
        {
            mRandomMeasurement = new RandomMeasurement(measurement);
        }

        public RandomMeasurement mRandomMeasurement { get; set; }

        public string Low
        {
            get { return mRandomMeasurement.Low.ToString(); }
            set
            {
                // check if value is a number
                bool isNumeric = double.TryParse(value, out double input);
                if (isNumeric && input >= 0)
                {
                    mRandomMeasurement.Low = input;
                }
            }
        }

        public string High
        {
            get { return mRandomMeasurement.High.ToString(); }
            set
            {
                // check if value is a number
                bool isNumeric = double.TryParse(value, out double input);
                if (isNumeric && input >= 0)
                {
                    mRandomMeasurement.High = input;
                }
            }
        }

        public string NumPnts
        {
            get { return mRandomMeasurement.NumPnts.ToString(); }
            set
            {
                // check if value is a number
                bool isNumeric = int.TryParse(value, out int input);
                if (isNumeric && input >= 0)
                {
                    mRandomMeasurement.NumPnts = input;
                }
            }
        }
    }
}
