using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Contracts.Infrastructure.Database.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyModuleConfigurations(this ModelBuilder modelBuilder, Assembly assembly)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

    }

}
