using Microsoft.AspNetCore.Http;

namespace Contracts.Domain.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Unauthorized", string errorCode = "UNAUTHORIZED")
        : base(message, StatusCodes.Status401Unauthorized, errorCode) { }
}
