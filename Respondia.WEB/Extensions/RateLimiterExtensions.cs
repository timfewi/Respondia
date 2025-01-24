using System.Threading.RateLimiting;

namespace Respondia.WEB.Extensions
{
    public static class RateLimiterExtensions
    {
        public static IServiceCollection AddRateLimiterServices(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("Default", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 50
                        }));
            });

            return services;
        }
    }
}
