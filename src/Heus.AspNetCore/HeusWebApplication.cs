
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Heus.Modularity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.AspNetCore;

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
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAspNetCoreServices();
        var app = builder.Build();
        app.UseHeus();
        app.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        Type startModuleType = Assembly.GetExecutingAssembly().GetType();
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
