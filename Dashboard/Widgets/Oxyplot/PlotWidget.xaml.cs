using Dashboard.EditorWindows;
using Dashboard.Interfaces;
using Dashboard.WidgetLayout;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyplotWidget.PlotWidget;
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

namespace Dashboard.Widgets.Oxyplot
{
    /// <summary>
    /// Interaction logic for OxyplotWidget.xaml
    /// </summary>
    public partial class PlotWidget : UserControl, IWidget, INotifyPropertyChanged
    {
        public PlotWidget()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Send Messages to Dashboard using this event handler
        public event EventHandler<EventArgs> Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        private WidgetPosition mPosition = new WidgetPosition();
        public WidgetPosition Position
        {
            get => mPosition;
            set
            {
                mPosition = value;
                OnPropertyChanged("Position");                
            }
        }

        private WidgetDimension mDimension = new WidgetDimension();
        public WidgetDimension Dimension
        {
            get => mDimension;
            set
            {
                mDimension = value;
                //OnPropertyChanged("Dimension");
            }
        }


        private WidgetAppearance mWidgetAppearance = new WidgetAppearance();
        public WidgetAppearance WidgetAppearance
        {
            get => mWidgetAppearance;
            set
            {
                mWidgetAppearance = value;
                OnPropertyChanged("WidgetAppearance");
                //OnPropertyChanged("WidgetAppearance.BorderColor");
                //OnPropertyChanged("WidgetAppearance.BackgroundColor");
            }
        }

        public PlotViewModel PlotViewModel { get; set; } = new PlotViewModel();

        public async Task RefreshData()
        {
            PlotViewModel.AddNewSeries(new LineSeries
            {
                Title = "TimeSeries"
            });
            int waitTime = 300;
            Random rnd = new Random();
            List<DataPoint> points = new List<DataPoint> {
                new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddHours(rnd.Next(-13, -1))), 5),
                new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddHours(rnd.Next(-13, -1))), 10),
                new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddHours(rnd.Next(-13, -1))), 64),
                };
            int seriesIndex = PlotViewModel.LinePlotModel.Series.Count - 1;
            for (int iter = 0; iter < points.Count; iter++)
            {
                await Task.Delay(waitTime);
                // Add point in series
                PlotViewModel.AddPointInLineSeries(seriesIndex, points[iter]);
            }
            PlotViewModel.MakeXAxisDateTime();
            PlotViewModel.SetXAxisStringFormat("dd-MMM-yyyy");
        }

        private async void RefreshDataBtn_Click(object sender, RoutedEventArgs e)
        {
            await RefreshData();
        }

        private void EditPositionBtn_Click(object sender, RoutedEventArgs e)
        {
            OnChanged(new CellPosChangeReqArgs());            
        }
    }
}
