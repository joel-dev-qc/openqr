using Serilog;

namespace OpenQR.API.Observability;

public static class SerilogExtensions
{
    public static WebApplicationBuilder AddApiSerilog(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Host.UseSerilog((context, loggerConfiguration) =>
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration));

        return builder;
    }
}
