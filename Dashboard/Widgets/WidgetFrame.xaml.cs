using Dashboard.Interfaces;
using Dashboard.States;
using Dashboard.WidgetLayout;
using Dashboard.Widgets.Oxyplot;
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
            AutoFetchManager_ = new WidgetContainerAutoFetchManager(Fetch_Timer_TickAsync, AutoFetchState_);
        }

        private IWidget mWidget = null;

        public WidgetContainerAutoFetchState AutoFetchState_ { get; set; } = new WidgetContainerAutoFetchState();

        public WidgetContainerAutoFetchState AutoFetchState
        {
            get { return AutoFetchState_; }
            set { AutoFetchState_ = value; UpdateAutoFetchState(); }
        }

        public async Task Fetch_Timer_TickAsync(object sender, EventArgs e)
        {
            // since the timer is suppressed, the refresh may be done by the global scheduler. 
            // Hence refresh only if not suppressed
            if (!AutoFetchState_.IsSuppressed)
            {
                await RefreshData();
            }
        }

        private WidgetContainerAutoFetchManager AutoFetchManager_;

        private void UpdateAutoFetchState(WidgetContainerAutoFetchState state)
        {
            AutoFetchState_ = state;
            UpdateAutoFetchState();
        }

        private void UpdateAutoFetchState()
        {
            AutoFetchManager_.WidgetContainerAutoFetchState = AutoFetchState_;
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

        public void OpenConfigWindow()
        {
            mWidget?.OpenConfigWindow();
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
                    DoWidgetCleanUp();
                    RemoveWidget();
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

        private void EditConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            OnChanged(new CellMessageArgs() { Message = CellMessageArgs.ConfigWindowOpenRequest });
        }

        public void DoWidgetCleanUp()
        {
            // free up the wiget resources inorder to prepare for deletion
            if (Children[0] is IWidget)
            {
                ((IWidget)Children[0]).DoCleanUpForDeletion();
            }
        }

        public void RemoveWidget()
        {
            Children.RemoveAt(0);
        }

        private void UpBtn_Click(object sender, RoutedEventArgs e)
        {
            OnChanged(new CellPosChangeReqArgs(CellPosChangeMsgType.POS_UP));
        }

        private void DownBtn_Click(object sender, RoutedEventArgs e)
        {
            OnChanged(new CellPosChangeReqArgs(CellPosChangeMsgType.POS_DOWN));
        }

        private void RightBtn_Click(object sender, RoutedEventArgs e)
        {
            OnChanged(new CellPosChangeReqArgs(CellPosChangeMsgType.POS_RIGHT));
        }

        private void LeftBtn_Click(object sender, RoutedEventArgs e)
        {
            OnChanged(new CellPosChangeReqArgs(CellPosChangeMsgType.POS_LEFT));
        }

        private void BarDisplayToggleClick(object sender, RoutedEventArgs e)
        {
            if (ToolBar.Visibility == Visibility.Visible)
            {
                ToolBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                ToolBar.Visibility = Visibility.Visible;
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete Widget ?", "Delete Widget", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            else
            {
                OnChanged(new CellPosChangeReqArgs(CellPosChangeMsgType.POS_DELETE));
            }
        }

        public IWidgetContainerState GenerateState()
        {
            WidgetFrameState containerState = new WidgetFrameState();
            containerState.Dimension = Dimension;
            containerState.Position = Position;
            containerState.WidgetAppearance = WidgetAppearance;
            containerState.WidgetContainerAutoFetchState_ = AutoFetchState_;
            // Generate WidgetState also
            containerState.WidgetState = mWidget.GenerateState();
            return containerState;
        }

        public void SetState(IWidgetContainerState state)
        {
            if (state is WidgetFrameState frameState)
            {
                Dimension = frameState.Dimension;
                Position = frameState.Position;
                WidgetAppearance = frameState.WidgetAppearance;
                AutoFetchState_ = frameState.WidgetContainerAutoFetchState_;
                // Set the Widget
                IWidget widget;
                if (frameState.WidgetState is BlankWidgetState)
                {
                    widget = new BlankWidget();
                }
                else if (frameState.WidgetState is OxyPlotWidgetState)
                {
                    widget = new PlotWidget();
                }
                else
                {
                    widget = new BlankWidget();
                }

                SetWidget(widget);
                // Set Widget State also
                mWidget.SetState(frameState.WidgetState);
                UpdateAutoFetchState(frameState.WidgetContainerAutoFetchState_);
            }
            else
            {
                Console.WriteLine("Inflation rejected since non WidgetFrameState given for inflation...");
            }
        }

        private void FetchSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            WidgetContainerAutoFetchState state = AutoFetchManager_.OpenWidgetContainerAutoFetchConfigWindow(AutoFetchState_);
            if (state != null)
            {
                UpdateAutoFetchState(state);
            }
        }
    }
}
