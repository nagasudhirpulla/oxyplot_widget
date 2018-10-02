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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OxyplotWidget.PlotWidget
{
    /// <summary>
    /// Interaction logic for OxyplotUC.xaml
    /// </summary>
    public partial class OxyplotUC : UserControl
    {
        public PlotViewModel PlotViewModel { get; set; } = new PlotViewModel();

        public OxyplotUC()
        {
            InitializeComponent();
            DataContext = PlotViewModel;
        }

        /// <summary>
        /// Refreshes the widget data. Not required
        /// </summary>
        public void RefreshWidget()
        {
            
        }
    }
}
