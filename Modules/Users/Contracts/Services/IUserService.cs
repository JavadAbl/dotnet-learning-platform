using Contracts.Dto.Request;
using Contracts.Dto.Response;
using Contracts.Providers;
using Users.Dto.Request;
using Users.Dto.Response;

namespace Users.Contracts.Services;

internal interface IUserService
{
    Task<UserDto> UserGetById(int id);

    Task<GetManyResponse<UserDto>> UserGetMany(GetManyQuery query, TokenPayload context);

    Task<int> UserCreate(UserCreateDto payload);

    Task UserUpdate(int userId, UserUpdateDto payload);

    Task SuperAdminCreate(string seedPass);

    Task UserChangePasswordOtp(UserChangePasswordOtpDto payload);
}

