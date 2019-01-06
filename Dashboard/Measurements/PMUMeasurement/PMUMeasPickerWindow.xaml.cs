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
            string path = Environment.CurrentDirectory;
            string configFilename = "meas.xml";
            xmlPath = path + "\\" + configFilename;
            PopulateFromStoredXml();
        }

        private XDocument measXml;
        private string xmlPath;
        private List<PmuXmlMeasurement> XmlMeasurements_ = new List<PmuXmlMeasurement>();
        public PmuXmlMeasurement SelectedMeas_ { get; set; }

        private void PopulateFromStoredXml()
        {
            if (File.Exists(xmlPath))
            {
                try
                {
                    measXml = XDocument.Load(xmlPath);
                    //SetTreeViewElements(measXml);
                    SetMeasDataTable();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error loading measurements xml file. {e.Message}");
                }
            }
        }

        private void FetchAndPopulatePMUMeasurements()
        {
            // get the label objects from psp data layer
            ConfigurationManagerJSON configManager = new ConfigurationManagerJSON();
            configManager.Initialize();
            HistoryDataAdapter adapter = new HistoryDataAdapter(configManager);
            measXml = adapter.GetMeasXml();
            // Bind the tree view with xml
            //SetTreeViewElements(measXml);
            SetMeasDataTable();
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
            //MeasTree.Items.Clear();
            //MeasTree.Items.Add(CreateTreeItem(measXml.Root));
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

        private void SetMeasDataTable()
        {
            // Traverse the Xml Doc to get the device elements
            CreateMeasTableList(measXml.Root);
        }

        private void CreateMeasTableList(XElement xEl)
        {
            foreach (XElement xElItem in measXml.Descendants())
            {
                if (xElItem.Name.LocalName == "device")
                {
                    CreateMeasDataFromDevice(xElItem);
                }
            }
            //https://stackoverflow.com/questions/8911026/multicolumn-listbox-in-wpf
            MeasListView.ItemsSource = XmlMeasurements_;
        }

        private void CreateMeasDataFromDevice(XElement xEl)
        {
            string volt = xEl.Element("voltageLevel").Value.ToString();
            foreach (XElement measXmlEl in xEl.Element("measurements").Elements())
            {
                try
                {
                    if (measXmlEl.Element("measurementSource").Attributes().First().Value == "ns2:DoubleDigitalMeasurementSource")
                    {
                        continue;
                    }
                    PmuXmlMeasurement meas = new PmuXmlMeasurement();
                    meas.DevVolt = volt;
                    meas.MeasId = int.Parse(measXmlEl.Element("measurementID").Value.ToString());
                    meas.ScadaStationName = measXmlEl.Element("scadaId").Element("stationName").Value.ToString();
                    meas.DevType = measXmlEl.Element("scadaId").Element("deviceType").Value.ToString();
                    meas.ScadaDevName = measXmlEl.Element("scadaId").Element("deviceName").Value.ToString();
                    meas.ScadaPntName = measXmlEl.Element("scadaId").Element("pointName").Value.ToString();
                    meas.PmuId = int.Parse(measXmlEl.Element("measurementSource").Element("pmuId").Value.ToString());
                    meas.PmuStationName = measXmlEl.Element("measurementSource").Element("stationName").Value.ToString();
                    XmlMeasurements_.Add(meas.Clone());
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error reading measurement xml element. {e.Message}");
                }
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            FetchAndPopulatePMUMeasurements();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = MeasListView.SelectedIndex;
            if (selectedIndex > -1)
            {
                SelectedMeas_ = (PmuXmlMeasurement)MeasListView.SelectedItems[0];
            }
            DialogResult = true;
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            string testXml = "<guestbook><guest><fname>Terje</fname><lname>Beck</lname></guest><guest><fname>Jan</fname><lname>Refsnes</lname></guest><guest><fname>Torleif</fname><lname>Rasmussen</lname></guest><guest><fname>anton</fname><lname>chek</lname></guest><guest><fname>stale</fname><lname>refsnes</lname></guest><guest><fname>hari</fname><lname>prawin</lname></guest><guest><fname>Hege</fname><lname>Refsnes</lname></guest></guestbook>";
            measXml = XDocument.Parse(testXml);
            SetTreeViewElements(measXml);
            SaveMeasXml();
        }

        private void FilterTxt_Changed(object sender, RoutedEventArgs e)
        {
            List<PmuXmlMeasurement> xmlMeasurements = XmlMeasurements_;
            if (!string.IsNullOrEmpty(StationFilter.Text))
            {
                xmlMeasurements = xmlMeasurements.Where(item => item.ScadaStationName.StartsWith(StationFilter.Text)).ToList();
            }
            if (!string.IsNullOrEmpty(DevTypeFilter.Text))
            {
                xmlMeasurements = xmlMeasurements.Where(item => item.DevType.StartsWith(DevTypeFilter.Text)).ToList();
            }
            if (!string.IsNullOrEmpty(PntNameFilter.Text))
            {
                xmlMeasurements = xmlMeasurements.Where(item => item.ScadaPntName.StartsWith(PntNameFilter.Text)).ToList();
            }
            if (!string.IsNullOrEmpty(VoltFilter.Text))
            {
                xmlMeasurements = xmlMeasurements.Where(item => item.DevVolt.StartsWith(VoltFilter.Text)).ToList();
            }
            MeasListView.ItemsSource = xmlMeasurements;
        }
    }

    public class PmuXmlMeasurement
    {
        public int PmuId { get; set; }
        public string DevVolt { get; set; }
        public int MeasId { get; set; }
        public string ScadaStationName { get; set; }
        public string DevType { get; set; }
        public string ScadaDevName { get; set; }
        public string ScadaPntName { get; set; }
        public string PmuStationName { get; set; }
        public PmuXmlMeasurement Clone()
        {
            return new PmuXmlMeasurement
            {
                PmuId = PmuId,
                DevVolt = DevVolt,
                MeasId = MeasId,
                ScadaStationName = ScadaStationName,
                DevType = DevType,
                ScadaDevName = ScadaDevName,
                ScadaPntName = ScadaPntName,
                PmuStationName = PmuStationName,
            };
        }
    }
}
