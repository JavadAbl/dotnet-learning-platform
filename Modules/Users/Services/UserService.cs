using Shared.Dto.Request;
using Shared.Dto.Response;
using Users.Domain.Models;
using Users.Dto.Request;
using Users.Dto.Response;
using Users.Shared.Repositories;
using Users.Shared.Services;

namespace Users.Services;

internal class UserService(IUserRepository userRep) : IUserService
{

    public async Task<int> UserCreate(UserCreateDto payload)
    {
        await userRep.CheckDuplicate(p => p.Mobile == payload.Mobile);
        var user = new User { FirstName = payload.FirstName, LastName = payload.LastName, Mobile = payload.Mobile, Password = "1" };
        await userRep.Add(user);
        return user.Id;
    }


    public async Task<UserDto> UserGetDtoById(int id)
    {
        return await userRep.FirstDto(x => x.Id == id);
    }

    public async Task<GetManyResponse<UserDto>> UserGetDtoMany(GetManyQuery? query)
    {
        var searchableFields = new[] { "FirstName", "LastName", "Email" };
        return await userRep.FindDtoMany(query, searchableFields);
    }

    public async Task UserUpdate(int userId, UserUpdateDto payload)
    {
        var entity = await userRep.First(x => x.Id == userId);
        await userRep.UpdatePartial(entity);
    }

    public async Task UserDelete(int userId)
    {
        var entity = await userRep.First(x => x.Id == userId);
        await userRep.Remove(entity);

    }
}

