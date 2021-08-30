using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Modularity
{
   internal class ModuleLoader
    {
        private readonly ILogger _logger;
        public ModuleLoader( ILogger logger) { _logger = logger; }
        public ServiceModuleDescriptor[] LoadModules(
            IServiceCollection services,
            Type startupModuleType
          )
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(startupModuleType, nameof(startupModuleType));

            var modules = GetDescriptors(services, startupModuleType);

            modules = SortByDependency(modules, startupModuleType);

            return modules.ToArray();
        }

        private List<ServiceModuleDescriptor> GetDescriptors(
            IServiceCollection services,
            Type startupModuleType
           )
        {
            var modules = new List<ServiceModuleDescriptor>();

            FillModules(modules, services, startupModuleType);
            SetDependencies(modules);

            return modules.Cast<ServiceModuleDescriptor>().ToList();
        }

        protected virtual void FillModules(
            List<ServiceModuleDescriptor> modules,
            IServiceCollection services,
            Type startupModuleType
          )
        {
           
            //All modules starting from the startup module
            foreach (var moduleType in ServiceModuleHelper.FindAllModuleTypes(startupModuleType, _logger))
            {
                modules.Add(CreateModuleDescriptor(services, moduleType));
            }

          
        }

        protected virtual void SetDependencies(List<ServiceModuleDescriptor> modules)
        {
            foreach (var module in modules)
            {
                SetDependencies(modules, module);
            }
        }

        protected virtual List<ServiceModuleDescriptor> SortByDependency(List<ServiceModuleDescriptor> modules, Type startupModuleType)
        {
            var sortedModules = modules.SortByDependencies(m => m.Dependencies);
            sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
            return sortedModules;
        }

        protected virtual ServiceModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType, bool isLoadedAsPlugIn = false)
        {
            return new ServiceModuleDescriptor(moduleType, CreateAndRegisterModule(services, moduleType), isLoadedAsPlugIn);
        }

        protected virtual ServiceModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
        {
            var module = (ServiceModule)Activator.CreateInstance(moduleType);
            services.AddSingleton(moduleType, module);
            return module;
        }

        protected virtual void SetDependencies(List<ServiceModuleDescriptor> modules, ServiceModuleDescriptor module)
        {
            foreach (var dependedModuleType in ServiceModuleHelper.FindDependedModuleTypes(module.Type))
            {
                var dependedModule = modules.FirstOrDefault(m => m.Type == dependedModuleType);
                if (dependedModule == null)
                {
                    throw new HeusException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
                }

                module.AddDependency(dependedModule);
            }
        }
    }
}
