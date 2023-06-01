using Amazon.Lambda.Core;
using Moq;
using Newtonsoft.Json.Linq;
using OpenSearch.Client;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Text.Json;

namespace LambdaApi.Tests
{
        public class PreTokenGenerationHandlerTests
        {
            
            [Fact]
            public async Task TestPreTokenGenerationHandler()
            {
            SetEnvironmentVariables();
                // Arrange
                var request = JsonSerializer.Deserialize<JsonElement>(@"{
                    ""request"": {
                     ""userAttributes"": {
                      ""email"": ""17mm@outlook.com""
                         }
                    }
                }");
                var context = new Mock<ILambdaContext>();
                var logger = new Mock<ILambdaLogger>();
                context.Setup(x => x.Logger).Returns(logger.Object);

                // Act
                var function = new LambdaEntryPoint();
                var response = await function.PreTokenGenerationHandler(request, context.Object);
            }

        private void SetEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("globalAdmins", "17mm@outlook.com,admin2");
            Environment.SetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME", "Lambdaapi");
            Environment.SetEnvironmentVariable("AWS_REGION", "us-east-1");
            Environment.SetEnvironmentVariable("AWS_LAMBDA_FUNCTION_VERSION", "1");
        }

        [Fact]
            public void TelemetryConfiguration()
            {
                Environment.SetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME", "Lambdaapi");
                Environment.SetEnvironmentVariable("AWS_REGION", "us-east-1");
                Environment.SetEnvironmentVariable("AWS_LAMBDA_FUNCTION_VERSION", "1");
                    var a = Sdk.CreateTracerProviderBuilder();
                    var b = a.AddXRayTraceId();
                    var c = b.AddAWSLambdaConfigurations();
                    var d = c.AddAspNetCoreInstrumentation();
                    var e = d.AddAWSInstrumentation();
                    var f = e.AddHttpClientInstrumentation();
                    var g = f.AddOtlpExporter();
                    g.Build();
            }
            private JsonElement GetJsonElement()
            {
                var requestJson = @"{
        ""version"": ""1"",
        ""triggerSource"": ""TokenGeneration_Authentication"",
        ""region"": ""us-east-1"",
        ""userPoolId"": ""us-east-1_JWQrvxjaV"",
        ""userName"": ""f354a6ce-1173-4837-ba5e-f79a344de0b3"",
        ""callerContext"": {
            ""awsSdkVersion"": ""aws-sdk-unknown-unknown"",
            ""clientId"": ""7oedqjapiud6d2dbat72sop64h""
        },
        ""request"": {
            ""userAttributes"": {
                ""sub"": ""f354a6ce-1173-4837-ba5e-f79a344de0b3"",
                ""cognito:email_alias"": ""17mm@outlook.com"",
                ""cognito:user_status"": ""CONFIRMED"",
                ""email_verified"": ""true"",
                ""nickname"": ""Manu"",
                ""given_name"": ""Manu"",
                ""family_name"": ""Mishra"",
                ""email"": ""17mm@outlook.com""
            },
            ""groupConfiguration"": {
                ""groupsToOverride"": [],
                ""iamRolesToOverride"": [],
                ""preferredRole"": null
            }
        },
        ""response"": {
            ""claimsOverrideDetails"": null
        }
    }";
                return JsonDocument.Parse(requestJson).RootElement;
            }
        }
   
}