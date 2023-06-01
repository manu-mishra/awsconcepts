using Infrastructure.Config;
using Infrastructure.Repository;
using Infrastructure.Search;
using Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Dependencies
    {
        public static IServiceCollection WithInfrastructureDependencies(this IServiceCollection services, IConfiguration configManager)
        {

            services
                .WithDomainEntityConfiguration()
                .WithDynamoDbEntityStorageDependencies()
                .WithS3DbEntityStorageDependencies()
                .WithEntitySearchDependencies(configManager);
            return services;
        }
    }
}
