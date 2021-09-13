using System.Collections.Generic;
using System.Reflection;
using Heus.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionRegistrationExtensions
    {
        public static IServiceCollection AddServiceRegistrar<T>(this IServiceCollection services,IServiceRegistrar serviceRegistrar)
            where T : class, IServiceRegistrar
        {
            GetOrCreateRegistrarList(services).Add(serviceRegistrar);
            return services;
        }
        public static List<IServiceRegistrar> GetServiceRegistrars(this IServiceCollection services)
        {
            return GetOrCreateRegistrarList(services);
        }
        private static ServiceRegistrarList GetOrCreateRegistrarList(IServiceCollection services)
        {
            var serviceRegistrars = services.GetSingletonInstanceOrNull<ServiceRegistrarList>();
            if (serviceRegistrars != null) return serviceRegistrars;
            serviceRegistrars = new ServiceRegistrarList { new DefaultServiceRegistrar() };
            services.AddSingleton(serviceRegistrars);
            return serviceRegistrars;
        }
    
    }
}