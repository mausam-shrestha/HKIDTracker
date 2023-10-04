using System.Text.Json.Serialization;
using HKIdTracker.Extensions;

namespace HKIdTracker
{
    internal record Appointment
    {
        [JsonPropertyName("data")]
        public List<AppointmentInfo> AppointmentInfo { get; init; } = null!;

        [JsonPropertyName("lastUpdateTime")]
        [JsonConverter(typeof(LastUpdateTimeConverter))]
        public DateTime LastUpdateTime { get; init; }
    }
}
