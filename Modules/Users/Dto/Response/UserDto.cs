using Users.Domain.Enums;

namespace Users.Dto.Response;

internal record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Mobile,
    bool IsActive,
    Role Role,
    string? Description
);
