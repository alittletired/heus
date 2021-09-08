using System;
using System.Collections.Generic;
using System.Linq;
using Heus.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Heus
{
    public class HeusApplication : IHeusApplication
    {
        public Type StartupModuleType { get; }

        public IServiceProvider ServiceProvider { get; private set; }

        public IServiceCollection Services { get; }

        public IReadOnlyList<ServiceModuleDescriptor> Modules { get; }

        public HeusApplication(Type startupModuleType,
             IServiceCollection services,
             Action<ApplicationCreationOptions>? optionsAction)
        {
        
            StartupModuleType = startupModuleType;
            Services = services;
            var options = new ApplicationCreationOptions(services);
            optionsAction?.Invoke(options);
            AddCoreServices(services);
            Modules = LoadModules(services, options);
            ConfigureServices();
        }

        private void AddCoreServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();
            services.AddLocalization();
            services.AddSingleton(typeof(IHeusApplication), this);
            services.AddSingleton<IModuleContainer>(this);
            var moduleScanner = new ModuleScanner(this);
            services.TryAddSingleton(moduleScanner);
            var loggerFactory = new LoggerFactory();
            var moduleLoader = new ModuleLoader(loggerFactory.CreateLogger(this.GetType().Name));
            services.TryAddSingleton(moduleLoader);
            services.AddAssemblyOf<IHeusApplication>();
        }
        private IReadOnlyList<ServiceModuleDescriptor> LoadModules(IServiceCollection services,
            ApplicationCreationOptions options)
        {
            return services
                .GetSingletonInstance<ModuleLoader>()
                .LoadModules(
                    services,
                    StartupModuleType
                );
        }

        private void ConfigureServices()
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

        private  void InitializeModules()
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