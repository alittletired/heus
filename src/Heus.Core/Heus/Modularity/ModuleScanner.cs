using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Heus.Modularity
{
    internal class ModuleScanner
    {
        private readonly IModuleContainer _moduleContainer;

        public ModuleScanner(IModuleContainer moduleContainer)
        {
            _moduleContainer = moduleContainer;
         
        }

        public IEnumerable<Type> Scan()
        {
            var assemblies= _moduleContainer.Modules.Select(m => m.Type.Assembly).Distinct();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    yield return type;
                }
            }
        }
    }
    
}