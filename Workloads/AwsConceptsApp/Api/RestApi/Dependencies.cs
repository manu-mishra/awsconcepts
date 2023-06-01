using Application;
using Application.Identity;
using Infrastructure;
using RestApi.Services;
using System.Reflection;

namespace RestApi
{
    public static class Dependencies
    {
        public static IMvcBuilder WithApplicationDomainControllers(this IMvcBuilder builder)
        {
            return builder.AddApplicationPart(Assembly.GetExecutingAssembly());
        }

        public static IServiceCollection WithApiControllerServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddApplicationDependencies()
                .WithInfrastructureDependencies(configuration)
                .AddHttpContextAccessor()
                .AddScoped<IIdentity, CurrentIdentityService>();
            return services;
        }
    }
}
