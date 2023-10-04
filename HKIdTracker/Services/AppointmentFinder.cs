using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HKIdTracker.Services
{
    public class AppointmentFinder : IAppointmentFinder
    {
        private const string APPOINTMENTURL = "https://eservices.es2.immd.gov.hk/surgecontrolgate/ticket/getSituation";
        private const string AVAILABLELIMITED = "quota-g";
        private const string AVAILABLEFULL = "quota-y";

        public async Task<string?> GetAvailableAppointments(List<DateTime> selectedDates)
        {
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var appointments = await httpClient.GetFromJsonAsync(APPOINTMENTURL, typeof(Appointment), CancellationToken.None) as Appointment;

            var availableAppointments = appointments!.AppointmentInfo.Where(x => selectedDates.Contains(x.Date)
                                                                                 && (x.GeneralService == AVAILABLEFULL
                                                                                 || x.GeneralService == AVAILABLELIMITED
                                                                                 || x.ExtendedService == AVAILABLEFULL
                                                                                 || x.ExtendedService == AVAILABLELIMITED)).OrderBy(x => x.Date).ToList();

            var s = new StringBuilder();

            var offices = GetOfficeNames();

            foreach (var appointment in availableAppointments)
            {
                s.AppendLine($"{appointment.Date.ToString("dd-MM")} {offices[appointment.OfficeId]}");
            }

            return s.Length == 0 ? null : s.ToString();
        }

        public Dictionary<string, string> GetOfficeNames()
        {
            return new Dictionary<string, string>
            {
                { "RHK", "Wan Chai, Hong Kong" },
                { "RKO", "Cheung Sha Wan, Kowloon" },
                { "RKT", "Kwun Tong, Kowloon" },
                { "FTO", "Fo Tan, New Territories" },
                { "TMO", "Tuen Mun, New Territories" },
                { "YLO", "Yuen Long, New Territories" }
            };
        }
    }
}
