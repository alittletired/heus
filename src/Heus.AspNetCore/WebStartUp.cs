using System;
using System.Linq;
using Heus.AspNetCore.OpenApi;
using Heus.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Heus.AspNetCore
{
    internal class WebStartUp
    {
        public IConfiguration Configuration { get; }

        public WebStartUp(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            var startModuleType = services.GetImplementationType(typeof(IModule));
            services.AddApplication(startModuleType);
            services.AddControllers()
                .AddControllersAsServices();
            services.AddSwaggerGen(c =>
            {
                services.Where(t => typeof(IOperationFilter).IsAssignableFrom(t.ServiceType))
                    .ToList()
                    .ForEach(t => c.OperationFilterDescriptors.Add(new FilterDescriptor
                    {
                        Type = t.ImplementationType,
                    }));
                services.Where(t => typeof(ISchemaFilter).IsAssignableFrom(t.ServiceType))
                    .ToList()
                    .ForEach(t => c.SchemaFilterDescriptors.Add(new FilterDescriptor
                    {
                        Type = t.ImplementationType,
                    }));
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Heus.Web", Version = "v1"});
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var application = app.ApplicationServices.GetRequiredService<IApplication>();
            application.Initialize(app.ApplicationServices);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Heus.Web v1"));
            }

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

    }
}