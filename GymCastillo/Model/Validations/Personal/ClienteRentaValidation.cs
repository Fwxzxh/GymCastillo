using System.Linq;
using FluentValidation;
using GymCastillo.Model.DataTypes.IntersectionTables;
using GymCastillo.Model.DataTypes.Personal;

namespace GymCastillo.Model.Validations.Personal {
    public class ClienteRentaValidation : AbstractValidator<ClienteRenta> {

        public ClienteRentaValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(renta => renta.DeudaCliente)
                .NotNull().WithMessage("La deuda del cliente no puede estar vacía")
                .GreaterThanOrEqualTo(0).WithMessage("La deuda del cliente debe de ser mayor a 0");
            
            RuleFor(usuario => usuario.Telefono)
                .Length(10).WithMessage("El número de teléfono debe de ser de 10 dígitos.")
                .Must(IsNumber).WithMessage("El número de teléfono solo debe de contener números");
        }
        
        
        private static bool IsNumber(string number) {
            return number.All(char.IsNumber);
        }
    }
}