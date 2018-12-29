using PspDataLayer;
using PspDataLayer.Config;
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

namespace Dashboard.Measurements.PspMeasurement
{
    /// <summary>
    /// Interaction logic for PspMeasPickerWindow.xaml
    /// List view with filtering
    /// </summary>
    public partial class PspMeasPickerWindow : Window
    {
        public PspMeasPickerWindow()
        {
            InitializeComponent();
            PopulatePspLabelsAsync();
        }

        public string SelectedLabel { get; set; } = "gujarat_thermal_mu";

        private async void PopulatePspLabelsAsync()
        {
            // get the label objects from psp data layer
            ConfigurationManagerJSON configManager = new ConfigurationManagerJSON();
            configManager.Initialize();
            PspDataAdapter adapter = new PspDataAdapter { ConfigurationManager = configManager };
            List<PspLabelApiItem> results = await adapter.GetMeasurementLabelsAsync();

            // Bind the list view with psp label items
            lvMeasurements.ItemsSource = results;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvMeasurements.ItemsSource);
            view.Filter = MeasurementsFilter;
        }

        private bool MeasurementsFilter(object item)
        {
            if (String.IsNullOrEmpty(TxtFilter.Text))
                return true;
            else
                return ((item as PspLabelApiItem).Label.IndexOf(TxtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvMeasurements.ItemsSource).Refresh();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            SelectedLabel = ((PspLabelApiItem)lvMeasurements.SelectedItem).Label;
            DialogResult = true;
        }
    }
}
