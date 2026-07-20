using Microsoft.AspNetCore.Http;

namespace Contracts.Domain.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message = "Resource not found", string errorCode = "NOT_FOUND")
        : base(message, StatusCodes.Status404NotFound, errorCode) { }
}
