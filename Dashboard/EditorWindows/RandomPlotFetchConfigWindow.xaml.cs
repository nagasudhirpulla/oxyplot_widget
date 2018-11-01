using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Dashboard.EditorWindows
{
    /// <summary>
    /// Interaction logic for RandomPlotFetchConfigWindow.xaml
    /// https://zamjad.wordpress.com/2014/05/20/using-tuple-in-listbox/
    /// </summary>
    public partial class RandomPlotFetchConfigWindow : Window
    {
        private EditorVM editorVM;
        public RandomPlotFetchConfigWindow(List<Tuple<int, int, int>> bounds)
        {
            InitializeComponent();
            editorVM = new EditorVM(bounds);
            DataContext = editorVM;
            ConfigItemsContainer.ItemsSource = editorVM.RandomSeriesConfigItems;
        }

        public List<Tuple<int, int, int>> GetConfigTuples()
        {
            List<Tuple<int, int, int>> tuples = new List<Tuple<int, int, int>>();
            for (int configItemIter = 0; configItemIter < editorVM.RandomSeriesConfigItems.Count; configItemIter++)
            {
                RandomSeriesConfigItem item = editorVM.RandomSeriesConfigItems[configItemIter];
                tuples.Add(new Tuple<int, int, int>(item.mLow, item.mHigh, item.mNumPnts));
            }
            return tuples;
        }

        private void AddBtnClick(object sender, RoutedEventArgs e)
        {
            editorVM.RandomSeriesConfigItems.Add(new RandomSeriesConfigItem(0, 10, 50));
        }

        private void ListItemDeleteBtnClick(object sender, RoutedEventArgs e)
        {
            // https://github.com/nagasudhirpulla/wpf_scada_dashboard/blob/master/WPFScadaDashboard/DashboardUserControls/DataPointsConfigWindow.xaml.cs
            if (MessageBox.Show("Remove Series ?", "Plot Series Configuration", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            else
            {
                //a button on list view has been clicked
                Button button = sender as Button;
                //walk up the tree to find the ListboxItem
                DependencyObject tvi = Helpers.ListUtility.FindParentTreeItem(button, typeof(ListBoxItem));
                //if not null cast the Dependancy object to type of Listbox item.
                if (tvi != null)
                {
                    ListBoxItem lbi = tvi as ListBoxItem;
                    // Delete the object from Observable Collection
                    RandomSeriesConfigItem seriesConfigItem = (RandomSeriesConfigItem)lbi.DataContext;
                    editorVM.RandomSeriesConfigItems.Remove(seriesConfigItem);
                }
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
    }

    public class EditorVM : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<RandomSeriesConfigItem> RandomSeriesConfigItems;
        public EditorVM(List<Tuple<int, int, int>> bounds)
        {
            RandomSeriesConfigItems = new ObservableCollection<RandomSeriesConfigItem>();
            for (int boundsIter = 0; boundsIter < bounds.Count; boundsIter++)
            {
                RandomSeriesConfigItems.Add(RandomSeriesConfigItem.ConvertFromTuple(bounds[boundsIter]));
            }
            OnPropertyChanged("RandomSeriesConfigItems");
        }
    }

    public class RandomSeriesConfigItem
    {
        public int mLow;
        public int mHigh;
        public int mNumPnts;

        public static RandomSeriesConfigItem ConvertFromTuple(Tuple<int, int, int> tuple)
        {
            RandomSeriesConfigItem item = new RandomSeriesConfigItem
            {
                mLow = tuple.Item1,
                mHigh = tuple.Item2,
                mNumPnts = tuple.Item3
            };
            return item;
        }

        public RandomSeriesConfigItem()
        {

        }

        public RandomSeriesConfigItem(int low, int high, int numPnts)
        {
            mLow = low; mHigh = high; mNumPnts = numPnts;
        }

        public string Low
        {
            get { return mLow.ToString(); }
            set
            {
                // check if value is a number
                bool isNumeric = int.TryParse(value, out int integerInput);
                if (isNumeric && integerInput >= 0)
                {
                    mLow = integerInput;
                }
            }
        }

        public string High
        {
            get { return mHigh.ToString(); }
            set
            {
                // check if value is a number
                bool isNumeric = int.TryParse(value, out int integerInput);
                if (isNumeric && integerInput >= 0)
                {
                    mHigh = integerInput;
                }
            }
        }

        public string NumPnts
        {
            get { return mNumPnts.ToString(); }
            set
            {
                // check if value is a number
                bool isNumeric = int.TryParse(value, out int integerInput);
                if (isNumeric && integerInput >= 0)
                {
                    mNumPnts = integerInput;
                }
            }
        }
    }
}
