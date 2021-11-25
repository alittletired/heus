using System;
using Heus;
using Heus.Modularity;
using JetBrains.Annotations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionApplicationExtensions
    {
        public static IApplication AddApplication(this IServiceCollection services, Type? startupModuleType = null)

        {
            var application = new CoreApplication(services,
                startupModuleType ?? services.GetImplementationType(typeof(IModule)));
            application.AddServices();
            return application;
        }
    }
}