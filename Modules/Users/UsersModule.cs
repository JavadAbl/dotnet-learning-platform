
using Contracts.Infrastructure.Database.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Contracts.Repositories;
using Users.Infrastructure.Database;
using Users.Infrastructure.Database.Repositories;

namespace Users;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration,
        IMvcBuilder mvcBuilder)
    {

        mvcBuilder.AddApplicationPart(typeof(UsersModule).Assembly);

        AddInfrastructure(services, configuration);

        return services;
    }


    private static void AddInfrastructure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>((sp, options) =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>());
        });

        services.AddScoped<IUserRepository, UserRepository>();
    }
}