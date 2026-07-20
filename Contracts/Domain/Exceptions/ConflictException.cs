using Microsoft.AspNetCore.Http;

namespace Contracts.Domain.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string message = "Resource already exists", string errorCode = "CONFLICT")
        : base(message, StatusCodes.Status409Conflict, errorCode) { }
}