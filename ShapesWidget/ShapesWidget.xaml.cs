using ShapesWidget.States;
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

namespace ShapesWidget
{
    /// <summary>
    /// Interaction logic for ShapesWidget.xaml
    /// </summary>
    public partial class ShapesWidget : UserControl
    {
        private WidgetViewModel WidgetModel = new WidgetViewModel(new WidgetState());
        private List<LayerManager> LayerManagers = new List<LayerManager>();

        public ShapesWidget()
        {
            InitializeComponent();
            DataContext = WidgetModel;
            InflateWidgetState();
        }

        /// <summary>
        /// Use widget state to inflate the layers and shapes and
        /// bind the states to the respective inflated objects
        /// </summary>
        private void InflateWidgetState()
        {
            for (int layerIter = 0; layerIter < WidgetModel.WidgetState_.LayerStates.Count; layerIter++)
            {
                LayerManagers.Add(new LayerManager(WidgetCanvas, WidgetModel.WidgetState_.LayerStates[layerIter]));
            }
        }
    }

    public class WidgetViewModel : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public WidgetState WidgetState_ = new WidgetState();

        public WidgetViewModel(WidgetState state)
        {
            WidgetState_ = state;
        }

        public Color BackgroundColor { get { return WidgetState_.BackgroundColor; } set { WidgetState_.BackgroundColor = value; OnPropertyChanged("SeriesConfigListItems"); } }
    }
}
