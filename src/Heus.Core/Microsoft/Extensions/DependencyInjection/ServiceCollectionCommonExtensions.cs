using System;
using System.Linq;
using System.Reflection;
using Heus;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
   public static class ServiceCollectionCommonExtensions
    {
        public static bool IsAdded(this IServiceCollection services, Type type)
        {
            return services.Any(d => d.ServiceType == type);
        }

        public static Type GetImplementationType(this IServiceCollection services, Type type)
        {
            return services.First(d => d.ServiceType == type).ImplementationType!;
        }
        public static T? GetSingletonInstanceOrNull<T>(this IServiceCollection services) 
        {
            return (T?)services
                .FirstOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }

        public static T GetSingletonInstance<T>(this IServiceCollection services)
        {
            var service = services.GetSingletonInstanceOrNull<T>();
            if (service == null)
            {
                throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);
            }

            return service;
        }
    }
}