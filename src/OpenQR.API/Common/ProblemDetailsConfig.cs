namespace OpenQR.API.Common;

public static class ProblemDetailsConfig
{
    public static IServiceCollection AddApiProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails();
        return services;
    }
}
