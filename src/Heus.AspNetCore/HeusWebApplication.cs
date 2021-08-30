using System;
using System.Diagnostics;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Heus.Modularity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.AspNetCore
{
    /// <summary>
    /// web应用
    /// </summary>
    public static class HeusWebApplication
    {
        /// <summary>
        /// 开启web应用
        /// </summary>
        /// <param name="args"></param>
        /// <param name="entryAssembly"></param>
        public static void Run<T>(string[] args, Assembly entryAssembly = null) where T:ServiceModule 
        {
            CreateHostBuilder<T>(args, entryAssembly).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder<T>(string[] args, Assembly entryAssembly)where T:ServiceModule  =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<StartUpModule<T>>();
                    var name =typeof(T).Assembly.GetName().Name;
                    webBuilder.UseSetting(WebHostDefaults.ApplicationKey, name);
                });
    }
}