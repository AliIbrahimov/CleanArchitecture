using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Persistence.Contexts;

public class BaseDbContext:DbContext
{
    public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration):base(dbContextOptions)
    {
        Configuration = configuration;
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<Model>()
    .Property(m => m.DailyPrice)
    .HasColumnType("decimal(18, 2)");
    }
    public IConfiguration Configuration { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Fuel> Fuels { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Transmission> Transmissions { get; set; }
}
