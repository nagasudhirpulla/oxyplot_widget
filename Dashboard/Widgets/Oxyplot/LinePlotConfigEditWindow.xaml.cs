using Dashboard.Interfaces;
using Dashboard.Measurements.PMUMeasurement;
using Dashboard.Measurements.RandomMeasurement;
using Dashboard.Measurements.RandomTimeSeriesMeasurement;
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

namespace Dashboard.Widgets.Oxyplot
{
    /// <summary>
    /// Interaction logic for LinePlotConfigEditWindow.xaml
    /// </summary>
    public partial class LinePlotConfigEditWindow : Window
    {
        public const string PMUMeasOption = "PMU_Measurement";
        public const string RandomTimeSeriesMeasOption = "Random_TimeSeries_Measurement";
        public const string RandomMeasOption = "Random_Measurement";

        public LinePlotConfigEditorVM editorVM;

        public LinePlotConfigEditWindow(LinePlotConfig config)
        {
            InitializeComponent();
            editorVM = new LinePlotConfigEditorVM(config);
            DataContext = editorVM;
            ConfigItemsContainer.ItemsSource = editorVM.SeriesConfigListItems;
            string[] comboItemStrings = new string[] { PMUMeasOption, RandomTimeSeriesMeasOption, RandomMeasOption };
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
            if (seriesIndex >= 0 && seriesIndex < editorVM.mLinePlotConfig.SeriesConfigs.Count)
            {
                /*
                // this means the mesurement is present in ViewModel series config list items
                IMeasurement seriesMeas = editorVM.mLinePlotConfig.SeriesConfigs[seriesIndex].Measurement;

                // handle if measurement is a Random Measurement
                if (seriesMeas is RandomMeasurement randomSeriesMeas)
                {
                    RandomMeasEditWindow randomMeasEditWindow = new RandomMeasEditWindow(randomSeriesMeas);
                    randomMeasEditWindow.ShowDialog();
                    if (randomMeasEditWindow.DialogResult == true)
                    {
                        // update the series measurement in the vm
                        editorVM.mLinePlotConfig.SeriesConfigs[seriesIndex].Measurement = randomMeasEditWindow.editorVM.mRandomMeasurement;
                        // update the view model list items
                        editorVM.RefreshSeriesConfigListItemAt(seriesIndex);
                    }
                }
                */
                LineSeriesConfigEditWindow lineSeriesConfigEditWindow = new LineSeriesConfigEditWindow(editorVM.mLinePlotConfig.SeriesConfigs[seriesIndex]);
                lineSeriesConfigEditWindow.ShowDialog();
                if (lineSeriesConfigEditWindow.DialogResult == true)
                {
                    // update the series measurement in the vm
                    editorVM.mLinePlotConfig.SeriesConfigs[seriesIndex] = lineSeriesConfigEditWindow.EditorVM.mLineSeriesConfig;
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

    public class LinePlotConfigEditorVM : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public LinePlotConfig mLinePlotConfig;

        // constructor
        public LinePlotConfigEditorVM(LinePlotConfig config)
        {
            mLinePlotConfig = config.Clone();
            SeriesConfigListItems = new ObservableCollection<SeriesConfigListItem>();
            SyncSeriesConfigListItemsWithConfig();
        }

        public void SyncSeriesConfigListItemsWithConfig()
        {
            SeriesConfigListItems.Clear();
            //create a list of config list items based on LineSeriesConfig items
            for (int configIter = 0; configIter < mLinePlotConfig.SeriesConfigs.Count; configIter++)
            {
                SeriesConfigListItems.Add(new SeriesConfigListItem(mLinePlotConfig.SeriesConfigs[configIter]));
            }
            OnPropertyChanged("SeriesConfigListItems");
        }

        public void RefreshSeriesConfigListItemAt(int seriesIndex)
        {
            //check if index is in config bounds
            if (seriesIndex >= 0 && seriesIndex < mLinePlotConfig.SeriesConfigs.Count)
            {
                // check if seriesIndex is in the display series List item bounds
                if (seriesIndex <= SeriesConfigListItems.Count)
                {
                    SeriesConfigListItems[seriesIndex].SeriesDisplayText = mLinePlotConfig.SeriesConfigs[seriesIndex].GetDisplayText();
                }
                RefreshSeriesListView();
            }
        }

        public void DeleteSeriesConfigAt(int seriesIndex)
        {
            //check if index is in config bounds
            if (seriesIndex >= 0 && seriesIndex < mLinePlotConfig.SeriesConfigs.Count)
            {
                // check if seriesIndex is in the display series List item bounds
                if (seriesIndex <= SeriesConfigListItems.Count)
                {
                    mLinePlotConfig.SeriesConfigs.RemoveAt(seriesIndex);
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
            get { return mLinePlotConfig.Name; }
            set { mLinePlotConfig.Name = value; }
        }

        public Color Background
        {
            get { return mLinePlotConfig.Appearance.BackgroundColor; }
            set { mLinePlotConfig.Appearance.BackgroundColor = value; }
        }

        public Color Foreground
        {
            get { return mLinePlotConfig.Appearance.ForegroundColor; }
            set { mLinePlotConfig.Appearance.ForegroundColor = value; }
        }

        public Color TextColor
        {
            get { return mLinePlotConfig.Appearance.TextColor; }
            set { mLinePlotConfig.Appearance.TextColor = value; }
        }

        public Color MajorAxesLineColor
        {
            get { return mLinePlotConfig.Appearance.MajorAxesLineColor; }
            set { mLinePlotConfig.Appearance.MajorAxesLineColor = value; }
        }

        public bool IsXAxisDateTime
        {
            get { return mLinePlotConfig.Appearance.IsXAxisDateTime; }
            set { mLinePlotConfig.Appearance.IsXAxisDateTime = value; OnPropertyChanged("DateTimeSettingsVisibility"); }
        }

        public Visibility DateTimeSettingsVisibility
        {
            get
            {
                Visibility visibility = Visibility.Visible;
                if (!IsXAxisDateTime)
                {
                    visibility = Visibility.Collapsed;
                }
                return visibility;
            }
        }

        public string AxisTimeFormat
        {
            get { return mLinePlotConfig.Appearance.AxisTimeFormat; }
            set { mLinePlotConfig.Appearance.AxisTimeFormat = value; }
        }

        public void AddSeries(string measType)
        {
            LineSeriesConfig lineSeriesConfig = new LineSeriesConfig();
            if (measType == LinePlotConfigEditWindow.RandomMeasOption)
            {
                lineSeriesConfig.Measurement = new RandomMeasurement();
            }
            else if (measType == LinePlotConfigEditWindow.PMUMeasOption)
            {
                lineSeriesConfig.Measurement = new PMUMeasurement();
            }
            else if (measType == LinePlotConfigEditWindow.RandomTimeSeriesMeasOption)
            {
                lineSeriesConfig.Measurement = new RandomTimeSeriesMeasurement();
            }
            mLinePlotConfig.SeriesConfigs.Add(lineSeriesConfig);
            SyncSeriesConfigListItemsWithConfig();
        }
    }

    public class SeriesConfigListItem
    {
        public string SeriesDisplayText { get; set; }

        public SeriesConfigListItem() { }

        public SeriesConfigListItem(LineSeriesConfig config)
        {
            SeriesDisplayText = config.GetDisplayText();
        }
    }
}
