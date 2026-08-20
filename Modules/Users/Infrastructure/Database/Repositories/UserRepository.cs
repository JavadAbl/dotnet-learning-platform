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
         u.Role,
         u.Description);



    async Task IUserRepository.Update(int id, UserUpdateDto payload)
    {
        var user = await First(x => x.Id == id);
        if (payload.FirstName is not null) user.FirstName = payload.FirstName;
        if (payload.LastName is not null) user.LastName = payload.LastName;
        if (payload.Mobile is not null) user.Mobile = payload.Mobile;
        if (payload.IsActive is not null) user.IsActive = payload.IsActive.Value;
        user.Description = payload.Description;
        await SaveChanges();
    }


}

