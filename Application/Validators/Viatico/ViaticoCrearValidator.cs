using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ViaticoDTO;
using FluentValidation;

namespace Application.Validators.Viatico
{
    public class ViaticoCrearValidator : AbstractValidator<ViaticoCrearDTO>
    {
        public ViaticoCrearValidator()
        {
            RuleFor(x => x.CategoriaId)
                .GreaterThan(0).WithMessage("La categoría es obligatoria.");

            RuleFor(x => x.UsuarioAppId)
                .GreaterThan(0).WithMessage("El usuario es obligatorio.");

            RuleFor(x => x.CicloId)
                .GreaterThan(0).WithMessage("El ciclo es obligatorio.");

            RuleFor(x => x.Facturas)
                .NotNull().WithMessage("Debe enviar al menos una factura.")
                .Must(f => f.Count > 0).WithMessage("Debe registrar al menos una factura.");

            RuleForEach(x => x.Facturas).ChildRules(f =>
            {
                f.RuleFor(x => x.RucProveedor)
                    .NotEmpty().WithMessage("El RUC del proveedor es obligatorio.")
                    .MaximumLength(13).WithMessage("El RUC no puede exceder 13 caracteres.");

                f.RuleFor(x => x.ProveedorNombre)
                    .NotEmpty().WithMessage("El nombre del proveedor es obligatorio.")
                    .MaximumLength(255).WithMessage("El nombre del proveedor no puede exceder 255 caracteres.");

                f.RuleFor(x => x.NumeroFactura)
                    .NotEmpty().WithMessage("El número de factura es obligatorio.")
                    .MaximumLength(50).WithMessage("El número de factura no puede exceder 50 caracteres.");

                f.RuleFor(x => x.Total)
                    .GreaterThan(0).WithMessage("El total debe ser mayor a cero.");

                f.RuleFor(x => x.FechaFactura)
                    .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de factura no puede ser futura.");
            });

            RuleFor(x => x.VehiculoId)
                .GreaterThan(0).When(x => x.CategoriaId == 1) // Ejemplo: si 1 = Movilización
                .WithMessage("El vehículo es obligatorio para la categoría Movilización.");
        }
    }

}
