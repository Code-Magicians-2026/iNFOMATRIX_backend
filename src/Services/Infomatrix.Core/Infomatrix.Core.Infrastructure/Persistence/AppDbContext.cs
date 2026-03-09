using Infomatrix.Core.Domain.Features.User;
using Microsoft.EntityFrameworkCore;

namespace Infomatrix.Core.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<UserEntity>();

        entity.HasKey(x => x.Id);
    }
}
