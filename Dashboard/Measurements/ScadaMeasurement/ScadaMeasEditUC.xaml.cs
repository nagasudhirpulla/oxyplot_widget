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

namespace Dashboard.Measurements.ScadaMeasurement
{
    /// <summary>
    /// Interaction logic for ScadaMeasEditUC.xaml
    /// </summary>
    public partial class ScadaMeasEditUC : UserControl
    {
        public ScadaMeasEditUCVM editorVM;
        public ScadaMeasEditUC(ScadaMeasurement meas)
        {
            InitializeComponent();
            editorVM = new ScadaMeasEditUCVM(meas);
            DataContext = editorVM;
        }
    }

    public class ScadaMeasEditUCVM
    {
        public ScadaMeasurement mPMUMeasurement;

        public ScadaMeasEditUCVM(ScadaMeasurement meas)
        {
            mPMUMeasurement = meas;
        }

        public string MeasId { get { return mPMUMeasurement.MeasId; } set { mPMUMeasurement.MeasId = value; } }

        public string MeasName { get { return mPMUMeasurement.MeasName; } set { mPMUMeasurement.MeasName = value; } }

        public VariableTime StartTime { get { return mPMUMeasurement.StartTime; } set { mPMUMeasurement.StartTime = value; } }

        public VariableTime EndTime { get { return mPMUMeasurement.EndTime; } set { mPMUMeasurement.EndTime = value; } }

        // implement combobox instead of text input
        public string FetchStrategy { get { return mPMUMeasurement.FetchStrategy; } set { mPMUMeasurement.FetchStrategy = value; } }

        public int FetchPeriodicitySecs { get { return mPMUMeasurement.FetchPeriodicitySecs; } set { mPMUMeasurement.FetchPeriodicitySecs = value; } }
    }
}
