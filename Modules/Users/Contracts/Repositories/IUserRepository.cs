using Shared.Infrastructure.Database.Repositories;
using Users.Domain.Models;
using Users.Dto.Request;
using Users.Dto.Response;

namespace Users.Shared.Repositories;

internal interface IUserRepository : IRepository<User, UserDto, UserCreateDto, UserUpdateDto>
{

    internal Task Update(int id, UserUpdateDto payload);
}

