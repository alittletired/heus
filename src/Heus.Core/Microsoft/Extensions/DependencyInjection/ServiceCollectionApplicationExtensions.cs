using System;
using Heus;
using Heus.Modularity;
using JetBrains.Annotations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionApplicationExtensions
    {
        public static IApplication AddApplication(
            this IServiceCollection services,
            Type startupModuleType,
             Action<ApplicationCreationOptions>? optionsAction =null)

        {
            var application = new CoreApplication(services, startupModuleType);
            application.AddServices();
            return application;
        }
    }
}