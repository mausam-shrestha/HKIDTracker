using System.Text.Json.Serialization;
using HKIdTracker.Extensions;

namespace HKIdTracker
{
    internal class AppointmentInfo
    {
        [JsonPropertyName("date")]
        [JsonConverter(typeof(AppointmentTimeConverter))]
        public DateTime Date { get; init; }

        [JsonPropertyName("officeId")]
        public string OfficeId { get; init; } = string.Empty;

        [JsonPropertyName("quotaK")]
        public string GeneralService { get; init; } = string.Empty;

        [JsonPropertyName("quotaR")]
        public string ExtendedService { get; init; } = string.Empty;
    }
}