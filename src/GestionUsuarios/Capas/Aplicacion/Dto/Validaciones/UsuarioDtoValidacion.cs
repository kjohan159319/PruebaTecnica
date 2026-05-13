using FluentValidation;
using GestionUsuarios.Capas.Aplicacion.Dto;

namespace GestionUsuarios.Capas.Aplicacion.Dto.Validaciones
{
    public class UsuarioDtoValidacion : AbstractValidator<UsuarioDto>
    {
        public UsuarioDtoValidacion()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(30).WithMessage("El nombre no puede superar 30 caracteres.");

            RuleFor(x => x.FechaNacimiento)
                .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria.")
                .LessThan(DateTime.Today).WithMessage("La fecha de nacimiento debe ser anterior a hoy.");

            RuleFor(x => x.Sexo)
                .NotEmpty().WithMessage("El sexo es obligatorio.")
                .Must(s => s == "Masculino" || s == "Femenino" || s == "Otro")
                .WithMessage("El sexo debe ser Masculino, Femenino u Otro.");
        }
    }
}
