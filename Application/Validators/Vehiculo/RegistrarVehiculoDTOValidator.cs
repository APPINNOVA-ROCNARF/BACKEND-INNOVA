using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.VehiculoDTO;
using FluentValidation;

namespace Application.Validators.Vehiculo
{
    public class RegistrarVehiculoDTOValidator : AbstractValidator<RegistrarVehiculoDTO>
    {
        public RegistrarVehiculoDTOValidator()
        {
            RuleFor(x => x.NombreUsuario)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MaximumLength(100);

            RuleFor(x => x.Placa)
                .NotEmpty().WithMessage("La placa es obligatoria.")
                .MaximumLength(10).WithMessage("La placa no puede tener más de 10 caracteres.")
                .Matches("^[A-Z0-9-]+$").WithMessage("La placa solo puede contener letras mayúsculas, números y guiones.");

            RuleFor(x => x.Fabricante)
                .NotEmpty().WithMessage("El fabricante es obligatorio.")
                .MaximumLength(50);

            RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage("El modelo es obligatorio.")
                .MaximumLength(50);

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("El color es obligatorio.")
                .MaximumLength(50);
        }
    }
}
