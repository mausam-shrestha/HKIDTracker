namespace HKIdTracker.Services
{
    public interface IAppointmentFinder
    {
        public Task<string?> GetAvailableAppointments(List<DateTime> selectedDates);
    }
}
