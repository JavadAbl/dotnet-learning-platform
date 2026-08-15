using Contracts.Dto.Request;
using Contracts.Dto.Response;
using Users.Contracts.Repositories;
using Users.Contracts.Services;
using Users.Dto.Request;
using Users.Dto.Response;

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

    public Task<int> UserCreate(UserCreateDto payload)
    {
        throw new NotImplementedException();
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

