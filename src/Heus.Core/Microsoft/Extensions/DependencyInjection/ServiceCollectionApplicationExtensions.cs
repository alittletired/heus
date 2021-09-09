using System;
using Heus;
using Heus.Modularity;
using JetBrains.Annotations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionApplicationExtensions
    {
        public static IHeusApplication AddApplication(
            this IServiceCollection services,
            Type startupModuleType,
             Action<ApplicationCreationOptions>? optionsAction =null)
            
        {
            return new HeusApplication(services,startupModuleType, optionsAction);
        }
    }
}