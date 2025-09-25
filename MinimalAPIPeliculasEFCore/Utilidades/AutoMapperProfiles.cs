using AutoMapper;
using MinimalAPIPeliculasEFCore.DTOs;
using MinimalAPIPeliculasEFCore.Entidades;

namespace MinimalAPIPeliculasEFCore.Utilidades;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<CrearGeneroDTO, Genero>();
        CreateMap<Genero, GeneroDTO>();
    }
}
