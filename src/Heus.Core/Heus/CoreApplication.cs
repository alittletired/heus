using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Heus.DependencyInjection;
using Heus.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Heus
{
    public class CoreApplication :IApplication
    {
        public Type StartupModuleType { get; }
        public IServiceCollection Services { get; set; } 
        public IReadOnlyList<ServiceModuleDescriptor> Modules { get; set; } = new List<ServiceModuleDescriptor>();
        public CoreApplication(IServiceCollection services,Type startupModuleType)
        {
            StartupModuleType = startupModuleType;
            Services = services;  
        }

        public void AddServices()
        {
            AddCoreServices(Services);
            Modules = LoadModules(Services);
            ConfigureServices();
        }

        private void AddCoreServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();
            services.AddLocalization();
            services.AddSingleton(typeof(IApplication), this);
            services.AddSingleton<IModuleContainer>(this);
            var moduleScanner = new ModuleScanner(this);
            services.TryAddSingleton(moduleScanner);
            var loggerFactory = new LoggerFactory();
            var moduleLoader = new ModuleLoader(loggerFactory.CreateLogger(this.GetType().Name));
            services.TryAddSingleton(moduleLoader);
        }
        private IReadOnlyList<ServiceModuleDescriptor> LoadModules(IServiceCollection services)
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

            HashSet<Type> serviceTypes = new HashSet<Type>();
            var registrars = Services.GetServiceRegistrars();
            var serviceRegisterContext = new ServiceRegisterContext(Services);
            //ConfigureServices
            foreach (var module in Modules)
            {

                if (!module.Instance.SkipAutoServiceRegistration)
                {
                    // Services.AddAssembly(module.Type.Assembly);
                }

                var types = module.Assembly.GetTypes()
                    .Where(type => !serviceTypes.Contains(type)&&
                                   type.IsClass &&
                                   !type.IsAbstract &&
                                   !type.IsGenericType
                    );
                foreach (var type in types)
                {
                    foreach (var registrar in registrars) 
                    {
                        registrar.Handle(serviceRegisterContext,type);
                    }
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
            using var scope =Services.GetSingletonInstance<IServiceProvider>().CreateScope();
            scope.ServiceProvider
                .GetRequiredService<ModuleManager>()
                .ShutdownModules(new ApplicationShutdownContext(scope.ServiceProvider));
        }

        private  void InitializeModules()
        {
            // using var scope =Services.GetSingletonInstance<IServiceProvider>().CreateScope();
            // scope.ServiceProvider
            //     .GetRequiredService<ModuleManager>()
            //     .InitializeModules(new ApplicationInitializationContext(scope.ServiceProvider));
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            Services.AddSingleton(serviceProvider);
            InitializeModules();
        }
        public void Dispose()
        {
        }
    }
}