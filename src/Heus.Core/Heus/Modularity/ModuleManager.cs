using System;
using System.Collections.Generic;
using System.Linq;
using Heus.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Heus.Modularity
{
    internal class ModuleManager : ISingletonDependency
    {
        private readonly IModuleContainer _moduleContainer;
        private readonly ILogger<ModuleManager> _logger;

        public ModuleManager(
            IModuleContainer moduleContainer,
            ILogger<ModuleManager> logger,
            IServiceProvider serviceProvider)
        {
            _moduleContainer = moduleContainer;
            _logger = logger;
        }

        public void InitializeModules(ApplicationInitializationContext context)
        {

            foreach (var module in _moduleContainer.Modules)
            {
                try
                {
                    (module.Instance as IModuleLifecycle)?.Initialize(context);
                }
                catch (Exception ex)
                {
                    throw new HeusException(
                        $"An error occurred during the initialize {module.Instance.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.",
                        ex);
                }
            }

            _logger.LogInformation("Initialized all service modules");
        }

        public void ShutdownModules(ApplicationShutdownContext context)
        {
            var modules = _moduleContainer.Modules.Reverse().ToList();


            foreach (var module in modules)
            {
                try
                {
                    (module.Instance as IModuleLifecycle)?.Shutdown(context);
                }
                catch (Exception ex)
                {
                    throw new HeusException(
                        $"An error occurred during the shutdown {module.Instance.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.",
                        ex);
                }
            }

        }
    }
}