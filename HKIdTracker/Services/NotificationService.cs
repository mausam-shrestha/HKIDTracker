using ntfy;
using ntfy.Actions;
using ntfy.Requests;

namespace HKIdTracker.Services
{
    public class NotificationService : INotificationService
    {
        public async Task Broadcast(string message)
        {
            // Create a new client
            var client = new Client("https://ntfy.sh");

            // Publish a message to the "test" topic
            var ntfyMessage = new SendingMessage
            {
                Title = "Appointments",
                Actions = new ntfy.Actions.Action[]
                {
                    new View("Book", new Uri("https://www.gov.hk/en/apps/immdicbooking2.htm"))
                },
                Message = message
            };

            await client.Publish("mausam_test_1", ntfyMessage);
        }
    }
}
