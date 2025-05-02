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

            RuleFor(x => x.Factura)
                .NotNull().WithMessage("La información de la factura es obligatoria.");

            When(x => x.Factura != null, () =>
            {
                RuleFor(x => x.Factura.RucProveedor)
                    .NotEmpty().WithMessage("El RUC del proveedor es obligatorio.")
                    .MaximumLength(13).WithMessage("El RUC no puede exceder 13 caracteres.");

                RuleFor(x => x.Factura.ProveedorNombre)
                    .NotEmpty().WithMessage("El nombre del proveedor es obligatorio.")
                    .MaximumLength(255).WithMessage("El nombre del proveedor no puede exceder 255 caracteres.");

                RuleFor(x => x.Factura.NumeroFactura)
                    .NotEmpty().WithMessage("El número de factura es obligatorio.")
                    .MaximumLength(50).WithMessage("El número de factura no puede exceder 50 caracteres.");

                RuleFor(x => x.Factura.Total)
                    .GreaterThan(0).WithMessage("El total debe ser mayor a cero.");

                RuleFor(x => x.Factura.FechaFactura)
                    .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de factura no puede ser futura.");
            });

            RuleFor(x => x.VehiculoId)
                .GreaterThan(0).WithMessage("El vehiculo es obligatorio.");
        }
    }
}
