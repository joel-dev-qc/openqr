var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongodb")
    .AddDatabase("openqr");

var api = builder.AddProject<Projects.OpenQR_API>("openqr-api")
    .WithReference(mongo)
    .WaitFor(mongo);

builder.AddProject<Projects.OpenQR_Web>("openqr-web")
    .WithReference(api)
    .WaitFor(api);

await builder.Build().RunAsync().ConfigureAwait(false);
