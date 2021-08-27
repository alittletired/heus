using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
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
        public static void Run(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<HeusStartUp>(); });
    }
}