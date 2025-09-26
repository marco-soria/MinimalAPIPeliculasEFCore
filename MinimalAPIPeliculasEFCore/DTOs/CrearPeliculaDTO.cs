﻿namespace MinimalAPIPeliculasEFCore.DTOs;

public class CrearPeliculaDTO
{
    public string Titulo { get; set; } = null!;
    public bool EnCines { get; set; }
    public DateTime FechaLanzamiento { get; set; }
    public IFormFile? Poster { get; set; }
}
