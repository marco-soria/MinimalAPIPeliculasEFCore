using FluentValidation;
using MinimalAPIPeliculasEFCore.DTOs;

namespace MinimalAPIPeliculasEFCore.Validaciones;

public class EditarClaimDTOValidador : AbstractValidator<EditarClaimDTO>
{
    public EditarClaimDTOValidador()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
            .MaximumLength(256).WithMessage(Utilidades.MaximumLengthMensaje)
            .EmailAddress().WithMessage(Utilidades.EmailMensaje);
    }
}
