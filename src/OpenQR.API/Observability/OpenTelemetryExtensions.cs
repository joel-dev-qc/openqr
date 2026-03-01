using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OpenQR.API.Observability;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddApiObservability(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var otlpEndpoint = configuration["OpenTelemetry:OtlpEndpoint"];
        var hasOtlpEndpoint = Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var otlpEndpointUri);

        services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService("OpenQR.API"))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                if (hasOtlpEndpoint)
                {
                    tracing.AddOtlpExporter(o => o.Endpoint = otlpEndpointUri!);
                }
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();

                if (hasOtlpEndpoint)
                {
                    metrics.AddOtlpExporter(o => o.Endpoint = otlpEndpointUri!);
                }
            });

        return services;
    }
}
