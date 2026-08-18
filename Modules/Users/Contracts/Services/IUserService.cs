using Shared.Dto.Request;
using Shared.Dto.Response;
using Users.Dto.Request;
using Users.Dto.Response;

namespace Users.Shared.Services;

internal interface IUserService
{
    Task<UserDto> UserGetDtoById(int id);

    Task<GetManyResponse<UserDto>> UserGetDtoMany(GetManyQuery? query);

    Task<int> UserCreate(UserCreateDto payload);

    Task UserUpdate(int userId, UserUpdateDto payload);

    Task UserDelete(int userId);

}

