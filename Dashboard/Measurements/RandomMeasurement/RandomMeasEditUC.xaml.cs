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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dashboard.Measurements.RandomMeasurement
{
    /// <summary>
    /// Interaction logic for RandomMeasEditUC.xaml
    /// </summary>
    public partial class RandomMeasEditUC : UserControl
    {
        public MeasEditUCVM editorVM;
        public RandomMeasEditUC(RandomMeasurement measurement)
        {
            InitializeComponent();
            editorVM = new MeasEditUCVM(measurement);
            DataContext = editorVM;
        }
    }

    public class MeasEditUCVM
    {
        public MeasEditUCVM(RandomMeasurement measurement)
        {
            mRandomMeasurement = measurement;
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
