using Microsoft.Extensions.Options;
using Respondia.BL.Services.EmailServices;
using Respondia.Core.Utils;

namespace Respondia.WEB.Extensions
{
    /// <summary>
    /// Extension methods for setting up email services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class EmailServicesExtensions
    {
        /// <summary>
        /// Adds email services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> to configure the services.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<EmailSenderService>();
            services.AddSingleton<EmailQueueService>();
            services.AddHostedService<EmailProcessorService>();
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<SmtpSettings>>().Value);

            return services;
        }
    }
}
