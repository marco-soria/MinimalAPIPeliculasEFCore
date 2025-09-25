using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculasEFCore.DTOs;
using MinimalAPIPeliculasEFCore.Entidades;
using MinimalAPIPeliculasEFCore.Repositorios;

namespace MinimalAPIPeliculasEFCore.Endpoints;

public static class GenerosEndpoints
{
    public static RouteGroupBuilder MapGeneros(this RouteGroupBuilder group)
    {
        group.MapGet("/", ObtenerGeneros).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos-get"));
        group.MapGet("/{id:int}", ObtenerGeneroPorId);
        group.MapPost("/", CrearGenero);
        group.MapPut("/{id:int}", ActualizarGenero);
        group.MapDelete("/{id:int}", BorrarGenero);
        return group;
    }

    static async Task<Ok<List<GeneroDTO>>> ObtenerGeneros(IRepositorioGeneros repositorio, IMapper mapper)
    {
        var generos = await repositorio.ObtenerTodos();
        var generosDTO = mapper.Map<List<GeneroDTO>>(generos);
        return TypedResults.Ok(generosDTO);
    }

    static async Task<Results<Ok<GeneroDTO>, NotFound>> ObtenerGeneroPorId(IRepositorioGeneros repositorio,
        int id, IMapper mapper)
    {
        var genero = await repositorio.ObtenerPorId(id);

        if (genero is null)
        {
            return TypedResults.NotFound();
        }

        var generoDTO = mapper.Map<GeneroDTO>(genero);

        return TypedResults.Ok(generoDTO);
    }

    static async Task<Results<Created<GeneroDTO>, BadRequest<string>>> CrearGenero(CrearGeneroDTO crearGeneroDTO,
        IRepositorioGeneros repositorio,
        IOutputCacheStore outputCacheStore, IMapper mapper)
    {
        var existeNombre = await repositorio.ExisteNombre(crearGeneroDTO.Nombre);

        if (existeNombre)
        {
            return TypedResults.BadRequest($"Ya existe un género con el nombre '{crearGeneroDTO.Nombre}'");
        }

        var genero = mapper.Map<Genero>(crearGeneroDTO);
        var id = await repositorio.Crear(genero);
        await outputCacheStore.EvictByTagAsync("generos-get", default);
        var generoDTO = mapper.Map<GeneroDTO>(genero);
        return TypedResults.Created($"/generos/{id}", generoDTO);
    }

    static async Task<Results<NoContent, NotFound, BadRequest<string>>> ActualizarGenero(int id,
        CrearGeneroDTO crearGeneroDTO,
        IRepositorioGeneros repositorio,
        IOutputCacheStore outputCacheStore, IMapper mapper)
    {
        var existe = await repositorio.Existe(id);

        if (!existe)
        {
            return TypedResults.NotFound();
        }

        // Verificar si ya existe otro género con el mismo nombre (excluyendo el actual)
        var existeNombre = await repositorio.ExisteNombre(crearGeneroDTO.Nombre, id);

        if (existeNombre)
        {
            return TypedResults.BadRequest($"Ya existe otro género con el nombre '{crearGeneroDTO.Nombre}'");
        }

        var genero = mapper.Map<Genero>(crearGeneroDTO);
        genero.Id = id;

        await repositorio.Actualizar(genero);
        await outputCacheStore.EvictByTagAsync("generos-get", default);
        return TypedResults.NoContent();
    }

    static async Task<Results<NoContent, NotFound>> BorrarGenero(int id, IRepositorioGeneros repositorio,
        IOutputCacheStore outputCacheStore)
    {
        var existe = await repositorio.Existe(id);

        if (!existe)
        {
            return TypedResults.NotFound();
        }

        await repositorio.Borrar(id);
        await outputCacheStore.EvictByTagAsync("generos-get", default);
        return TypedResults.NoContent();
    }
}