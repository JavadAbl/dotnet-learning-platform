
using Shared.Domain.Models;
using Users.Domain.Enums;

namespace Users.Domain.Models;

public class User : BaseEntity
{
    public required string Mobile { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public bool IsActive { get; set; } = true;
    public Role Role { get; set; }
    public string? Description { get; set; }


}
