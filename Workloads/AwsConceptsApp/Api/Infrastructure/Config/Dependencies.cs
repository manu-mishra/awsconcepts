using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Config
{
    internal static class Dependencies
    {
        internal static IServiceCollection WithDomainEntityConfiguration(this IServiceCollection services)
        {
            services.AddSingleton(EntityConfigLookUp.GetConfigMap());
            return services;
        }
    }
}
