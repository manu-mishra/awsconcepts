using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenSearch.Client;
using OpenSearch.Net;

namespace Infrastructure.Search
{
    internal static class Dependencies
    {
        public static IServiceCollection WithEntitySearchDependencies(this IServiceCollection services, IConfiguration configManager)
        {
            services.AddSingleton(getSearchClient(configManager));
            services.AddScoped<IEntitySearchProvider, OpenSearchProvider>();
            return services;
        }

        private static IOpenSearchClient getSearchClient(IConfiguration configManager)
        {
            string? userName = Environment.GetEnvironmentVariable("elasticUserName");
            string? password = Environment.GetEnvironmentVariable("elasticPassword");
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
            {
                userName = configManager["elasticUserName"];
                password = configManager["elasticPassword"];
            }
            Uri[] uris = new Uri[] { new Uri("https://search-awsconcepts-f6zgsd3tkq5fi6hododigdm7vm.us-east-1.es.amazonaws.com/") };
            StaticConnectionPool connectionPool = new StaticConnectionPool(uris);
            ConnectionSettings settings = new ConnectionSettings(connectionPool, new TracedConnection())
                .BasicAuthentication(userName, password);

            return new OpenSearchClient(settings);
        }
    }
}
