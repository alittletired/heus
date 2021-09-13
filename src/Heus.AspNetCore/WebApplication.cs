using System;
using System.Diagnostics;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Heus.Modularity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.AspNetCore
{
    /// <summary>
    /// web应用
    /// </summary>
    public static class WebApplication
    {
        /// <summary>
        /// 开启web应用
        /// </summary>
        /// <param name="args"></param>
        /// <param name="startModuleType"></param>
        public static void Run(string[] args, Type startModuleType)
        {
            CreateHostBuilder(args, startModuleType).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args, Type startModuleType)
        {
            if (!typeof(IModule).IsAssignableFrom(startModuleType))
            {
                throw new InvalidOperationException($"{startModuleType}必须实现IModule");
            }

            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {


                    webBuilder.ConfigureServices(services =>
                        services.AddSingleton(typeof(IModule), startModuleType));
                    webBuilder.UseStartup<WebStartUp>();
                    var name = startModuleType.Assembly.GetName().Name;
                    webBuilder.UseSetting(WebHostDefaults.ApplicationKey, name);
                });
        }

    }
}