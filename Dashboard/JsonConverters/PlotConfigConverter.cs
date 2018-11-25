using Dashboard.Interfaces;
using Dashboard.States;
using Dashboard.Widgets;
using Dashboard.Widgets.Oxyplot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Dashboard.JsonConverters
{
    public class PlotConfigConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IPlotConfig);
        }
        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization.");
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            List<IPlotConfig> fields = new List<IPlotConfig>();
            var jsonArray = JArray.Load(reader);

            foreach (var item in jsonArray)
            {
                var jsonObject = item as JObject;
                var plotConfig = default(IPlotConfig);
                string objectTypeName = jsonObject["TypeName"].Value<string>();
                if (objectTypeName == typeof(LinePlotConfig).Name)
                {
                    plotConfig = new LinePlotConfig();
                }

                serializer.Populate(jsonObject.CreateReader(), plotConfig);
                fields.Add(plotConfig);
            }
            return fields;
        }
    }
}
