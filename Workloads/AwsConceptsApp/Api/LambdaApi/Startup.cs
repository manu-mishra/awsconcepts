using Microsoft.AspNetCore.Authentication.JwtBearer;
using RestApi;
using System.Diagnostics;

namespace LambdaApi;

public class Startup
{
    string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    public Startup(IHostEnvironment hostEnvironment)
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(hostEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
    }
    //public Startup(IConfiguration configuration)
    //{
    //    Configuration = configuration;
    //}

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        //AWSSDKHandler.RegisterXRayForAllServices();
        services.AddControllers().WithApplicationDomainControllers();
        services.WithApiControllerServiceDependencies(Configuration)
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = options.DefaultScheme = options.DefaultChallengeScheme =
    JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    //x.MetadataAddress = "https://cognito-idp.us-east-1.amazonaws.com/us-east-1_QBoIUnnt6/.well-known/openid-configuration";
    //var audience = Environment.GetEnvironmentVariable("oAuthAudiance");
    //var authority = Environment.GetEnvironmentVariable("oAuthAuthority");

    //if (string.IsNullOrEmpty(audience))
    //    audience = Configuration.GetValue<string>("oAuthAudiance");
    //if (string.IsNullOrEmpty(authority))
    //    authority = Configuration.GetValue<string>("oAuthAuthority");


    x.Audience = Configuration.GetValue<string>("oAuthAudiance");
    x.Authority = Configuration.GetValue<string>("oAuthAuthority");
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = false
    };
    x.IncludeErrorDetails = true;
});
        services.AddAuthorization();
        //services.AddScoped<IApplicationLogger, XrayInstrumentation>();
        
        services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:3000", 
                        "https://awsconcepts.com", 
                        "https://www.awsconcepts.com",
                        "https://api.awsconcepts.com")
                    .AllowAnyHeader()                          
                    .AllowAnyMethod();
                });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseHttpsRedirection();
        app.UseCors(MyAllowSpecificOrigins);
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers().RequireAuthorization();
            endpoints.MapGet("/", async context =>
            {
                string? traceId = Activity.Current?.TraceId.ToHexString();
                string version = "1";
                string? epoch = traceId?.Substring(0, 8);
                string? random = traceId?.Substring(8);
                string response = "{" + "\"traceId\"" + ": " + "\"" + version + "-" + epoch + "-" + random + "\"" + "}";
                Console.WriteLine(response);
                foreach (KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> item in context.Request.Headers)
                {
                    Console.WriteLine(item);
                }
                foreach (KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> item in context.Response.Headers)
                {
                    Console.WriteLine(item);
                }
                await context.Response.WriteAsync(response);
            });
            endpoints.MapGet("/api", async context =>
            {
                string? traceId = Activity.Current?.TraceId.ToHexString();
                string version = "1";
                string? epoch = traceId?.Substring(0, 8);
                string? random = traceId?.Substring(8);
                string response = "{" + "\"traceId\"" + ": " + "\"" + version + "-" + epoch + "-" + random + "\"" + "}";
                Console.WriteLine(response);
                foreach (KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> item in context.Request.Headers)
                {
                    response= response +$"{item.Key} and value = {item.Value}" + Environment.NewLine;
                    Console.WriteLine(item);
                }
                await context.Response.WriteAsync(response);
            });
        });
    }
}