using FluentValidation;
using GymCastillo.Model.DataTypes.Otros;

namespace GymCastillo.Model.Validations.Pagos {
    public class RentasValidation : AbstractValidator<Rentas> {

        public RentasValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(renta => renta.IdClienteRenta)
                .NotNull().WithMessage("El id del cliente renta no debe ser nulo.");

            RuleFor(renta => renta.IdEspacio)
                .NotNull().WithMessage("El id del espacio de renta no debe ser nulo.");

            RuleFor(renta => renta.Dia)
                .NotNull().WithMessage("El dia no puede ser nulo ")
                .GreaterThanOrEqualTo(1).WithMessage("El dia no es un día válido, Menor a 1.")
                .LessThanOrEqualTo(7).WithMessage("El dia no es un día válido, Mayor a 7");

            RuleFor(renta => renta.HoraInicio)
                .NotNull().WithMessage("La hora de inicio no debe ser nula.");

            RuleFor(renta => renta.HoraFin)
                .NotNull().WithMessage("La hora de fin no debe ser nula.");

            RuleFor(renta => renta.Costo)
                .NotNull().WithMessage("El costo de la renta no debe ser nulo.");
        }
    }
}