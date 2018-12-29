using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PspDataLayer.Config
{
    public class ConfigurationManagerJSON
    {
        public string Host { get; set; } = "10.2.100.56";
        public int Port { get; set; } = 7001;
        public string Path { get; set; } = "api/psp";
        public string LabelsPath { get; set; } = "api/psp/labels";
        public string configFilename = "pspApiConfig.json";

        public void Initialize()
        {
            string path = Environment.CurrentDirectory;
            string jsonPath = path + "\\" + configFilename;

            if (File.Exists(jsonPath))
            {
                ConfigurationManagerJSON pmuHistConfig = JsonConvert.DeserializeObject<ConfigurationManagerJSON>(File.ReadAllText(jsonPath));
                CloneFromObject(pmuHistConfig);
            }
            else
            {
                Save();
            }
        }

        public void Save()
        {
            string ConfigJSONStr = JsonConvert.SerializeObject(this, Formatting.Indented);

            string path = Environment.CurrentDirectory;
            string jsonPath = path + "\\" + configFilename;

            File.WriteAllText(jsonPath, ConfigJSONStr);
        }

        public void CloneFromObject(ConfigurationManagerJSON configuration)
        {
            Host = configuration.Host;
            Port = configuration.Port;
            Path = configuration.Path;
            LabelsPath = configuration.LabelsPath;
        }
    }
}
