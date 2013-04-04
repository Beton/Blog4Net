using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Blog4Net.Web.Utilities
{
    public class CustomDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Overriden Write - Read TODO");
        }
    }
}