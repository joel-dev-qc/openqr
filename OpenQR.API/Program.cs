using Carter;
using OpenQR.API.Common;
using OpenQR.API.Health;
using OpenQR.API.Observability;
using OpenQR.API.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSerilog();

builder.Services.AddApiProblemDetails();
builder.Services.AddCarter();
builder.Services.AddApiOpenApi();
builder.Services.AddApiObservability(builder.Configuration);
builder.Services.AddApiHealthChecks();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();
app.MapApiOpenApi();
app.MapApiHealthChecks();

await app.RunAsync().ConfigureAwait(false);
