namespace HKIdTracker.Services
{
    public interface INotificationService
    {
        public Task Broadcast(string message);
    }
}
