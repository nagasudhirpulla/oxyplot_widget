using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            String filenameStr = (String)((App)Application.Current).Properties["FilePathArgName"];
            OpenFileName(filenameStr);
        }

        private void OpenFileName(string filenameStr)
        {
            if (filenameStr != null)
            {
                //String fileStr = File.ReadAllText(filenameStr);
                Console.WriteLine($"Opening {filenameStr}");
                DashboardUC.OpenFileName(filenameStr);
            }
        }

        private void AddBlankWidget_Click(object sender, RoutedEventArgs e)
        {
            DashboardUC.AddNewBlankWidget();
        }

        private void AddPlotWidget_Click(object sender, RoutedEventArgs e)
        {
            DashboardUC.AddNewPlotWidget();
        }
    }
}
