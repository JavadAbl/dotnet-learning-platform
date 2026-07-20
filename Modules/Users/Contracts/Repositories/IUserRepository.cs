using Contracts.Contracts.Repositories;
using Users.Domain.Models;

namespace Users.Contracts.Repositories;

internal interface IUserRepository : IRepository<User>
{
}

