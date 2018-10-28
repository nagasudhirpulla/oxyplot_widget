using Dashboard.Interfaces;
using Dashboard.WidgetLayout;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dashboard.Widgets
{
    /// <summary>
    /// Interaction logic for BaseWidget.xaml
    /// https://stackoverflow.com/questions/9094486/adding-children-to-usercontrol
    /// Creating floating widgets in grid using the panel.zindex property - https://stackoverflow.com/questions/5450985/how-to-make-overlay-control-above-all-other-controls
    /// </summary>
    [ContentProperty(nameof(Children))]
    public partial class WidgetFrame : UserControl, IWidgetContainer, INotifyPropertyChanged
    {
        public static readonly DependencyPropertyKey ChildrenProperty = DependencyProperty.RegisterReadOnly(
            nameof(Children),
            typeof(UIElementCollection),
            typeof(WidgetFrame),
            new PropertyMetadata());

        public UIElementCollection Children
        {
            get { return (UIElementCollection)GetValue(ChildrenProperty.DependencyProperty); }
            private set { SetValue(ChildrenProperty, value); }
        }

        public WidgetFrame()
        {
            InitializeComponent();
            Children = PART_Host.Children;
            DataContext = this;
        }

        private IWidget mWidget = null;

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

        private void OnChanged(object sender, EventArgs e)
        {
            Changed?.Invoke(sender, e);
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
                OnPropertyChanged("Dimension");
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

        public async Task RefreshData()
        {
            // call child widget to refresh data
            await mWidget?.RefreshData();
        }

        public void SetWidget(IWidget widget)
        {
            // check if widget is valid
            if (widget != null && widget is UserControl)
            {
                //check if already child is present
                int widgetCount = Children.Count;
                // remove widget children if present using the widget child count
                for (int childIter = 0; childIter < widgetCount; childIter++)
                {
                    if (Children[0] is IWidget)
                    {
                        ((IWidget)Children[0]).DoCleanUpForDeletion();
                    }
                    Children.RemoveAt(0);
                }
                // add the widget to the container
                Children.Add((UserControl)widget);
                widget.Changed = (EventArgs e) => Changed?.Invoke(widget, e);
                mWidget = widget;
            }
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
