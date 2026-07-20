namespace Users.Dto.Response;

internal record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Mobile,
    bool IsActive,
    string Role
);
