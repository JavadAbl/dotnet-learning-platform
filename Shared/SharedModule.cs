
using Shared.Infrastructure.Database.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace Shared;

public static class SharedModule
{
    public static IServiceCollection AddContractsModule(this IServiceCollection services)
    {


        services.AddSingleton<AuditSaveChangesInterceptor>();

        return services;
    }
}