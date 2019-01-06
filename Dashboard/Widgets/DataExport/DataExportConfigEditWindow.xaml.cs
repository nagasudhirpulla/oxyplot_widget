using Dashboard.Measurements.PMUMeasurement;
using Dashboard.Measurements.PspMeasurement;
using Dashboard.Measurements.RandomMeasurement;
using Dashboard.Measurements.RandomTimeSeriesMeasurement;
using Dashboard.Measurements.ScadaMeasurement;
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

namespace Dashboard.Widgets.DataExport
{
    /// <summary>
    /// Interaction logic for DataExportConfigEditWindow.xaml
    /// </summary>
    public partial class DataExportConfigEditWindow : Window
    {
        public const string PMUMeasOption = "PMU_Measurement";
        public const string ScadaMeasOption = "Scada_Measurement";
        public const string PspMeasOption = "PSP_Measurement";
        public const string RandomTimeSeriesMeasOption = "Random_TimeSeries_Measurement";
        public const string RandomMeasOption = "Random_Measurement";

        public DataExportConfigEditorVM editorVM;

        public DataExportConfigEditWindow(DataExportConfig config)
        {
            InitializeComponent();
            editorVM = new DataExportConfigEditorVM(config);
            DataContext = editorVM;
            ConfigItemsContainer.ItemsSource = editorVM.SeriesConfigListItems;
            string[] comboItemStrings = new string[] { PMUMeasOption, ScadaMeasOption, PspMeasOption, RandomTimeSeriesMeasOption, RandomMeasOption };
            MeasOptionComboBox.ItemsSource = comboItemStrings;
            MeasOptionComboBox.SelectedIndex = 0;
        }

        private void AddSeriesBtnClick(object sender, RoutedEventArgs e)
        {
            // get the selected value from combobox
            string measType = MeasOptionComboBox.SelectedValue.ToString();
            editorVM.AddSeries(measType);
        }

        private int GetSeriesConfigListItemIndexFromButton(Button button)
        {
            int seriesIndex = -1;
            //walk up the tree to find the ListboxItem
            DependencyObject tvi = Helpers.ListUtility.FindParentTreeItem(button, typeof(ListBoxItem));
            //if not null cast the Dependancy object to type of Listbox item.
            if (tvi != null)
            {
                ListBoxItem lbi = tvi as ListBoxItem;
                SeriesConfigListItem seriesConfigListItem = (SeriesConfigListItem)lbi.DataContext;
                // find the index of this list item in the ViewModel List
                seriesIndex = editorVM.SeriesConfigListItems.IndexOf(seriesConfigListItem);
            }
            return seriesIndex;
        }

        private void EditSeriesBtnClick(object sender, RoutedEventArgs e)
        {
            // a button on list view has been clicked
            Button button = sender as Button;
            // get the series config list item index
            int seriesIndex = GetSeriesConfigListItemIndexFromButton(button);
            if (seriesIndex >= 0 && seriesIndex < editorVM.mDataExportConfig.SeriesConfigs.Count)
            {
                DataSeriesConfigEdittWindow seriesConfigEditWindow = new DataSeriesConfigEdittWindow(editorVM.mDataExportConfig.SeriesConfigs[seriesIndex]);
                seriesConfigEditWindow.ShowDialog();
                if (seriesConfigEditWindow.DialogResult == true)
                {
                    // update the series measurement in the vm
                    editorVM.mDataExportConfig.SeriesConfigs[seriesIndex] = seriesConfigEditWindow.EditorVM.mLineSeriesConfig;
                    // update the view model list items
                    editorVM.RefreshSeriesConfigListItemAt(seriesIndex);
                }
            }
        }

        private void DeleteSeriesBtnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete Series ?", "Delete Series", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            // a button on list view has been clicked
            Button button = sender as Button;
            // get the series config list item index
            int seriesIndex = GetSeriesConfigListItemIndexFromButton(button);
            editorVM.DeleteSeriesConfigAt(seriesIndex);
        }

        private void DuplicateSeriesBtnClick(object sender, RoutedEventArgs e)
        {
            // a button on list view has been clicked
            Button button = sender as Button;
            // get the series config list item index
            int seriesIndex = GetSeriesConfigListItemIndexFromButton(button);
            editorVM.DuplicateSeriesConfigAt(seriesIndex);
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

    public class DataExportConfigEditorVM : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public DataExportConfig mDataExportConfig;

        // constructor
        public DataExportConfigEditorVM(DataExportConfig config)
        {
            mDataExportConfig = config.Clone();
            SeriesConfigListItems = new ObservableCollection<SeriesConfigListItem>();
            SyncSeriesConfigListItemsWithConfig();
        }

        public void SyncSeriesConfigListItemsWithConfig()
        {
            SeriesConfigListItems.Clear();
            //create a list of config list items based on LineSeriesConfig items
            for (int configIter = 0; configIter < mDataExportConfig.SeriesConfigs.Count; configIter++)
            {
                SeriesConfigListItems.Add(new SeriesConfigListItem(mDataExportConfig.SeriesConfigs[configIter]));
            }
            OnPropertyChanged("SeriesConfigListItems");
        }

        public void RefreshSeriesConfigListItemAt(int seriesIndex)
        {
            //check if index is in config bounds
            if (seriesIndex >= 0 && seriesIndex < mDataExportConfig.SeriesConfigs.Count)
            {
                // check if seriesIndex is in the display series List item bounds
                if (seriesIndex <= SeriesConfigListItems.Count)
                {
                    SeriesConfigListItems[seriesIndex].SeriesDisplayText = mDataExportConfig.SeriesConfigs[seriesIndex].GetDisplayText();
                }
                RefreshSeriesListView();
            }
        }

        public void DeleteSeriesConfigAt(int seriesIndex)
        {
            //check if index is in config bounds
            if (seriesIndex >= 0 && seriesIndex < mDataExportConfig.SeriesConfigs.Count)
            {
                // check if seriesIndex is in the display series List item bounds
                if (seriesIndex <= SeriesConfigListItems.Count)
                {
                    mDataExportConfig.SeriesConfigs.RemoveAt(seriesIndex);
                }
                SyncSeriesConfigListItemsWithConfig();
            }
        }

        public void DuplicateSeriesConfigAt(int seriesIndex)
        {
            //check if index is in config bounds
            if (seriesIndex >= 0 && seriesIndex < mDataExportConfig.SeriesConfigs.Count)
            {
                // check if seriesIndex is in the display series List item bounds
                if (seriesIndex <= SeriesConfigListItems.Count)
                {
                    DataSeriesConfig duplicatedConfig = mDataExportConfig.SeriesConfigs[seriesIndex].Clone();
                    mDataExportConfig.SeriesConfigs.Insert(seriesIndex, duplicatedConfig);
                }
                SyncSeriesConfigListItemsWithConfig();
            }
        }

        private void RefreshSeriesListView()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(SeriesConfigListItems);
            view.Refresh();
        }

        // Line plot series list items section
        public ObservableCollection<SeriesConfigListItem> SeriesConfigListItems;

        // Line plot appearance and name config section
        public string Name
        {
            get { return mDataExportConfig.Name; }
            set { mDataExportConfig.Name = value; }
        }

        public void AddSeries(string measType)
        {
            DataSeriesConfig lineSeriesConfig = new DataSeriesConfig();
            if (measType == DataExportConfigEditWindow.RandomMeasOption)
            {
                lineSeriesConfig.Measurement = new RandomMeasurement();
            }
            else if (measType == DataExportConfigEditWindow.PMUMeasOption)
            {
                lineSeriesConfig.Measurement = new PMUMeasurement();
            }
            else if (measType == DataExportConfigEditWindow.RandomTimeSeriesMeasOption)
            {
                lineSeriesConfig.Measurement = new RandomTimeSeriesMeasurement();
            }
            else if (measType == DataExportConfigEditWindow.ScadaMeasOption)
            {
                lineSeriesConfig.Measurement = new ScadaMeasurement();
            }
            else if (measType == DataExportConfigEditWindow.PspMeasOption)
            {
                lineSeriesConfig.Measurement = new PspMeasurement();
            }
            mDataExportConfig.SeriesConfigs.Add(lineSeriesConfig);
            SyncSeriesConfigListItemsWithConfig();
        }
    }

    public class SeriesConfigListItem
    {
        public string SeriesDisplayText { get; set; }

        public SeriesConfigListItem() { }

        public SeriesConfigListItem(DataSeriesConfig config)
        {
            SeriesDisplayText = config.GetDisplayText();
        }
    }
}
