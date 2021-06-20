using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Heus.Modularity.PlugIns;

namespace Heus.Modularity
{
    public interface IModuleLoader
    {
        [NotNull]
        IHeusModuleDescriptor[] LoadModules(
            [NotNull] IServiceCollection services,
            [NotNull] Type startupModuleType,
            [NotNull] PlugInSourceList plugInSources
        );
    }
}
