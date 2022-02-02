using FluentValidation;
using GymCastillo.Model.DataTypes.Ventas;

namespace GymCastillo.Model.Validations.Ventas {
    public class VentaValidation : AbstractValidator<Venta> {

        public VentaValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(venta => venta.FechaVenta)
                .NotEmpty().WithMessage("La Fecha De venta no puede estar vacía.");

            RuleFor(venta => venta.Concepto)
                .NotNull().WithMessage("El concepto de la venta no puede ser nulo.")
                .Length(3, 2000).WithMessage("El concepto debe de tener entre 3 y 2000 caracteres");

            RuleFor(venta => venta.Costo)
                .NotEmpty().WithMessage("El costo de la venta no puede estar vacío o ser 0.");
        }

    }
}