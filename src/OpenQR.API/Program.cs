using Carter;
using OpenQR.API.Common;
using OpenQR.API.OpenApi;
using OpenQR.Infrastructure;
using OpenQR.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddInfrastructure();

builder.Services.AddApiProblemDetails();
builder.Services.AddCarter();
builder.Services.AddApiOpenApi();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();
app.MapApiOpenApi();
app.MapDefaultEndpoints();

await app.RunAsync().ConfigureAwait(false);
