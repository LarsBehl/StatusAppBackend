using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StatusAppBackend.Utils
{
    public class SwaggerAuthConfig : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if(operation.Security is null)
                operation.Security = new List<OpenApiSecurityRequirement>();
            
            if(operation.Tags.Any(t => t.Name == "Authentication"))
                return;
            
            if(operation.Tags.Any(t => t.Name == "Service"))
                return;
            
            var scheme = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme, Id = "bearer"
                }
            };
            operation.Security.Add(new OpenApiSecurityRequirement()
            {
                [scheme] = new List<string>()
            });
        }
    }
}