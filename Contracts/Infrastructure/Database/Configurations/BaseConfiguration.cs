using Contracts.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

/// <summary>
/// Generic base class that applies common conventions to every entity
/// derived from <see cref="BaseEntity"/>: primary key, audit columns,
/// soft-delete query filter, and standard column lengths.
///
/// Concrete configurations should call <c>base.Configure(builder)</c> first,
/// then add entity-specific mapping on top.
/// </summary>
/// <typeparam name="T">The entity type derived from <see cref="BaseEntity"/>.</typeparam>
public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
       public virtual void Configure(EntityTypeBuilder<T> builder)
       {
              builder.HasKey(e => e.Id);
              builder.Property(e => e.Id)
                     .ValueGeneratedOnAdd();

              builder.Property(e => e.CreatedAt)
                     .IsRequired();

              builder.Property(e => e.UpdatedAt);

              builder.Property(e => e.CreatedBy)
                     .HasMaxLength(256);

              builder.Property(e => e.UpdatedBy)
                     .HasMaxLength(256);

              builder.Property(e => e.DeletedAt);

              // Soft-delete: every query automatically excludes tombstoned rows.
              // To bypass in rare cases, use IgnoreQueryFilters() in the LINQ query.
              builder.HasQueryFilter(e => e.DeletedAt == null);
       }
}
