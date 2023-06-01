using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Newtonsoft.Json.Linq;
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Contrib.Instrumentation.AWSLambda.Implementation;
using OpenTelemetry.Trace;
using System.Text.Json;

namespace LambdaApi;

public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayHttpApiV2ProxyFunction
{
    private IServiceProvider services;
#pragma warning disable CS8601 // Possible null reference assignment.
    TracerProvider tracerProvider = Sdk.CreateTracerProviderBuilder()
        .AddXRayTraceId()
        .AddAWSLambdaConfigurations()
        .AddAspNetCoreInstrumentation()
        .AddAWSInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter()
        .Build();
#pragma warning restore CS8601 // Possible null reference assignment.
    static LambdaEntryPoint()
    {
        Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());
    }
    protected override void Init(IWebHostBuilder builder)
    {
        builder
        .UseStartup<Startup>();
    }
    protected override void PostCreateHost(IHost webHost)
    {
        base.PostCreateHost(webHost);
        services = webHost.Services;
    }

    [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
    public async Task<APIGatewayHttpApiV2ProxyResponse> TracingFunctionHandlerAsync(APIGatewayHttpApiV2ProxyRequest input, ILambdaContext context)
        => await AWSLambdaWrapper.Trace(tracerProvider, FunctionHandlerAsync, input, context);

    [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
    public async Task<JsonElement> PreTokenGenerationHandler(JsonElement request, ILambdaContext context)
    {
        
        
        string requestString = request.ToString();

        // Modify the JSON string
        JObject requestJObject = JObject.Parse(requestString);
        JObject response = new JObject();
        JObject claimsOverrideDetails = new JObject();
        JObject claimsToAddOrOverride = new JObject();
        string email = (string)requestJObject["request"]["userAttributes"]["email"];

        // Use the email claim as the value for "app_claims"
       // claimsToAddOrOverride["app_claims"] = await getUserClaims(email);
        claimsOverrideDetails["claimsToAddOrOverride"] = claimsToAddOrOverride;
        response["claimsOverrideDetails"] = claimsOverrideDetails;
        requestJObject["response"] = response;

        // Convert the modified JSON string back to JsonElement
        requestString = requestJObject.ToString();
        request = JsonSerializer.Deserialize<JsonElement>(requestString);

        return request;
    }
    //private async Task<string> getUserClaims(string email)
    //{
    //    var mediator = services.GetService<IMediator>();
    //    if (mediator is null)
    //        throw new Exception("miss configuration");
    //    var userClaims = await mediator.Send(new GetUserAccessQuery(email));
    //    var jsonResponse = JsonSerializer.Serialize(userClaims, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    //    ;
    //    return Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(jsonResponse));
    //}
   
}

