
using Shared.Infrastructure.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Models;

namespace Users.Infrastructure.Database;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyModuleConfigurations(typeof(UsersDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }


}