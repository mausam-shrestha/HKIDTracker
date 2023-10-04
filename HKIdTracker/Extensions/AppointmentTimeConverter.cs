using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HKIdTracker.Extensions
{
    public class AppointmentTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var lastUpdateRaw = reader.GetString()!;

            return DateTime.ParseExact(lastUpdateRaw, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(CultureInfo.GetCultureInfo("zh-HK")));
        }
    }
}
