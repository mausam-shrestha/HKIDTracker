using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HKIdTracker.Extensions
{
    public class LastUpdateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var lastUpdateRaw = reader.GetString()!;

            return DateTime.Parse(lastUpdateRaw, CultureInfo.GetCultureInfo("zh-HK"));
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(CultureInfo.GetCultureInfo("zh-HK")));
        }
    }
}
