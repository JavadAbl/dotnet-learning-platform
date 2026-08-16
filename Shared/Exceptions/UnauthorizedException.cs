using Microsoft.AspNetCore.Http;

namespace Shared.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Unauthorized", string errorCode = "UNAUTHORIZED")
        : base(message, StatusCodes.Status401Unauthorized, errorCode) { }
}
