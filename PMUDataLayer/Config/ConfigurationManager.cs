using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PMUDataLayer.Config
{
    public class ConfigurationManagerJSON
    {
        public int LatencyTime { get; set; } = 1;
        public string Host { get; set; } = "172.16.183.131";
        public int Port { get; set; } = 24721;
        public string Path { get; set; } = "/eterra-ws/HistoricalTrendProvider";
        public string UserName { get; set; } = "pdcAdmin";
        public string Password { get; set; } = "p@ssw0rd";
        public string configFilename = "pmuHistorianConfig.json";

        public void Initialize()
        {
            string path = Environment.CurrentDirectory;
            string jsonPath = path + "\\"+ configFilename;

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
            UserName = configuration.UserName;
            Password = configuration.Password;
            LatencyTime = configuration.LatencyTime;
        }
    }
}
