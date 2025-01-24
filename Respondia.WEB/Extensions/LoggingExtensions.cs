using Serilog;

namespace Respondia.WEB.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring Serilog logging.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Configures Serilog logging for the specified <see cref="IHostBuilder"/> using the provided <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to configure.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> to use for configuring Serilog.</param>
        /// <returns>The configured <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            hostBuilder.UseSerilog();
            return hostBuilder;
        }
    }
}
