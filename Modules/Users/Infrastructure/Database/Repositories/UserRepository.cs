using Contracts.Infrastructure.Database.Repositories;
using System.Linq.Expressions;
using Users.Contracts.Repositories;
using Users.Domain.Models;
using Users.Dto.Response;

namespace Users.Infrastructure.Database.Repositories;

internal class UserRepository : Repository<User, UserDto>, IUserRepository
{

    private static readonly Expression<Func<User, UserDto>> ToDto =
     u => new UserDto(
         u.Id,
         u.FirstName,
         u.LastName,
         u.Mobile,
         u.IsActive,
         u.Role);

    public UserRepository(UsersDbContext dbContext)
       : base(dbContext, ToDto)
    {
    }

}

