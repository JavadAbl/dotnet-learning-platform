using Shared.Dto.Request;
using Shared.Dto.Response;
using Users.Shared.Repositories;
using Users.Shared.Services;
using Users.Dto.Request;
using Users.Dto.Response;
using Users.Domain.Models;

namespace Users.Services;

internal class UserService(IUserRepository userRep) : IUserService
{

    public Task SuperAdminCreate(string seedPass)
    {
        throw new NotImplementedException();
    }

    public Task UserChangePasswordOtp(UserChangePasswordOtpDto payload)
    {
        throw new NotImplementedException();
    }

    public async Task<int> UserCreate(UserCreateDto payload)
    {
        await userRep.CheckDuplicate(p => p.Mobile == payload.Mobile);
        var user = new User { FirstName = payload.FirstName, LastName = payload.LastName, Mobile = payload.Mobile, Password = "1" };
        await userRep.AddAsync(user);
        await userRep.SaveChangesAsync();
        return user.Id;
    }

    public async Task<UserDto> UserGetById(int id)
    {
        return await userRep.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<GetManyResponse<UserDto>> UserGetMany(GetManyQuery? query)
    {
        var searchableFields = new[] { "FirstName", "LastName", "Email" };
        return await userRep.FindMany(query, searchableFields);
    }

    public Task UserUpdate(int userId, UserUpdateDto payload)
    {
        throw new NotImplementedException();
    }
}

