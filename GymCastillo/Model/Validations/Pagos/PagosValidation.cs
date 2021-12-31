using FluentValidation;
using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model.Validations.Pagos {
    public class PagosValidation : AbstractValidator<AbstractMovimientos> {
        public PagosValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(clase => clase.Concepto)
                .NotEmpty().WithMessage("El concepto no puede estar vacío.")
                .Length(3, 300).WithMessage("El concepto debe de ser de entre 3 y 300 caracteres.");

            RuleFor(clase => clase.NumeroRecibo)
                // .NotEmpty().WithMessage("El número de recibo no puede estar vacío.")
                .Length(0, 30).WithMessage("El número de recibo debe de ser de entre 3 y 30 caracteres.");

            RuleFor(clase => clase.Monto)
                .NotEmpty().WithMessage("El número de recibo no puede estar vacío o en 0.");

            RuleFor(clase => clase.FechaRegistro)
                .NotNull().WithMessage("la fecha de registro no puede ser nula.");
        }
    }
}