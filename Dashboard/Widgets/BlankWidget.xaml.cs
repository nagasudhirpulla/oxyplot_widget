using Dashboard.Interfaces;
using Dashboard.WidgetLayout;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dashboard.Widgets
{
    /// <summary>
    /// Interaction logic for BlankWidget.xaml
    /// </summary>
    public partial class BlankWidget : UserControl, IWidget, INotifyPropertyChanged
    {
        public BlankWidget()
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

        private WidgetPosition mPosition = new WidgetPosition();
        public WidgetPosition Position
        {
            get => mPosition;
            set
            {
                mPosition = value;
                //OnPropertyChanged("Position");
                OnPropertyChanged("Position.Row");
                OnPropertyChanged("Position.RowSapan");
                OnPropertyChanged("Position.Column");
                OnPropertyChanged("Position.ColSapan");
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
                //OnPropertyChanged("WidgetAppearance");
                OnPropertyChanged("WidgetAppearance.BorderColor");
                OnPropertyChanged("WidgetAppearance.BackgroundColor");                
            }
        }

        public async Task RefreshData()
        {
            // do nothing
        }
    }
}
