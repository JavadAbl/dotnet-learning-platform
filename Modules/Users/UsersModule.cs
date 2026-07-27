
using Contracts.Infrastructure.Database.Interceptors;
using Contracts.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Contracts.Repositories;
using Users.Contracts.Services;
using Users.Infrastructure.Database;
using Users.Infrastructure.Database.Repositories;
using Users.Services;

namespace Users;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration,
        IMvcBuilder mvcBuilder)
    {


        AddApi(services, configuration, mvcBuilder);
        AddServices(services);
        AddInfrastructure(services, configuration);

        return services;
    }


    private static void AddApi(this IServiceCollection services, IConfiguration configuration,
        IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddApplicationPart(typeof(UsersModule).Assembly);

        mvcBuilder.ConfigureApplicationPartManager(manager =>
        {
            if (!manager.FeatureProviders.Any(p => p is InternalControllerFeatureProvider))
            {
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            }
        });

    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
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