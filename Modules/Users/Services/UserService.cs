using Contracts.Dto.Request;
using Contracts.Dto.Response;
using Contracts.Providers;
using Users.Contracts.Services;
using Users.Dto.Request;
using Users.Dto.Response;

namespace Users.Services;

internal class UserService : IUserService
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

    public Task<UserDto> UserGetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetManyResponse<UserDto>> UserGetMany(GetManyQuery query, TokenPayload context)
    {
        throw new NotImplementedException();
    }

    public Task UserUpdate(int userId, UserUpdateDto payload)
    {
        throw new NotImplementedException();
    }
}

