using Dashboard.Interfaces;
using Dashboard.Measurements.PMUMeasurement;
using Dashboard.Measurements.RandomMeasurement;
using Dashboard.Measurements.RandomTimeSeriesMeasurement;
using Dashboard.Measurements.ScadaMeasurement;
using Dashboard.States;
using Dashboard.Widgets;
using Dashboard.Widgets.Oxyplot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Dashboard.JsonConverters
{
    public class MeasurementConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IMeasurement);
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
            var jsonObject = JObject.Load(reader);
            var measurement = default(IMeasurement);

            string objectTypeName = jsonObject["TypeName"].Value<string>();
            if (objectTypeName == typeof(RandomMeasurement).Name)
            {
                measurement = new RandomMeasurement();
            }
            else if (objectTypeName == typeof(RandomTimeSeriesMeasurement).Name)
            {
                measurement = new RandomTimeSeriesMeasurement();
            }
            else if (objectTypeName == typeof(PMUMeasurement).Name)
            {
                measurement = new PMUMeasurement();
            }
            else if (objectTypeName == typeof(ScadaMeasurement).Name)
            {
                measurement = new ScadaMeasurement();
            }

            if (measurement != null)
            {
                serializer.Populate(jsonObject.CreateReader(), measurement);
            }

            return measurement;
        }
    }
}