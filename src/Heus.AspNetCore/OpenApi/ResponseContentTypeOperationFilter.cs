using System.Linq;
using Heus.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Heus.AspNetCore.OpenApi
{
    [Service]
    internal class ResponseContentTypeOperationFilter: IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // if (!context.ApiDescription.TryGetMethodInfo(out var methodInfo))
            // {
            //     return;
            // }

            operation.Responses.Remove("text/plain");
            operation.Responses.Remove("text/json");

        }
    }
}