namespace Users.Dto.Request;

internal record UserUpdateDto(
    string? FirstName = null,
    string? LastName = null,
    string? Mobile = null,
    bool? IsActive = null
);
