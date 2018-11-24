using System;
using System.Collections.Generic;
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
    public partial class VariableTimePicker : UserControl
    {
        public VariableTimePickerVM PickerVM { get; set; }
        public VariableTimePicker()
        {
            InitializeComponent();
            PickerVM = new VariableTimePickerVM(new VariableTime());
            DataContext = PickerVM;
        }
    }
    public class VariableTimePickerVM
    {
        public VariableTime _VariableTime { get; set; }
        public VariableTimePickerVM(VariableTime variableTime)
        {
            _VariableTime = variableTime.Clone();
        }
    }
}
