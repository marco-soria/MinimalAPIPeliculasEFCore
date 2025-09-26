using FluentValidation;
using MinimalAPIPeliculasEFCore.DTOs;

namespace MinimalAPIPeliculasEFCore.Validaciones;

public class CrearPeliculaDTOValidador : AbstractValidator<CrearPeliculaDTO>
{
    public CrearPeliculaDTOValidador()
    {
        RuleFor(x => x.Titulo).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
            .MaximumLength(150).WithMessage(Utilidades.MaximumLengthMensaje);
    }
}
