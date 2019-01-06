using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;
using PMUDataLayer;
using PMUDataLayer.Config;

namespace Dashboard.Measurements.PMUMeasurement
{
    /// <summary>
    /// Interaction logic for PMUMeasPickerWindow.xaml
    /// </summary>
    public partial class PMUMeasPickerWindow : Window
    {
        // https://stackoverflow.com/questions/12052565/binding-xdocument-to-treeview-attributes-dont-show-up
        // lazy loading - https://www.wpf-tutorial.com/treeview-control/lazy-loading-treeview-items/
        public PMUMeasPickerWindow()
        {
            InitializeComponent();
            //PopulatePMUMeasurements();
            string path = Environment.CurrentDirectory;
            string configFilename = "meas.xml";
            xmlPath = path + "\\" + configFilename;
            PopulateFromStoredXml();
        }

        private void PopulateFromStoredXml()
        {
            if (File.Exists(xmlPath))
            {
                try
                {
                    measXml = XDocument.Load(xmlPath);
                    SetTreeViewElements(measXml);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error loading measurements xml file. {e.Message}");
                }
            }
        }

        private XDocument measXml;
        private string xmlPath;

        private void PopulatePMUMeasurements()
        {
            // get the label objects from psp data layer
            ConfigurationManagerJSON configManager = new ConfigurationManagerJSON();
            configManager.Initialize();
            HistoryDataAdapter adapter = new HistoryDataAdapter(configManager);
            measXml = adapter.GetMeasXml();
            // Bind the tree view with xml
            SetTreeViewElements(measXml);
            SaveMeasXml();
        }

        private void SaveMeasXml()
        {
            try
            {
                measXml.Save(xmlPath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving measurements xml file. {e.Message}");
            }
        }

        private void SetTreeViewElements(XDocument measXml)
        {
            MeasTree.Items.Clear();
            MeasTree.Items.Add(CreateTreeItem(measXml.Root));
        }

        private TreeViewItem CreateTreeItem(object o)
        {
            TreeViewItem item = new TreeViewItem();
            if (o is XElement xEl)
            {
                item.Tag = o;
                item.Header = xEl.Name.LocalName;
                item.Items.Add("Loading...");
            }
            return item;
        }

        private void SelectMeasId(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem item)
            {
            }
        }

        public void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            if ((item.Items.Count == 1) && (item.Items[0] is string))
            {
                item.Items.Clear();
                try
                {
                    if (item.Tag is XElement xEl)
                    {
                        // if tag is has elements then create tree item
                        if (xEl.HasElements == true)
                        {
                            foreach (var xElItem in xEl.Elements())
                            {
                                item.Items.Add(CreateTreeItem(xElItem));
                            }
                        }
                        else
                        {
                            item.Items.Add(new TreeViewItem { Header = xEl.Value.ToString() });
                        }
                    }
                }
                catch { }
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            PopulatePMUMeasurements();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            string testXml = "<guestbook><guest><fname>Terje</fname><lname>Beck</lname></guest><guest><fname>Jan</fname><lname>Refsnes</lname></guest><guest><fname>Torleif</fname><lname>Rasmussen</lname></guest><guest><fname>anton</fname><lname>chek</lname></guest><guest><fname>stale</fname><lname>refsnes</lname></guest><guest><fname>hari</fname><lname>prawin</lname></guest><guest><fname>Hege</fname><lname>Refsnes</lname></guest></guestbook>";
            measXml = XDocument.Parse(testXml);
            SetTreeViewElements(measXml);
            SaveMeasXml();
        }
    }
}
