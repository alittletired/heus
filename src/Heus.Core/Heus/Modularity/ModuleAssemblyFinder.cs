using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Heus.Modularity
{
    internal class ModuleAssemblyFinder 
    {
        private readonly IModuleContainer _moduleContainer;

        private readonly Lazy<IReadOnlyList<Assembly>> _assemblies;

        public ModuleAssemblyFinder(IModuleContainer moduleContainer)
        {
            _moduleContainer = moduleContainer;

            _assemblies = new Lazy<IReadOnlyList<Assembly>>(FindAll, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IReadOnlyList<Assembly> Assemblies => _assemblies.Value;

        public IReadOnlyList<Assembly> FindAll()
        {
            var assemblies = new List<Assembly>();

            foreach (var module in _moduleContainer.Modules)
            {
                assemblies.Add(module.Type.Assembly);
            }

            return assemblies.Distinct().ToImmutableList();
        }
    }
}