using Scalar.AspNetCore;

namespace OpenQR.API.OpenApi;

public static class OpenApiExtensions
{
    public static IServiceCollection AddApiOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }

    public static WebApplication MapApiOpenApi(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        return app;
    }
}
