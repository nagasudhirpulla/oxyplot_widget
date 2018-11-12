using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Dashboard.Interfaces;
using Dashboard.Measurements.PMUMeasurement;
using Dashboard.Measurements.RandomMeasurement;

namespace Dashboard.Widgets.Oxyplot
{
    /// <summary>
    /// Interaction logic for LineSeriesConfigEditWindow.xaml
    /// </summary>
    public partial class LineSeriesConfigEditWindow : Window
    {
        public LineSeriesConfigEditorVM EditorVM { get; set; }
        public LineSeriesConfigEditWindow(LineSeriesConfig config)
        {
            InitializeComponent();
            EditorVM = new LineSeriesConfigEditorVM(config);
            DataContext = EditorVM;
            SetupMeasUC(EditorVM.mLineSeriesConfig.Measurement);
        }

        private void SetupMeasUC(IMeasurement measurement)
        {
            if (measurement is RandomMeasurement)
            {
                MeasEditContainer.Children.Add(new RandomMeasEditUC((RandomMeasurement)measurement));
            }
            else if(measurement is PMUMeasurement)
            {
                MeasEditContainer.Children.Add(new PMUMeasEditUC((PMUMeasurement)measurement));
            }
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

    public class LineSeriesConfigEditorVM : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public LineSeriesConfig mLineSeriesConfig;

        // constructor
        public LineSeriesConfigEditorVM(LineSeriesConfig config)
        {
            mLineSeriesConfig = config.Clone();
        }

        // Line plot appearance and name config section
        public string Name
        {
            get { return mLineSeriesConfig.Name; }
            set { mLineSeriesConfig.Name = value; }
        }

        public Color Color
        {
            get { return mLineSeriesConfig.Appearance.Color; }
            set { mLineSeriesConfig.Appearance.Color = value; }
        }
    }
}
