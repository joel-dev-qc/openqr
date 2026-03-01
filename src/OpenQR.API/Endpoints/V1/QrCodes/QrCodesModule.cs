using Carter;

namespace OpenQR.API.Endpoints.V1.QrCodes;

public sealed class QrCodesModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/qrcodes")
            .WithTags("QrCodes");

        group.MapGet("/", GetQrCodes)
            .Produces<List<QrCodeResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all QR codes")
            .WithDescription("Returns the list of all QR codes.");

        group.MapPost("/", CreateQrCode)
            .Produces<QrCodeResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a QR code")
            .WithDescription("Creates a new QR code for the given URL.");
    }

    private static IResult GetQrCodes()
    {
        return Results.Ok(Array.Empty<QrCodeResponse>());
    }

    private static IResult CreateQrCode(CreateQrCodeRequest request)
    {
        var response = new QrCodeResponse(
            Guid.NewGuid().ToString(),
            request.Url,
            DateTimeOffset.UtcNow);

        return Results.Created($"/v1/qrcodes/{response.Id}", response);
    }
}
