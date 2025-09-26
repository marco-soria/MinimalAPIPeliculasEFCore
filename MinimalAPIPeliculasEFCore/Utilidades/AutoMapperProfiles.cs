﻿using AutoMapper;
using MinimalAPIPeliculasEFCore.DTOs;
using MinimalAPIPeliculasEFCore.Entidades;

namespace MinimalAPIPeliculasEFCore.Utilidades;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<CrearGeneroDTO, Genero>();
        //         CreateMap<CrearGeneroDTO, Genero>().ReverseMap();

        CreateMap<ActualizarGeneroDTO, Genero>();
        CreateMap<Genero, GeneroDTO>();

        CreateMap<CrearActorDTO, Actor>()
            .ForMember(x => x.Foto, opciones => opciones.Ignore());
        CreateMap<Actor, ActorDTO>();

        CreateMap<CrearPeliculaDTO, Pelicula>()
          .ForMember(x => x.Poster, opciones => opciones.Ignore());
        CreateMap<Pelicula, PeliculaDTO>()
            .ForMember(p => p.Generos,
            entidad => entidad.MapFrom(p =>
            p.GenerosPeliculas.Select(gp =>
                new GeneroDTO { Id = gp.GeneroId, Nombre = gp.Genero.Nombre })))
            .ForMember(p => p.Actores, entidad => entidad.MapFrom(p =>
            p.ActoresPeliculas.Select(ap =>
                new ActorPeliculaDTO
                {
                    Id = ap.ActorId,
                    Nombre = ap.Actor.Nombre,
                    Personaje = ap.Personaje
                })));

        CreateMap<CrearComentarioDTO, Comentario>();
        CreateMap<Comentario, ComentarioDTO>();

        CreateMap<AsignarActorPeliculaDTO, ActorPelicula>();
    }
}
