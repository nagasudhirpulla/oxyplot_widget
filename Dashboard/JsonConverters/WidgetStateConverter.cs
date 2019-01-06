using Dashboard.Interfaces;
using Dashboard.States;
using Dashboard.Widgets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Dashboard.JsonConverters
{
    public class WidgetStateConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IWidgetState);
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
            var WidgetState = default(IWidgetState);
            string objectTypeName = jsonObject["TypeName"].Value<string>();
            if (objectTypeName == typeof(BlankWidgetState).Name)
            {
                WidgetState = new BlankWidgetState();
            }
            else if (objectTypeName == typeof(OxyPlotWidgetState).Name)
            {
                WidgetState = new OxyPlotWidgetState();
            }
            else if (objectTypeName == typeof(DataExportWidgetState).Name)
            {
                WidgetState = new DataExportWidgetState();
            }

            if (WidgetState!=null)
            {
                serializer.Populate(jsonObject.CreateReader(), WidgetState);
            }

            return WidgetState;
        }
    }
}
