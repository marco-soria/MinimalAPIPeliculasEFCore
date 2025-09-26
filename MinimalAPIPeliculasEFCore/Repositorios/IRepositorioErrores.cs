namespace MinimalAPIPeliculasEFCore.Repositorios;
using Error = MinimalAPIPeliculasEFCore.Entidades.Error;

public interface IRepositorioErrores
{
    Task Crear(Error error);
}
