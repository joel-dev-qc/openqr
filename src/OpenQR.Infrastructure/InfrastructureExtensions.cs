using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace OpenQR.Infrastructure;

public static class InfrastructureExtensions
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddMongoDBClient("openqr");

        builder.Services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var connectionString = builder.Configuration.GetConnectionString("openqr") ?? string.Empty;
            var mongoUrl = MongoUrl.Create(connectionString);
            var databaseName = string.IsNullOrWhiteSpace(mongoUrl.DatabaseName) ? "openqr" : mongoUrl.DatabaseName;
            return client.GetDatabase(databaseName);
        });

        return builder;
    }
}
