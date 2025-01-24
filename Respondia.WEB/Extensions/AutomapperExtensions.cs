using Respondia.BL.Utils;
using Respondia.Core.Utils;

namespace Respondia.WEB.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(EntryPointBusinessLogic).Assembly,
                typeof(EntryPointCore).Assembly
            );

            return services;
        }
    }
}
