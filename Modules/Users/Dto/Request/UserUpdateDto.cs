namespace Users.Dto.Request;

internal record UserUpdateDto(
    string? FirstName,
    string? LastName,
    string? Mobile,
    bool? IsActive
);
