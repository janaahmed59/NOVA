using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerUI;
namespace NOVA.API.Extensions;

public static class SwaggerServiceExtensions
{
    //private const string DarkThemePath = "/swagger-ui/nova-dark.css";

    public static IServiceCollection AddNovaSwagger(
        this IServiceCollection services)
    {
        services.AddSwaggerGen();

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