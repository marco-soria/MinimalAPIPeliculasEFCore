using Microsoft.EntityFrameworkCore;
using MinimalAPIPeliculasEFCore.Entidades;

namespace MinimalAPIPeliculasEFCore;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Genero>().Property(p => p.Nombre).HasMaxLength(50);
    }
    public DbSet<Genero> Generos { get; set; }
}
