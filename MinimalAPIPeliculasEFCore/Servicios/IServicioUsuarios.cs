using Microsoft.AspNetCore.Identity;

namespace MinimalAPIPeliculasEFCore.Servicios;

public interface IServicioUsuarios
{
    Task<IdentityUser?> ObtenerUsuario();
}
