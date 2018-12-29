using Dashboard.Helpers;
using Dashboard.UserControls.VariableTimePicker;
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

namespace Dashboard.Measurements.PspMeasurement
{
    /// <summary>
    /// Interaction logic for PspMeasEditUC.xaml
    /// </summary>
    public partial class PspMeasEditUC : UserControl
    {
        public PspMeasEditUCVM editorVM;
        public PspMeasEditUC(PspMeasurement meas)
        {
            InitializeComponent();
            editorVM = new PspMeasEditUCVM(meas);
            DataContext = editorVM;
        }
    }

    public class PspMeasEditUCVM
    {
        public PspMeasurement mPspMeasurement;

        public PspMeasEditUCVM(PspMeasurement meas)
        {
            mPspMeasurement = meas;
        }

        public string MeasLabel { get { return mPspMeasurement.MeasLabel; } set { mPspMeasurement.MeasLabel = value; } }

        public string MeasName { get { return mPspMeasurement.MeasName; } set { mPspMeasurement.MeasName = value; } }

        public VariableTime StartTime { get { return mPspMeasurement.StartTime; } set { mPspMeasurement.StartTime = value; } }

        public VariableTime EndTime { get { return mPspMeasurement.EndTime; } set { mPspMeasurement.EndTime = value; } }

        public TimeSpan MaxFetchSize { get { return mPspMeasurement.MaxFetchSize; } set { mPspMeasurement.MaxFetchSize = value; } }

        public TimeSpan MaxResolution { get { return mPspMeasurement.MaxResolution; } set { mPspMeasurement.MaxResolution = value; } }

        public SamplingStrategy SamplingStrategy { get { return mPspMeasurement.SamplingStrategy; } set { mPspMeasurement.SamplingStrategy = value; } }

    }
}
