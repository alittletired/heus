using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Heus.AspNetCore.OpenApi
{
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