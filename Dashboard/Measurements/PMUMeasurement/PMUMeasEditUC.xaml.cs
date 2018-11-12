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

        public DateTime StartTime { get { return mPMUMeasurement.StartTime; } set { mPMUMeasurement.StartTime = value; } }

        public DateTime EndTime { get { return mPMUMeasurement.EndTime; } set { mPMUMeasurement.EndTime = value; } }
    }
}
