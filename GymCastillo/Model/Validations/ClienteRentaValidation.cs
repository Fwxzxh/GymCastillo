using FluentValidation;
using GymCastillo.Model.DataTypes;

namespace GymCastillo.Model.Validations {
    public class ClienteRentaValidation : AbstractValidator<ClienteRenta> {

        public ClienteRentaValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(renta => renta.DeudaCliente)
                .NotNull().WithMessage("La deuda del cliente no puede estar vacía")
                .GreaterThanOrEqualTo(0).WithMessage("La deuda del cliente debe de ser mayor a 0");
        }
    }
}