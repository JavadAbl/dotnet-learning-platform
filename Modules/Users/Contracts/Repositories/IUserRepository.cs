using Shared.Infrastructure.Database.Repositories;
using Users.Domain.Models;
using Users.Dto.Response;

namespace Users.Shared.Repositories;

internal interface IUserRepository : IRepository<User, UserDto>
{
}

