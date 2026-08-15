using Contracts.Infrastructure.Database.Repositories;
using Users.Domain.Models;
using Users.Dto.Response;

namespace Users.Contracts.Repositories;

internal interface IUserRepository : IRepository<User, UserDto>
{
}

