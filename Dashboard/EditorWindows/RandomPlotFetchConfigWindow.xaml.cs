using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Dashboard.EditorWindows
{
    /// <summary>
    /// Interaction logic for RandomPlotFetchConfigWindow.xaml
    /// https://zamjad.wordpress.com/2014/05/20/using-tuple-in-listbox/
    /// </summary>
    public partial class RandomPlotFetchConfigWindow : Window
    {
        public ObservableCollection<Tuple<int, int, int>> Bounds = new ObservableCollection<Tuple<int, int, int>>();
        public RandomPlotFetchConfigWindow()
        {
            InitializeComponent();
            DataContext = Bounds;
        }
    }
}
