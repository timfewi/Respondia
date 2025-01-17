using Microsoft.Extensions.DependencyInjection;
using Respondia.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services to the <see cref="IServiceCollection"/> based on the <see cref="InjectableAttribute"/> found in the specified assemblies.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="assemblies">The assemblies to scan for types with the <see cref="InjectableAttribute"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> with the added services.</returns>
        public static IServiceCollection AddInjectables(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var typesWithAttributes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes<InjectableAttribute>().Any());

                foreach (var type in typesWithAttributes)
                {
                    var attribute = type.GetCustomAttribute<InjectableAttribute>()!;
                    var interfaces = type.GetInterfaces();

                    foreach (var @interface in interfaces)
                    {
                        if (@interface.IsGenericType && @interface.ContainsGenericParameters)
                        {
                            services.Add(new ServiceDescriptor(
                                @interface.GetGenericTypeDefinition(),
                                type.GetGenericTypeDefinition(),
                                attribute.Lifetime));
                        }
                        else
                        {
                            var addMethod = attribute.Lifetime switch
                            {
                                ServiceLifetime.Scoped => new Action<IServiceCollection, Type, Type>((s, iface, impl) => s.AddScoped(iface, impl)),
                                ServiceLifetime.Singleton => new Action<IServiceCollection, Type, Type>((s, iface, impl) => s.AddSingleton(iface, impl)),
                                ServiceLifetime.Transient => new Action<IServiceCollection, Type, Type>((s, iface, impl) => s.AddTransient(iface, impl)),
                                _ => new Action<IServiceCollection, Type, Type>((s, iface, impl) => s.AddTransient(iface, impl))
                            };

                            addMethod(services, @interface, type);
                        }
                    }

                    if (!interfaces.Any())
                    {
                        var addMethod = attribute.Lifetime switch
                        {
                            ServiceLifetime.Scoped => new Action<IServiceCollection, Type>((s, t) => s.AddScoped(t)),
                            ServiceLifetime.Singleton => new Action<IServiceCollection, Type>((s, t) => s.AddSingleton(t)),
                            ServiceLifetime.Transient => new Action<IServiceCollection, Type>((s, t) => s.AddTransient(t)),
                            _ => new Action<IServiceCollection, Type>((s, t) => s.AddTransient(t))
                        };

                        addMethod(services, type);
                    }
                }
            }

            return services;
        }
    }
}
