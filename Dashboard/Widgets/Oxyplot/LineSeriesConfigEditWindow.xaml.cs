﻿using System;
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
using Dashboard.Interfaces;
using Dashboard.Measurements.PMUMeasurement;
using Dashboard.Measurements.RandomMeasurement;
using Dashboard.Measurements.RandomTimeSeriesMeasurement;
using Dashboard.Measurements.ScadaMeasurement;
using Microsoft.Win32;
using Newtonsoft.Json;

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
            string jsonText = JsonConvert.SerializeObject(EditorVM.mLineSeriesConfig.Measurement);
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
            OpenMeasurement();
        }

        private void OpenMeasurement()
        {
            // todo implement this
            MessageBox.Show("Yet to be implemented");
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

        public int TimeShiftYears { get { return mLineSeriesConfig.DisplayTimeShift.Years; } set { mLineSeriesConfig.DisplayTimeShift.Years = value; } }
        public int TimeShiftMonths { get { return mLineSeriesConfig.DisplayTimeShift.Months; } set { mLineSeriesConfig.DisplayTimeShift.Months = value; } }
        public int TimeShiftDays { get { return mLineSeriesConfig.DisplayTimeShift.Days; } set { mLineSeriesConfig.DisplayTimeShift.Days = value; } }
        public int TimeShiftHours { get { return mLineSeriesConfig.DisplayTimeShift.Hours; } set { mLineSeriesConfig.DisplayTimeShift.Hours = value; } }
        public int TimeShiftMinutes { get { return mLineSeriesConfig.DisplayTimeShift.Minutes; } set { mLineSeriesConfig.DisplayTimeShift.Minutes = value; } }
        public int TimeShiftSeconds { get { return mLineSeriesConfig.DisplayTimeShift.Seconds; } set { mLineSeriesConfig.DisplayTimeShift.Seconds = value; } }

    }
}
