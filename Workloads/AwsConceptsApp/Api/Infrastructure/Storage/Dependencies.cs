using Amazon.S3;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Storage
{
    internal static class Dependencies
    {
        internal static IServiceCollection WithS3DbEntityStorageDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client());
            services.AddScoped(typeof(IFileStorageRepository), typeof(S3StorageRepository));
            return services;
        }
    }
}
