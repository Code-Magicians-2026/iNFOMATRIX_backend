using Infomatrix.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infomatrix.Core.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
