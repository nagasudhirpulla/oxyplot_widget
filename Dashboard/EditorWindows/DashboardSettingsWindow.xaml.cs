using Dashboard.States;
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

namespace Dashboard.EditorWindows
{
    /// <summary>
    /// Interaction logic for DashboardSettingsWindow.xaml
    /// </summary>
    public partial class DashboardSettingsWindow : Window
    {
        public DashboardSettingsWindow(DashboardState state)
        {
            InitializeComponent();
            mEditorVM = new DashboardSetingsEditorVM(state);
            DataContext = mEditorVM;
        }

        private DashboardSetingsEditorVM mEditorVM;

        public DashboardState DashboardState { get { return mEditorVM.mDashboardState; } }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Apply Changes ?", "Apply Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            else
            {
                DialogResult = true;
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }

    public class DashboardSetingsEditorVM
    {
        public DashboardState mDashboardState;
        public DashboardSetingsEditorVM(DashboardState state)
        {
            mDashboardState = state.GenerateSettings();
        }
        public string Name { get { return mDashboardState.Name; } set { mDashboardState.Name = value; } }
        public int InitHeight { get { return mDashboardState.InitHeight; } set { mDashboardState.InitHeight = value; } }
        public int InitWidth { get { return mDashboardState.InitWidth; } set { mDashboardState.InitWidth = value; } }
        public bool IsDimensionsLocked { get { return mDashboardState.IsDimensionsLocked; } set { mDashboardState.IsDimensionsLocked = value; } }
    }
}
