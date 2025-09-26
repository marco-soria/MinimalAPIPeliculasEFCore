using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculasEFCore.Repositorios;

namespace MinimalAPIPeliculasEFCore.DTOs;

public class CrearGeneroPeticionDTO
{
    public IRepositorioGeneros Repositorio { get; set; } = null!;
    public IOutputCacheStore OutputCacheStore { get; set; } = null!;
    public IMapper Mapper { get; set; } = null!;
}
