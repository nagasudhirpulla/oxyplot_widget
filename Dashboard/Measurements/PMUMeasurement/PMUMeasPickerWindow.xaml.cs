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
using System.Xml.Linq;
using PMUDataLayer;
using PMUDataLayer.Config;

namespace Dashboard.Measurements.PMUMeasurement
{
    /// <summary>
    /// Interaction logic for PMUMeasPickerWindow.xaml
    /// </summary>
    public partial class PMUMeasPickerWindow : Window
    {
        public PMUMeasPickerWindow()
        {
            InitializeComponent();
            PopulatePMUMeasurements();
        }
        private XDocument measXml;

        private void PopulatePMUMeasurements()
        {
            // get the label objects from psp data layer
            ConfigurationManagerJSON configManager = new ConfigurationManagerJSON();
            configManager.Initialize();
            HistoryDataAdapter adapter = new HistoryDataAdapter(configManager);
            measXml = adapter.GetMeasXml();
            // Bind the tree view with xml
            MeasTree.DataContext = measXml;
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            PopulatePMUMeasurements();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
