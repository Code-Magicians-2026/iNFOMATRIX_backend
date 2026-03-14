using Infomatrix.Core.Domain.Features.Child;
using Infomatrix.Core.Domain.Features.Family;
using Infomatrix.Core.Domain.Features.Parent;
using Microsoft.EntityFrameworkCore;

namespace Infomatrix.Core.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<ParentEntity> Parents { get; set; }
    public DbSet<ChildEntity> Children { get; set; }
    public DbSet<FamilyEntity> Families { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var parentEntity = modelBuilder.Entity<ParentEntity>();
        parentEntity.HasKey(x => x.Id);

        var childEntity = modelBuilder.Entity<ChildEntity>();
        childEntity.HasKey(x => x.Id);

        var familyEntity = modelBuilder.Entity<FamilyEntity>();
        familyEntity.HasKey(x => x.Id);
    }
}
