using FluentValidation;
using GymCastillo.Model.DataTypes.Personal;

namespace GymCastillo.Model.Validations.Personal {
    public class ClienteValidations : AbstractValidator<Cliente> {

        public ClienteValidations() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(usuario => usuario.CondicionEspecial)
                .NotNull().WithMessage("La condición especial no puede ser nula.");

            RuleFor(usuario => usuario.Activo)
                .NotNull().WithMessage("El atributo activo no puede ser nulo.");

            RuleFor(usuario => usuario.FechaVencimientoPago)
                .NotNull().WithMessage("El atributo fecha vencimiento pago no puede ser nulo.");

            RuleFor(usuario => usuario.MedioConocio)
                .NotNull().WithMessage("El medio conoció no puede ser null.");

            RuleFor(usuario => usuario.ClasesTotalesDisponibles)
                .NotNull().WithMessage("El total de clases disponibles no puede estar vacío.")
                .GreaterThanOrEqualTo(0).WithMessage("El total de clases disponibles debe de ser mayor o igual a 0.");

            RuleFor(usuario => usuario.ClasesSemanaDisponibles)
                .NotNull().WithMessage("El total de clases por semana disponibles no puede estar vacío.")
                .GreaterThanOrEqualTo(0).WithMessage("El total de clases por semana debe de ser mayor o igual a 0.");

            RuleFor(usuario => usuario.Niño)
                .NotNull().WithMessage("El atributo niño no puede ser null.");

            RuleFor(usuario => usuario.IdTipoCliente)
                .NotEmpty().WithMessage("Se debe de seleccionar el tipo de cliente.");

        }
    }
}