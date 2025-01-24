using Microsoft.EntityFrameworkCore;
using Respondia.DataAccess.Data;

namespace Respondia.WEB.Extensions
{
    /// <summary>
    /// Extension methods for setting up the application's DbContext.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Adds the application's DbContext to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the DbContext to.</param>
        /// <param name="configuration">The configuration to use for retrieving the connection string.</param>
        /// <returns>The updated service collection.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the connection string 'DefaultConnection' is not found.</exception>
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
