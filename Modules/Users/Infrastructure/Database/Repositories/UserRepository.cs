using Shared.Infrastructure.Database.Repositories;
using System.Linq.Expressions;
using Users.Domain.Models;
using Users.Dto.Request;
using Users.Dto.Response;
using Users.Shared.Repositories;

namespace Users.Infrastructure.Database.Repositories;

internal class UserRepository(UsersDbContext dbContext) : Repository<User, UserDto, UserCreateDto, UserUpdateDto>(dbContext, ToDto), IUserRepository
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

