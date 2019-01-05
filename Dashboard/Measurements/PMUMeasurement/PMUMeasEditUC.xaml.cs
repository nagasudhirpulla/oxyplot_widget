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

namespace Dashboard.Measurements.PMUMeasurement
{
    /// <summary>
    /// Interaction logic for PMUMeasEditUC.xaml
    /// </summary>
    public partial class PMUMeasEditUC : UserControl
    {
        public PMUMeasEditUCVM editorVM;
        public PMUMeasEditUC(PMUMeasurement meas)
        {
            InitializeComponent();
            editorVM = new PMUMeasEditUCVM(meas);
            DataContext = editorVM;
        }

        private void ShowPmuMeasPicker_Click(object sender, RoutedEventArgs e)
        {
            PMUMeasPickerWindow pmuMeasPicker = new PMUMeasPickerWindow();
            pmuMeasPicker.ShowDialog();
            if (pmuMeasPicker.DialogResult == true)
            {
                // todo set the measurement label and fields
            }
        }
    }

    public class PMUMeasEditUCVM
    {
        public PMUMeasurement mPMUMeasurement;

        public PMUMeasEditUCVM(PMUMeasurement meas)
        {
            mPMUMeasurement = meas;
        }

        public int MeasId { get { return mPMUMeasurement.MeasId; } set { mPMUMeasurement.MeasId = value; } }

        public string MeasName { get { return mPMUMeasurement.MeasName; } set { mPMUMeasurement.MeasName = value; } }

        public VariableTime StartTime { get { return mPMUMeasurement.StartTime; } set { mPMUMeasurement.StartTime = value; } }

        public VariableTime EndTime { get { return mPMUMeasurement.EndTime; } set { mPMUMeasurement.EndTime = value; } }

        public TimeSpan MaxFetchSize { get { return mPMUMeasurement.MaxFetchSize; } set { mPMUMeasurement.MaxFetchSize = value; } }

        public TimeSpan MaxResolution { get { return mPMUMeasurement.MaxResolution; } set { mPMUMeasurement.MaxResolution = value; } }

        public SamplingStrategy SamplingStrategy { get { return mPMUMeasurement.SamplingStrategy; } set { mPMUMeasurement.SamplingStrategy = value; } }

    }
}
