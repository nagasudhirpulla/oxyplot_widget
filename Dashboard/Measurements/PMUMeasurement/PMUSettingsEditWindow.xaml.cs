using PMUDataLayer.Config;
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

namespace Dashboard.Measurements.PMUMeasurement
{
    /// <summary>
    /// Interaction logic for PMUSettingsEditWindow.xaml
    /// </summary>
    public partial class PMUSettingsEditWindow : Window
    {
        PMUSettingsEditVM SettingsEditVM = new PMUSettingsEditVM();

        public PMUSettingsEditWindow()
        {
            InitializeComponent();
            DataContext = SettingsEditVM;
        }

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

    public class PMUSettingsEditVM
    {
        public ConfigurationManagerJSON ConfigurationManager { get; set; } = new ConfigurationManagerJSON();

        public PMUSettingsEditVM()
        {
            ConfigurationManager.Initialize();
        }

        public void Save()
        {
            ConfigurationManager.Save();
        }

        public string Host { get { return ConfigurationManager.Host; } set { ConfigurationManager.Host = value; } }
        public string Path { get { return ConfigurationManager.Path; } set { ConfigurationManager.Path = value; } }
        public int Port { get { return ConfigurationManager.Port; } set { ConfigurationManager.Port = value; } }
        public string UserName { get { return ConfigurationManager.UserName; } set { ConfigurationManager.UserName = value; } }
        public string Password { get { return ConfigurationManager.Password; } set { ConfigurationManager.Password = value; } }
        public int LatencyTime { get { return ConfigurationManager.LatencyTime; } set { ConfigurationManager.LatencyTime = value; } }
    }
}
