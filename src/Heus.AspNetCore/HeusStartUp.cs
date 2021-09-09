using System;
using Heus.AspNetCore.OpenApi;
using Heus.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Heus.AspNetCore
{
    internal class StartUp
    {
        private readonly Type _startupModule;
        public StartUp(IConfiguration configuration,Type startupModule)
        {
            Configuration = configuration;
            _startupModule = startupModule;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(_startupModule);
            services.AddControllers()
                .AddControllersAsServices();
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<ResponseContentTypeOperationFilter>();
                c.SchemaFilter<EnumSchemaFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Heus.Web", Version = "v1"});
            });
        }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var application = app.ApplicationServices.GetRequiredService<IHeusApplication>();
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