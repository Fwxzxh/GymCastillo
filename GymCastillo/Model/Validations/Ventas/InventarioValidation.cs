using FluentValidation;
using GymCastillo.Model.DataTypes.Ventas;

namespace GymCastillo.Model.Validations.Ventas {
    public class InventarioValidation : AbstractValidator<Inventario> {

        public InventarioValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(item => item.NombreProducto)
                .NotEmpty().WithMessage("El nombre del producto no puede estar vacío.")
                .Length(3, 30).WithMessage("El concepto debe de ser de entre 3 y 30 caracteres.");

            RuleFor(item => item.Descripción)
                .NotEmpty().WithMessage("La descripción del producto no puede estar vacío.")
                .Length(3, 150).WithMessage("La descripción debe de ser de entre 3 y 150 caracteres.");

            RuleFor(item => item.Costo)
                .NotEmpty().WithMessage("El costo del producto no puede estar vacío o ser 0.");

            RuleFor(item => item.Existencias)
                .NotNull().WithMessage("las existencias del producto no pueden estar vacías.");
        }
    }
}