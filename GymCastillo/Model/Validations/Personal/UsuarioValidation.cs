using System.Linq;
using FluentValidation;
using GymCastillo.Model.DataTypes.Personal;

namespace GymCastillo.Model.Validations.Personal {
    public class UsuarioValidation : AbstractValidator<Usuario> {

        public UsuarioValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(usuario => usuario.Username)
                .NotEmpty().WithMessage("El nombre de usuario no debe de estar vacío.")
                .Length(3, 20).WithMessage("El nombre de usuario debe de tener entre 3 y 20 caracteres")
                .Must(IsLetter).WithMessage("El nombre de usuario solo debe de contener letras");

            RuleFor(usuario => usuario.Password)
                .NotEmpty().WithMessage("El password no debe de estar vacío.")
                .Length(4, 15).WithMessage("El password debe de tener entre 4 y 15 caracteres")
                .Must(IsLetterOrNumber).WithMessage("El password solo debe de tener números y letras");
        }

        private static bool IsLetter(string number) {
            return number.All(char.IsLetter);
        }

        private static bool IsLetterOrNumber(string number) {
            return number.All(char.IsLetterOrDigit);
        }
    }
}