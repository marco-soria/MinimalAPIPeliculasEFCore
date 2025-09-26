using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinimalAPIPeliculasEFCore.Entidades;
using Error = MinimalAPIPeliculasEFCore.Entidades.Error;

namespace MinimalAPIPeliculasEFCore;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Genero>().Property(p => p.Nombre).HasMaxLength(50);

        modelBuilder.Entity<Actor>().Property(p => p.Nombre).HasMaxLength(150);
        modelBuilder.Entity<Actor>().Property(p => p.Foto).IsUnicode(false);

        modelBuilder.Entity<Pelicula>().Property(p => p.Titulo).HasMaxLength(150);
        modelBuilder.Entity<Pelicula>().Property(p => p.Poster).IsUnicode(false);

        modelBuilder.Entity<GeneroPelicula>().HasKey(g => new { g.GeneroId, g.PeliculaId }); // la llave primaria de la tabla GeneroPelicula sera una llave compuesta entre GeneroId y PeliculaId

        modelBuilder.Entity<ActorPelicula>().HasKey(g => new { g.ActorId, g.PeliculaId }); // la llave primaria de la tabla ActorPelicula sera una llave compuesta entre ActorId y PeliculaId

        modelBuilder.Entity<IdentityUser>().ToTable("Usuarios");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsuariosClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UsuariosLogins");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UsuariosRoles");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsuariosTokens");
    }

    public DbSet<Genero> Generos { get; set; }
    public DbSet<Actor> Actores { get; set; }
    public DbSet<Pelicula> Peliculas { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }
    public DbSet<GeneroPelicula> GenerosPeliculas { get; set; }
    public DbSet<ActorPelicula> ActoresPeliculas { get; set; }

    public DbSet<Error> Errores { get; set; }

}
