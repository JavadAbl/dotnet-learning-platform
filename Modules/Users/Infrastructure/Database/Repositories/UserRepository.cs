using Shared.Infrastructure.Database.Repositories;
using System.Linq.Expressions;
using Users.Shared.Repositories;
using Users.Domain.Models;
using Users.Dto.Response;

namespace Users.Infrastructure.Database.Repositories;

internal class UserRepository(UsersDbContext dbContext) : Repository<User, UserDto>(dbContext, ToDto), IUserRepository
{

    private static readonly Expression<Func<User, UserDto>> ToDto =
     u => new UserDto(
         u.Id,
         u.FirstName,
         u.LastName,
         u.Mobile,
         u.IsActive,
         u.Role);


}

