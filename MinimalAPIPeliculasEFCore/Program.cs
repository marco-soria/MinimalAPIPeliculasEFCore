using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalAPIPeliculasEFCore;
using MinimalAPIPeliculasEFCore.Endpoints;

using MinimalAPIPeliculasEFCore.GraphQL;
using MinimalAPIPeliculasEFCore.Repositorios;
using MinimalAPIPeliculasEFCore.Servicios;
using MinimalAPIPeliculasEFCore.Swagger;
using MinimalAPIPeliculasEFCore.Utilidades;
using Error = MinimalAPIPeliculasEFCore.Entidades.Error;

var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenespermitidos")!;
// Services area

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
{
    opciones.UseSqlServer("name=DefaultConnection");
});

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutacion>()
    .AddAuthorization()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();

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
//builder.Services.AddStackExchangeRedisOutputCache(opciones =>
//{
//    opciones.Configuration = builder.Configuration.GetConnectionString("redis");
//});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Peliculas API",
        Description = "Este es un web api para trabajar con datos de pel�culas",
        Contact = new OpenApiContact
        {
            Email = "admin@admin.com",
            Name = "Admin",
            Url = new Uri("https://gmail.com")
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/license/mit/")
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.OperationFilter<FiltroAutorizacion>();

    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        }, new string[]{}
    //    }
    //});
});

builder.Services.AddScoped<IRepositorioGeneros, RepositorioGeneros>();
builder.Services.AddScoped<IRepositorioActores, RepositorioActores>();
builder.Services.AddScoped<IRepositorioPeliculas, RepositorioPeliculas>();
builder.Services.AddScoped<IRepositorioComentarios, RepositorioComentarios>();
builder.Services.AddScoped<IRepositorioErrores, RepositorioErrores>();

builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();
builder.Services.AddHttpContextAccessor();

//builder.Services.AddAutoMapper(c => { }, typeof(Program));
builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfiles).Assembly);


builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;

    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //IssuerSigningKey = Llaves.ObtenerLlave(builder.Configuration).First(),
        IssuerSigningKeys = Llaves.ObtenerTodasLasLlaves(builder.Configuration),
        ClockSkew = TimeSpan.Zero
    };

});
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("esadmin", politica => politica.RequireClaim("esadmin"));
});

// end of services area



var app = builder.Build();

//Middleware area

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context =>
{
    var exceptionHandleFeature = context.Features.Get<IExceptionHandlerFeature>();
    var excepcion = exceptionHandleFeature?.Error!;

    var error = new Error();
    error.Fecha = DateTime.UtcNow;
    error.MensajeDeError = excepcion.Message;
    error.StackTrace = excepcion.StackTrace;

    var repositorio = context.RequestServices.GetRequiredService<IRepositorioErrores>();
    await repositorio.Crear(error);

    await TypedResults.BadRequest(
        new { tipo = "error", mensaje = "ha ocurrido un mensaje de error inesperado", estatus = 500 })
    .ExecuteAsync(context);
}));
app.UseStatusCodePages();

app.UseStaticFiles();

app.UseCors();

app.UseOutputCache();

app.UseAuthorization();

app.MapGraphQL();

app.MapGet("/", [EnableCors(policyName: "libre")] () => "�Hola, mundo!");
app.MapGet("/error", () =>
{
    throw new InvalidOperationException("error de ejemplo");
});

app.MapPost("/modelbinding/{nombre}", ([FromRoute] string? nombre) =>
{
    if (nombre is null)
    {
        nombre = "Vac�o";
    }

    return TypedResults.Ok(nombre);
});

app.MapGroup("/generos").MapGeneros();
app.MapGroup("/actores").MapActores();
app.MapGroup("/peliculas").MapPeliculas();
app.MapGroup("/pelicula/{peliculaId:int}/comentarios").MapComentarios();
app.MapGroup("/usuarios").MapUsuarios();

//end of middleware area
app.Run();
