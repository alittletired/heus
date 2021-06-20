using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Heus.Modularity.PlugIns;

namespace Heus.Modularity
{
    public class ModuleLoader : IModuleLoader
    {
        public IHeusModuleDescriptor[] LoadModules(
            IServiceCollection services,
            Type startupModuleType,
            PlugInSourceList plugInSources)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(startupModuleType, nameof(startupModuleType));
            Check.NotNull(plugInSources, nameof(plugInSources));

            var modules = GetDescriptors(services, startupModuleType, plugInSources);

            modules = SortByDependency(modules, startupModuleType);

            return modules.ToArray();
        }

        private List<IHeusModuleDescriptor> GetDescriptors(
            IServiceCollection services,
            Type startupModuleType,
            PlugInSourceList plugInSources)
        {
            var modules = new List<HeusModuleDescriptor>();

            FillModules(modules, services, startupModuleType, plugInSources);
            SetDependencies(modules);

            return modules.Cast<IHeusModuleDescriptor>().ToList();
        }

        protected virtual void FillModules(
            List<HeusModuleDescriptor> modules,
            IServiceCollection services,
            Type startupModuleType,
            PlugInSourceList plugInSources)
        {
            var logger = services.GetInitLogger<AbpApplicationBase>();

            //All modules starting from the startup module
            foreach (var moduleType in HeusModuleHelper.FindAllModuleTypes(startupModuleType, logger))
            {
                modules.Add(CreateModuleDescriptor(services, moduleType));
            }

            //Plugin modules
            foreach (var moduleType in plugInSources.GetAllModules(logger))
            {
                if (modules.Any(m => m.Type == moduleType))
                {
                    continue;
                }

                modules.Add(CreateModuleDescriptor(services, moduleType, isLoadedAsPlugIn: true));
            }
        }

        protected virtual void SetDependencies(List<HeusModuleDescriptor> modules)
        {
            foreach (var module in modules)
            {
                SetDependencies(modules, module);
            }
        }

        protected virtual List<IHeusModuleDescriptor> SortByDependency(List<IHeusModuleDescriptor> modules, Type startupModuleType)
        {
            var sortedModules = modules.SortByDependencies(m => m.Dependencies);
            sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
            return sortedModules;
        }

        protected virtual HeusModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType, bool isLoadedAsPlugIn = false)
        {
            return new HeusModuleDescriptor(moduleType, CreateAndRegisterModule(services, moduleType), isLoadedAsPlugIn);
        }

        protected virtual IHeusModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
        {
            var module = (IHeusModule)Activator.CreateInstance(moduleType);
            services.AddSingleton(moduleType, module);
            return module;
        }

        protected virtual void SetDependencies(List<HeusModuleDescriptor> modules, HeusModuleDescriptor module)
        {
            foreach (var dependedModuleType in HeusModuleHelper.FindDependedModuleTypes(module.Type))
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
