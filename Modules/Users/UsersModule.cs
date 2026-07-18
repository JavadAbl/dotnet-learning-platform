
using Contracts.Infrastructure.Database.Interceptors;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Infrastructure.Database;

namespace Users;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration,
        IMvcBuilder mvcBuilder)
    {

        mvcBuilder.AddApplicationPart(typeof(UsersModule).Assembly);

        services.AddDbContext<UsersDbContext>((sp, options) =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>());
        });

        return services;
    }
}