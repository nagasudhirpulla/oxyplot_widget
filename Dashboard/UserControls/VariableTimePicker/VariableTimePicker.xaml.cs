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

namespace Dashboard.UserControls.VariableTimePicker
{
    /// <summary>
    /// Interaction logic for VariableTimePicker.xaml
    /// </summary>
    public partial class VariableTimePicker : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty VariableTimeObjProperty = DependencyProperty.Register("VariableTimeObj", typeof(VariableTime),
         typeof(VariableTimePicker), new PropertyMetadata(null));

        public VariableTime VariableTimeObj
        {
            get { return (VariableTime)GetValue(VariableTimeObjProperty); }
            set
            {
                SetValue(VariableTimeObjProperty, value);
                OnPropertyChanged("VariableTimeObj");
            }
        }

        public VariableTimePicker()
        {
            InitializeComponent();
        }

        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
