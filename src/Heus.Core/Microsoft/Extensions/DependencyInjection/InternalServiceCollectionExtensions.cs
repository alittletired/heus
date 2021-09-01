using Heus.DependencyInjection;
using Heus.Modularity;
using Heus.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Heus.Internal
{
    public static class InternalServiceCollectionExtensions
    {
            internal static void AddCoreServices(this IServiceCollection services,
                IHeusApplication abpApplication,
                ApplicationCreationOptions applicationCreationOptions)
            {
                services.AddOptions();
                services.AddLogging();
                services.AddLocalization();
                
                services.AddSingleton(typeof(IHeusApplication), abpApplication);
                services.AddSingleton<IModuleContainer>(abpApplication);
                
                var assemblyFinder = new ModuleAssemblyFinder(abpApplication);
                var typeFinder = new ModuleTypeFinder(assemblyFinder);
               
                services.TryAddSingleton(assemblyFinder);
                services.TryAddSingleton(typeFinder);
                var loggerFactory = new LoggerFactory();
                var moduleLoader = new ModuleLoader(loggerFactory.CreateLogger(abpApplication.GetType().Name));
                services.TryAddSingleton(moduleLoader);
                services.AddAssemblyOf<IHeusApplication>();

                // services.AddTransient(typeof(ISimpleStateCheckerManager<>), typeof(SimpleStateCheckerManager<>));

               
            }
        }
    
}