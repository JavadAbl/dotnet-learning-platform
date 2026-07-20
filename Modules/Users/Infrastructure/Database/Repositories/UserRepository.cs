using Contracts.Infrastructure.Database.Repositories;
using Users.Contracts.Repositories;
using Users.Domain.Models;

namespace Users.Infrastructure.Database.Repositories;

internal class UserRepository(UsersDbContext dbContext) : Repository<User>(dbContext), IUserRepository
{
}

