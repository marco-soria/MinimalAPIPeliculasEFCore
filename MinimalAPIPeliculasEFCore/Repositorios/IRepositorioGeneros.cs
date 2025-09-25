using MinimalAPIPeliculasEFCore.Entidades;

namespace MinimalAPIPeliculasEFCore.Repositorios
{
    public interface IRepositorioGeneros
    {
        Task<List<Genero>> ObtenerTodos();
        Task<Genero?> ObtenerPorId(int id);
        Task<int> Crear(Genero genero);
        Task<bool> Existe(int id);
        Task<bool> ExisteNombre(string nombre);
        Task<bool> ExisteNombre(string nombre, int idExcluir);
        Task Actualizar(Genero genero);
        Task Borrar(int id);
    }
}
