using System.Text.Json;
using System.Text.Json.Serialization;

namespace QPay.API.Extensions
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _format;

        public DateTimeConverter(string format = "dd-MM-yyyy HH:mm")
        {
            _format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (DateTime.TryParse(reader.GetString(), out var date))
            {
                return date;
            }

            // Fallback if exact format is needed
            return DateTime.ParseExact(reader.GetString()!, _format, null);
            //return DateTime.ParseExact(reader.GetString()!, _format, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }


}

