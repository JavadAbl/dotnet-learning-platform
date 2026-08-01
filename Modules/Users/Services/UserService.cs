using Contracts.Dto.Request;
using Contracts.Dto.Response;
using Contracts.Extensions;
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

    public Task<UserDto> UserGetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<GetManyResponse<UserDto>> UserGetMany(GetManyQuery? query)
    {
        var searchableFields = new[] { "FirstName", "LastName", "Email" };

        var usersQuery = userRep.GetQueryable().ApplyGetManyQuery(query, searchableFields)
            .Select(user => new UserDto(Id: user.Id, FirstName: user.FirstName, LastName: user.LastName, Mobile: user.Mobile, IsActive: user.IsActive, Role: user.Role));

        var users = await userRep.FindMany<UserDto>(usersQuery);

        return users;
    }

    public Task UserUpdate(int userId, UserUpdateDto payload)
    {
        throw new NotImplementedException();
    }
}

