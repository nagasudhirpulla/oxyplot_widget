using Dashboard.Interfaces;
using Dashboard.JsonConverters;
using Dashboard.Measurements.RandomMeasurement;
using Newtonsoft.Json;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Widgets.DataExport
{
    public class DataExportConfig
    {
        public List<DataSeriesConfig> SeriesConfigs { get; set; } = new List<DataSeriesConfig>();
        public string TypeName { get; set; } = typeof(DataExportConfig).Name;
        public string Name { get; set; } = "Default";

        public DataExportConfig Clone()
        {
            DataExportConfig dataExportConfig = new DataExportConfig { Name = Name};
            dataExportConfig.SeriesConfigs = (from config in SeriesConfigs select config.Clone()).ToList();
            return dataExportConfig;
        }

        public void OpenConfigEditWindow()
        {
            DataExportConfigEditWindow configEditWindow = new DataExportConfigEditWindow(this);
            configEditWindow.ShowDialog();
            if (configEditWindow.DialogResult == true)
            {
                Name = configEditWindow.editorVM.mDataExportConfig.Name;
                SeriesConfigs = configEditWindow.editorVM.mDataExportConfig.SeriesConfigs;
            }
        }
    }

    public class DataSeriesConfig
    {
        public string Name { get; set; } = "Default";

        [JsonConverter(typeof(MeasurementConverter))]
        public IMeasurement Measurement { get; set; } = new RandomMeasurement();

        public async Task<List<DataPoint>> FetchData(bool applyTimeShift)
        {
            List<DataPoint> dataPoints;
            dataPoints = await Measurement.FetchDataAsync(null);
            return dataPoints;
        }

        public string GetDisplayText()
        {
            return Measurement.GetDisplayText();
        }

        public DataSeriesConfig Clone()
        {
            DataSeriesConfig config = new DataSeriesConfig { Name = Name, Measurement = Measurement.Clone() };
            return config;
        }
    }
}
