using Dashboard.Interfaces;
using Dashboard.States;
using Dashboard.Widgets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Dashboard.JsonConverters
{
    public class WidgetContainerStateConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IWidgetContainerState);
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
            List<IWidgetContainerState> fields = new List<IWidgetContainerState>();
            var jsonArray = JArray.Load(reader);
            
            foreach (var item in jsonArray)
            {
                var jsonObject = item as JObject;
                var WidgetContainerState = default(IWidgetContainerState);
                string objectTypeName = jsonObject["TypeName"].Value<string>();
                if (objectTypeName == typeof(WidgetFrameState).Name)
                {
                    WidgetContainerState = new WidgetFrameState();
                }
                serializer.Populate(jsonObject.CreateReader(), WidgetContainerState);
                fields.Add(WidgetContainerState);
            }
            return fields;
        }
    }
}
