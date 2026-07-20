
using Contracts.Domain.Models;
using Users.Domain.Enums;

namespace Users.Domain.Models;

public class User : BaseEntity
{
    public required string Mobile { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public required bool IsActive { get; set; }
    public Role Role { get; set; }

}
