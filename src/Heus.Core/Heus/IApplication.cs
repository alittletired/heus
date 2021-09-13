using System;
using Heus.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Heus
{
     public interface IApplication: IModuleContainer,IDisposable
    {
        /// <summary>
        /// Type of the startup (entrance) module of the application.
        /// </summary>
        Type StartupModuleType { get; }

        /// <summary>
        /// List of services registered to this application.
        /// Can not add new services to this collection after application initialize.
        /// </summary>
        IServiceCollection Services { get; }
     
        /// <summary>
        /// Used to gracefully shutdown the application and all modules.
        /// </summary>
        void Shutdown();
        /// <summary>
        /// Sets the service provider and initializes all the modules.
        /// </summary>
        void Initialize(IServiceProvider serviceProvider);
    }
}