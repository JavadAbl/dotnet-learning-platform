
using Contracts.Infrastructure.Database.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts;

public static class ContractsModule
{
    public static IServiceCollection AddContractsModule(this IServiceCollection services)
    {


        services.AddSingleton<AuditSaveChangesInterceptor>();

        return services;
    }
}