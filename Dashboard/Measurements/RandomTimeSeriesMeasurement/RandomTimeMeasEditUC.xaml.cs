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

namespace Dashboard.Measurements.RandomTimeSeriesMeasurement
{
    /// <summary>
    /// Interaction logic for RandomTimeMeasEditUC.xaml
    /// </summary>
    public partial class RandomTimeMeasEditUC : UserControl
    {
        public MeasEditUCVM editorVM;
        public RandomTimeMeasEditUC(RandomTimeSeriesMeasurement measurement)
        {
            InitializeComponent();
            editorVM = new MeasEditUCVM(measurement);
            DataContext = editorVM;
        }

        public class MeasEditUCVM
        {
            public MeasEditUCVM(RandomTimeSeriesMeasurement measurement)
            {
                mRandomMeasurement = measurement;
            }

            public RandomTimeSeriesMeasurement mRandomMeasurement { get; set; }

            public double Low { get { return mRandomMeasurement.Low; } set { mRandomMeasurement.Low = value; } }

            public double High { get { return mRandomMeasurement.High; } set { mRandomMeasurement.High = value; } }

            public DateTime FromTime { get { return mRandomMeasurement.FromTime; } set { mRandomMeasurement.FromTime = value; } }

            public DateTime ToTime { get { return mRandomMeasurement.ToTime; } set { mRandomMeasurement.ToTime = value; } }

            public TimeSpan TimeResolution { get { return mRandomMeasurement.TimeResolution; } set { mRandomMeasurement.TimeResolution = value; } }
        }
    }
}
