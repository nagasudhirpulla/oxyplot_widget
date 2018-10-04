using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace PMUDataLayer.Config
{
    public class ConfigurationManagerXML
    {
        public void Initialize()
        {
            XmlDocument domDocument = new XmlDocument();

            string path = Environment.CurrentDirectory;
            string xmlPath = path + "\\AppSettings.xml";

            if (File.Exists(xmlPath))
            {
                domDocument.Load(xmlPath);
                XmlNode node = domDocument.SelectSingleNode("HistoryDataProvider/DataSource");

                Host = node.Attributes.GetNamedItem("Host").Value;
                string port = node.Attributes.GetNamedItem("Port").Value;
                Port = Int32.Parse(port);
                Path = node.Attributes.GetNamedItem("Path").Value;
                UserName = node.Attributes.GetNamedItem("UserName").Value;
                Password = node.Attributes.GetNamedItem("Password").Value;

                LatencyTime = 1;

                XmlNode latencyNode = domDocument.SelectSingleNode("HistoryDataProvider/LatencyTime");
                if (latencyNode != null)
                {
                    string latencyValue = latencyNode.Value;
                    int latency = 1;
                    if (int.TryParse(latencyValue, out latency))
                        LatencyTime = latency;
                }
            }
            else
            {
                Host = "host";
                Port = 24721;
                Path = "path";
                UserName = "pdcAdmin";
                Password = "pass";
                LatencyTime = 1;
            }
        }

        public int LatencyTime { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Path { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        internal void Save()
        {
            var sw = new StringWriter(new StringBuilder(1024));
            var xmlTextWriter = new XmlTextWriter(sw);
            xmlTextWriter.Formatting = Formatting.Indented;

            xmlTextWriter.WriteStartElement("HistoryDataProvider");
            xmlTextWriter.WriteStartElement("DataSource");

            xmlTextWriter.WriteStartAttribute("Host");
            xmlTextWriter.WriteString(this.Host);
            xmlTextWriter.WriteEndAttribute();

            xmlTextWriter.WriteStartAttribute("Port");
            xmlTextWriter.WriteString(this.Port.ToString());
            xmlTextWriter.WriteEndAttribute();

            xmlTextWriter.WriteStartAttribute("Path");
            xmlTextWriter.WriteString(this.Path);
            xmlTextWriter.WriteEndAttribute();

            xmlTextWriter.WriteStartAttribute("UserName");
            xmlTextWriter.WriteString(this.UserName);
            xmlTextWriter.WriteEndAttribute();

            xmlTextWriter.WriteStartAttribute("Password");
            xmlTextWriter.WriteString(this.Password);
            xmlTextWriter.WriteEndAttribute();

            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("LatencyTime");
            xmlTextWriter.WriteString(LatencyTime.ToString());
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteEndElement();
            xmlTextWriter.Close();

            string path = Environment.CurrentDirectory;
            string xmlPath = path + "\\AppSettings.xml";

            File.WriteAllText(xmlPath, sw.ToString());
        }
    }
}
