using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using HKIdTracker.Services;
using Xunit;

namespace HKIdTracker.Tests;

public class FunctionTest
{
    public FunctionTest()
    {
    }

    [Fact]
    public void TestGetMethod()
    {
        IAppointmentFinder appointmentFinder = new AppointmentFinder();
        INotificationService notificationService = new NotificationService();

        var context = new TestLambdaContext();
        var functions = new Functions(appointmentFinder, notificationService);

        var response = functions.Get(context).Result;

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var serializationOptions = new HttpResultSerializationOptions { Format = HttpResultSerializationOptions.ProtocolFormat.RestApi };
        var apiGatewayResponse = new StreamReader(response.Serialize(serializationOptions)).ReadToEnd();
        Assert.True(!string.IsNullOrEmpty(apiGatewayResponse));
    }
}
