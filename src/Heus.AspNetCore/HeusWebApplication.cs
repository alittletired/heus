using System;
using System.Diagnostics;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
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
        public static void Run(string[] args, Assembly entryAssembly = null)
        {
            CreateHostBuilder(args, entryAssembly).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args, Assembly entryAssembly) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<HeusStartUp>();
                    entryAssembly ??= Assembly.GetEntryAssembly();
                    var name = entryAssembly!.GetName().Name;
                    webBuilder.UseSetting(WebHostDefaults.ApplicationKey, name);
                });
    }
}