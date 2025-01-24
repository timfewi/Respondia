using Microsoft.Extensions.Caching.Hybrid;

namespace Respondia.WEB.Extensions
{
    public static class CachingExtensions
    {
        /// <summary>
        /// Registers the caching services with configured options.
        /// </summary>
        /// <param name="services">The service collection to add the caching services to.</param>
        /// <returns>The service collection with caching services added.</returns>
#pragma warning restore EXTEXP0018
#pragma warning disable EXTEXP0018
        public static IServiceCollection AddCachingServices(this IServiceCollection services)
        {
            services.AddHybridCache(options =>
            {
                options.MaximumPayloadBytes = 1024 * 1024; // 1 MB
                options.MaximumKeyLength = 1024;
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(5),
                    LocalCacheExpiration = TimeSpan.FromMinutes(5)
                };
            });

            return services;
        }
    }
}
