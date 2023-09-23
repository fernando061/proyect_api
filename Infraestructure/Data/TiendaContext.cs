using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data;
public class TiendaContext : DbContext
{
    public TiendaContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Rol> Rol { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }
}
