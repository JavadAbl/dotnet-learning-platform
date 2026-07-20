using Microsoft.AspNetCore.Http;

namespace Contracts.Domain.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException(string message = "Bad request", string errorCode = "BAD_REQUEST")
        : base(message, StatusCodes.Status400BadRequest, errorCode) { }
}
