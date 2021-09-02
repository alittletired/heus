using System;
using Heus;
using Heus.Modularity;
using JetBrains.Annotations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionApplicationExtensions
    {
        public static IHeusApplication AddApplication<TStartupModule>(
            [NotNull] this IServiceCollection services,
            [CanBeNull] Action<ApplicationCreationOptions> optionsAction = null)
            where TStartupModule : ModuleBase
        {
            return new HeusApplication(typeof(TStartupModule), services, optionsAction);
        }
    }
}