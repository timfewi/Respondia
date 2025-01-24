using FluentValidation;
using FluentValidation.AspNetCore;
using Respondia.BL.Utils;

namespace Respondia.WEB.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring FluentValidation services.
    /// </summary>
    public static class FluentValidationExtensions
    {
        /// <summary>
        /// Adds FluentValidation services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <returns>The IServiceCollection with the FluentValidation services added.</returns>
        public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
        {
            // Adds automatic validation for ASP.NET Core.
            services.AddFluentValidationAutoValidation();

            // Adds client-side adapters for FluentValidation.
            services.AddFluentValidationClientsideAdapters();

            // Registers all validators from the assembly containing the EntryPointBusinessLogic class.
            services.AddValidatorsFromAssemblyContaining<EntryPointBusinessLogic>();

            return services;
        }
    }
}
