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
