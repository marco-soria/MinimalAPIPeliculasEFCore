using MinimalAPIPeliculasEFCore.Entidades;
using Microsoft.EntityFrameworkCore;

namespace MinimalAPIPeliculasEFCore.Repositorios;

public class RepositorioGeneros : IRepositorioGeneros
{
    private readonly ApplicationDbContext context;

    public RepositorioGeneros(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> ExisteNombre(string nombre)
    {
        return await context.Generos.AnyAsync(x => x.Nombre.ToLower() == nombre.ToLower());
    }

    public async Task<bool> ExisteNombre(string nombre, int idExcluir)
    {
        return await context.Generos.AnyAsync(x => x.Nombre.ToLower() == nombre.ToLower() && x.Id != idExcluir);
    }

    public async Task<bool> Existe(int id)
    {
        return await context.Generos.AnyAsync(x => x.Id == id);
    }

    public async Task<Genero?> ObtenerPorId(int id)
    {
        return await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Genero>> ObtenerTodos()
    {
        return await context.Generos.OrderBy(x => x.Nombre).ToListAsync();
    }

    public async Task<int> Crear(Genero genero)
    {
        context.Add(genero);
        await context.SaveChangesAsync();
        return genero.Id;
    }

    public async Task Actualizar(Genero genero)
    {
        context.Update(genero);
        await context.SaveChangesAsync();
    }

    public async Task Borrar(int id)
    {
        await context.Generos.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}