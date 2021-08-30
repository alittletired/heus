using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.OpenApi.Any;
using System.Reflection;
using Heus.DependencyInjection;

namespace Heus.AspNetCore.OpenApi
{
    internal class EnumSchemaFilter:ISchemaFilter,ITransientDependency
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum = schema.Enum.Select(enumValue =>
                {        
                    if (enumValue is not OpenApiPrimitive<int> openApiValue)
                    {
                        throw new Exception("枚举类型不能转化");
                    }
                    OpenApiObject apiObject = new();
                    var value = openApiValue.Value;
                    var name = Enum.GetName(context.Type, value);
                    var enumMember =context.Type.GetMember(name!).First() ;
                    apiObject["name"] =new OpenApiString(name) ;
                    apiObject["value"] = new OpenApiInteger(value);
                    var display = enumMember.GetCustomAttribute<DisplayAttribute>();
                    apiObject["text"] =new OpenApiString(display?.Name??name) ;
                    return  apiObject;
                }).Cast<IOpenApiAny>().ToList();
            }
        }
    }
}