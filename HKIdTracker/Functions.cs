using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using HKIdTracker.Services;
using System.Collections;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HKIdTracker;

public class Functions
{
    private readonly IAppointmentFinder _appointmentFinder;
    private readonly INotificationService _notificationService;

    /// <summary>
    /// Default constructor that Lambda will invoke.
    /// </summary>
    public Functions(IAppointmentFinder appointmentFinder, INotificationService notificationService)
    {
        _appointmentFinder = appointmentFinder;
        _notificationService = notificationService;
    }


    /// <summary>
    /// A Lambda function to respond to HTTP Get methods from API Gateway
    /// </summary>
    /// <remarks>
    /// This uses the <see href="https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.Annotations/README.md">Lambda Annotations</see> 
    /// programming model to bridge the gap between the Lambda programming model and a more idiomatic .NET model.
    /// 
    /// This automatically handles reading parameters from an APIGatewayProxyRequest
    /// as well as syncing the function definitions to serverless.template each time you build.
    /// 
    /// If you do not wish to use this model and need to manipulate the API Gateway 
    /// objects directly, see the accompanying Readme.md for instructions.
    /// </remarks>
    /// <param name="context">Information about the invocation, function, and execution environment</param>
    /// <returns>The response as an implicit <see cref="APIGatewayProxyResponse"/></returns>
    [LambdaFunction(Policies = "AWSLambdaBasicExecutionRole", MemorySize = 256, Timeout = 30)]
    [RestApi(LambdaHttpMethod.Get, "/")]
    public async Task<IHttpResult> Get(ILambdaContext context)
    {
        context.Logger.LogInformation("Handling the 'Get' Request");

        var initialDates = new List<DateTime>();

        var octStartDate = new DateTime(2023, 10, 19);

        for (int i = 1; i < 12; i++)
        {
            initialDates.Add(octStartDate.AddDays(i));
        }

        var decemberDates = new List<DateTime>();

        var decStartDate = new DateTime(2023, 12, 16);

        for (int i = 1; i < 24; i++)
        {
            decemberDates.Add(decStartDate.AddDays(i));
        }

        initialDates.AddRange(decemberDates);

        var appointmentInfo = await _appointmentFinder.GetAvailableAppointments(initialDates);

        if (!string.IsNullOrEmpty(appointmentInfo))
        {
            await _notificationService.Broadcast(appointmentInfo);

            return HttpResults.Ok(appointmentInfo);
        }
        else
        {
            return HttpResults.Ok($"Appointment not found for these days > { string.Join(",", initialDates) }.");
        }
    }
}
