using Microsoft.AspNetCore.Cors;
using MinimalAPIPeliculasEFCore.Entidades;

var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenespermitidos")!;
// Services area

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>
    {
        configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });

    opciones.AddPolicy("libre", configuracion =>
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// end of services area



var app = builder.Build();

//Middleware area

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseOutputCache();

app.MapGet("/", [EnableCors(policyName: "libre")] () => "¡Hola, mundo!");

app.MapGet("/generos", () =>
{
    var generos = new List<Genero>
    {
        new() { Id = 1, Nombre = "Acción" },
        new() { Id = 2, Nombre = "Comedia" },
        new() { Id = 3, Nombre = "Drama" }
    };

    return generos;
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));

//end of middleware area
app.Run();
