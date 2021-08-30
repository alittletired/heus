using System;
using System.Collections.Generic;
using System.Linq;
using Heus.Internal;
using Heus.Modularity;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Heus
{
    public class HeusApplication : IHeusApplication
    {
        [NotNull] public Type StartupModuleType { get; }

        public IServiceProvider ServiceProvider { get; private set; }

        public IServiceCollection Services { get; }

        public IReadOnlyList<ServiceModuleDescriptor> Modules { get; }

        public HeusApplication([NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            [CanBeNull] Action<ApplicationCreationOptions> optionsAction)
        {
            Check.NotNull(startupModuleType, nameof(startupModuleType));
            Check.NotNull(services, nameof(services));
            StartupModuleType = startupModuleType;
            Services = services;
            var options = new ApplicationCreationOptions(services);
            optionsAction?.Invoke(options);
            services.AddCoreServices(this, options);
            Modules = LoadModules(services, options);
            ConfigureServices();
        }

        protected IReadOnlyList<ServiceModuleDescriptor> LoadModules(IServiceCollection services,
            ApplicationCreationOptions options)
        {
            return services
                .GetSingletonInstance<ModuleLoader>()
                .LoadModules(
                    services,
                    StartupModuleType
                );
        }

        protected void ConfigureServices()
        {
            var context = new ServiceConfigurationContext(Services);
            Services.AddSingleton(context);
            //PreConfigureServices
            foreach (var module in Modules)
            {
                try
                {
                    module.Instance.PreConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new HeusException(
                        $"An error occurred during PreConfigureServices phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.",
                        ex);
                }
            }

            //ConfigureServices
            foreach (var module in Modules)
            {

                if (!module.Instance.SkipAutoServiceRegistration)
                {
                    Services.AddAssembly(module.Type.Assembly);
                }

                try
                {
                    module.Instance.ConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new HeusException(
                        $"An error occurred during ConfigureServices phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.",
                        ex);
                }
            }

            //PostConfigureServices
            foreach (var module in Modules)
            {
                try
                {
                    module.Instance.PostConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new HeusException(
                        $"An error occurred during PostConfigureServices phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.",
                        ex);
                }
            }


        }

        public  void Shutdown()
        {
            using var scope = ServiceProvider.CreateScope();
            scope.ServiceProvider
                .GetRequiredService<ModuleManager>()
                .ShutdownModules(new ApplicationShutdownContext(scope.ServiceProvider));
        }

        protected  void InitializeModules()
        {
            using var scope = ServiceProvider.CreateScope();
            scope.ServiceProvider
                .GetRequiredService<ModuleManager>()
                .InitializeModules(new ApplicationInitializationContext(scope.ServiceProvider));
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));
            ServiceProvider = serviceProvider;
            InitializeModules();
        }


        public void Dispose()
        {
        }
    }
}