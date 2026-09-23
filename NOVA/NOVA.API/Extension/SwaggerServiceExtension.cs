using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection.Metadata;
using System.Xml.Linq;
namespace NOVA.API.Extensions;

public static class SwaggerServiceExtensions
{
    //private const string DarkThemePath = "/swagger-ui/nova-dark.css";

    public static IServiceCollection AddSwaggerService(
    this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token"
            });

            options.AddSecurityRequirement(document =>
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] =
                        new List<string>()
                });
        });

        return services;
    }

    public static void ConfigureNovaSwaggerUI(
        this SwaggerUIOptions options)
    {
        options.DocumentTitle = "NOVA API";
        //options.InjectStylesheet(DarkThemePath);

        options.DocExpansion(DocExpansion.List);
        options.DefaultModelsExpandDepth(0);
        options.EnableDeepLinking();
        options.DisplayRequestDuration();
    }
}