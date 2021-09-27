using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Heus.AspNetCore.OpenApi
{
    public static class OpenApiExtensions
    {
        public static void AddOpenApi(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var operationFilters= services.Where(t => typeof(IOperationFilter).IsAssignableFrom(t.ServiceType))
                    .ToList();
                operationFilters.ForEach(t => c.OperationFilterDescriptors.Add(new FilterDescriptor
                    {
                        Type = t.ImplementationType,
                    }));
                var schemaFilters= services.Where(t => typeof(ISchemaFilter).IsAssignableFrom(t.ServiceType))
                    .ToList();
                schemaFilters.ForEach(t => c.SchemaFilterDescriptors.Add(new FilterDescriptor
                    {
                        Type = t.ImplementationType,
                    }));
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Heus.Web", Version = "v1"});
            });
        }

        public static void UseOpenApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", env.ApplicationName));
        }
    }
}