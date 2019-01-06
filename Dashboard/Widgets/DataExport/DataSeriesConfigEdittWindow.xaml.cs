using Dashboard.Interfaces;
using Dashboard.JsonConverters;
using Dashboard.Measurements.PMUMeasurement;
using Dashboard.Measurements.PspMeasurement;
using Dashboard.Measurements.RandomMeasurement;
using Dashboard.Measurements.RandomTimeSeriesMeasurement;
using Dashboard.Measurements.ScadaMeasurement;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace Dashboard.Widgets.DataExport
{
    /// <summary>
    /// Interaction logic for DataSeriesConfigEdittWindow.xaml
    /// </summary>
    public partial class DataSeriesConfigEdittWindow : Window
    {
        public DataSeriesConfigEditorVM EditorVM { get; set; }
        public DataSeriesConfigEdittWindow(DataSeriesConfig config)
        {
            InitializeComponent();
            EditorVM = new DataSeriesConfigEditorVM(config);
            DataContext = EditorVM;
            SetupMeasUC(EditorVM.mLineSeriesConfig.Measurement);
        }

        private void SetupMeasUC(IMeasurement measurement)
        {
            if (MeasEditContainer.Children.Count > 0)
            {
                // Remove the children for replacement with the input measurement
                MeasEditContainer.Children.Clear();
            }
            if (measurement is RandomMeasurement)
            {
                MeasEditContainer.Children.Add(new RandomMeasEditUC((RandomMeasurement)measurement));
            }
            else if (measurement is PMUMeasurement)
            {
                MeasEditContainer.Children.Add(new PMUMeasEditUC((PMUMeasurement)measurement));
            }
            else if (measurement is RandomTimeSeriesMeasurement)
            {
                MeasEditContainer.Children.Add(new RandomTimeMeasEditUC((RandomTimeSeriesMeasurement)measurement));
            }
            else if (measurement is ScadaMeasurement)
            {
                MeasEditContainer.Children.Add(new ScadaMeasEditUC((ScadaMeasurement)measurement));
            }
            else if (measurement is PspMeasurement)
            {
                MeasEditContainer.Children.Add(new PspMeasEditUC((PspMeasurement)measurement));
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

        private void SaveMeasBtnClick(object sender, RoutedEventArgs e)
        {
            SaveMeasurement();
        }

        private void SaveMeasurement()
        {
            string jsonText = JsonConvert.SerializeObject(EditorVM.mLineSeriesConfig.Measurement, Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            string filename = EditorVM.mLineSeriesConfig.Name;
            SaveFileDialog savefileDialog = new SaveFileDialog
            {
                // set a default file name
                FileName = filename,
                // set filters - this can be done in properties as well
                Filter = "meas Files (*.meas)|*.meas|All files (*.*)|*.*"
            };

            if (savefileDialog.ShowDialog() == true)
            {
                File.WriteAllText(savefileDialog.FileName, jsonText);
                Console.WriteLine("Saved the measurement to file!!!");
            }
        }

        private void OpenMeasBtnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // openFileDialog.Multiselect = true;
            openFileDialog.Filter = "meas files (*.meas)|*.meas|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileNames[0];
                OpenMeasurement(filename);
            }
        }

        private void OpenMeasurement(string filename)
        {
            // Load measurement from file string using the json converter
            IMeasurement loadedMeasurement = JsonConvert.DeserializeObject<IMeasurement>(File.ReadAllText(filename), new MeasurementConverter());

            // Replace the current measurement with the loaded measurement
            ReplaceConfigMeasurement(loadedMeasurement);
        }

        private void ReplaceConfigMeasurement(IMeasurement newMeas)
        {
            EditorVM.mLineSeriesConfig.Measurement = newMeas;
            SetupMeasUC(EditorVM.mLineSeriesConfig.Measurement);
        }
    }

    public class DataSeriesConfigEditorVM : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public DataSeriesConfig mLineSeriesConfig;

        // constructor
        public DataSeriesConfigEditorVM(DataSeriesConfig config)
        {
            mLineSeriesConfig = config.Clone();
        }

        // Line plot appearance and name config section
        public string Name
        {
            get { return mLineSeriesConfig.Name; }
            set { mLineSeriesConfig.Name = value; }
        }
    }
}