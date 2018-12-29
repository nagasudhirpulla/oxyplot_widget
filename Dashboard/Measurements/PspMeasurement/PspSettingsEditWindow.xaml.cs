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
    /// Interaction logic for PspSettingsEditWindow.xaml
    /// </summary>
    public partial class PspSettingsEditWindow : Window
    {
        public PspSettingsEditWindow()
        {
            InitializeComponent();
            DataContext = SettingsEditVM;
        }

        PspSettingsEditVM SettingsEditVM = new PspSettingsEditVM();

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Save Changes ?", "Save Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            else
            {
                SettingsEditVM.Save();
                DialogResult = true;
            }
        }
    }

    public class PspSettingsEditVM
    {
        public ConfigurationManagerJSON ConfigurationManager { get; set; } = new ConfigurationManagerJSON();

        public PspSettingsEditVM()
        {
            ConfigurationManager.Initialize();
        }

        public void Save()
        {
            ConfigurationManager.Save();
        }

        public string Host { get { return ConfigurationManager.Host; } set { ConfigurationManager.Host = value; } }
        public string Path { get { return ConfigurationManager.Path; } set { ConfigurationManager.Path = value; } }
        public string LabelsPath { get { return ConfigurationManager.LabelsPath; } set { ConfigurationManager.LabelsPath = value; } }
        public int Port { get { return ConfigurationManager.Port; } set { ConfigurationManager.Port = value; } }
    }
}
