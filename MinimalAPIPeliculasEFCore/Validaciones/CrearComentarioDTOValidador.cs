using FluentValidation;
using MinimalAPIPeliculasEFCore.DTOs;

namespace MinimalAPIPeliculasEFCore.Validaciones;

public class CrearComentarioDTOValidador : AbstractValidator<CrearComentarioDTO>
{
    public CrearComentarioDTOValidador()
    {
        RuleFor(x => x.Cuerpo).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje);
    }
}
