using System.ComponentModel.DataAnnotations;

namespace OpenQR.API.Endpoints.V1.QrCodes;

public sealed record CreateQrCodeRequest([Required][Url] string Url);

public sealed record QrCodeResponse(string Id, string Url, DateTimeOffset CreatedAt);
