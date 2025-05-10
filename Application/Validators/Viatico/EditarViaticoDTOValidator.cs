using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ViaticoDTO;
using FluentValidation;

namespace Application.Validators.Viatico
{
    public class EditarViaticoDTOValidator : AbstractValidator<EditarViaticoDTO>
    {
        public EditarViaticoDTOValidator()
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.NombreProveedor) || !string.IsNullOrWhiteSpace(x.NumeroFactura))
                .WithMessage("Debe proporcionar al menos 'NombreProveedor' o 'NumeroFactura'.");

            When(x => !string.IsNullOrWhiteSpace(x.NombreProveedor), () =>
            {
                RuleFor(x => x.NombreProveedor!)
                    .MaximumLength(100).WithMessage("El nombre del proveedor no puede exceder los 100 caracteres.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.NumeroFactura), () =>
            {
                RuleFor(x => x.NumeroFactura!)
                    .MaximumLength(50).WithMessage("El número de factura no puede exceder los 50 caracteres.");
            });
        }
    }
}
