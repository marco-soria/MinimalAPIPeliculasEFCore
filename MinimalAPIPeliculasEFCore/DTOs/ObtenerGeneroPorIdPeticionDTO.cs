using AutoMapper;
using MinimalAPIPeliculasEFCore.Repositorios;

namespace MinimalAPIPeliculasEFCore.DTOs;

public class ObtenerGeneroPorIdPeticionDTO
{
    public int Id { get; set; }
    public IRepositorioGeneros Repositorio { get; set; } = null!;
    public IMapper Mapper { get; set; } = null!;
}
