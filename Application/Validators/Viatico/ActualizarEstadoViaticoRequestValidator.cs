using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ViaticoDTO;
using FluentValidation;

namespace Application.Validators.Viatico
{
    public class ActualizarEstadoViaticoRequestValidator : AbstractValidator<ActualizarEstadoViaticoRequest>
    {
        public ActualizarEstadoViaticoRequestValidator()
        {
            RuleFor(x => x.Accion)
                .IsInEnum().WithMessage("La acción enviada no es válida.");

            RuleFor(x => x.Viaticos)
                .NotEmpty().WithMessage("Debe seleccionar al menos un viático.");

            RuleForEach(x => x.Viaticos).SetValidator(new ActualizarViaticoItemValidator());
        }
    }

    public class ActualizarViaticoItemValidator : AbstractValidator<ActualizarViaticoItem>
    {
        public ActualizarViaticoItemValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del viático debe ser mayor a 0.");
        }
    }
}
