using System.Globalization;
using System.Linq;
using FluentValidation;
using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model.Validations {
    /// <summary>
    /// Clase que se encarga de validar los campos editables de los objetos tipo Usuario. <br/>
    /// Siendo: Id, Nombre, ApellidoPaterno, ApellidoMaterno, FechaNacimiento, Telefono, NombreContacto, TelefonoContacto.
    /// </summary>
    public class UsuarioGralValidation : AbstractValidator<AbstUsuario> {

        public UsuarioGralValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("es-mx");

            RuleFor(usuario => usuario.Nombre)
                .NotNull().NotEmpty().WithMessage("El nombre no puede estar vacío.")
                .Length(3, 30).WithMessage("El nombre debe de tener entre 3 y 30 caracteres.")
                .Must(IsLetter).WithMessage("El nombre solo debe de tener letras");

            RuleFor(usuario => usuario.ApellidoPaterno)
                .NotNull().NotEmpty().WithMessage("El apellido paterno puede estar vacío.")
                .Length(3, 30).WithMessage("El apellido paterno debe de tener entre 3 y 30 caracteres.")
                .Must(IsLetter).WithMessage("El apellido paterno solo debe de tener letras");

            RuleFor(usuario => usuario.ApellidoMaterno)
                .NotNull().NotEmpty().WithMessage("El apellido materno puede estar vacío.")
                .Length(3, 30).WithMessage("El apellido materno debe de tener entre 3 y 30 caracteres.")
                .Must(IsLetter).WithMessage("El apellido materno solo debe de tener letras");

            RuleFor(usuario => usuario.Domicilio)
                .NotNull().NotEmpty().WithMessage("El domicilio no puede estar vacío.")
                .Length(3, 150).WithMessage("El apellido materno debe de tener entre 3 y 150 caracteres.");

            RuleFor(usuario => usuario.Telefono)
                .NotNull().NotEmpty().WithMessage("El número de teléfono no puede estar vacío.")
                .Length(10).WithMessage("El número de teléfono debe de ser de 10 dígitos.")
                .Must(IsNumber).WithMessage("El número de teléfono solo debe de contener números");

            RuleFor(usuario => usuario.NombreContacto)
                .NotNull().NotEmpty().WithMessage("El nombre de contacto no puede estar vacío.")
                .Length(3, 30).WithMessage("El nombre de contacto debe de tener entre 3 y 30 caracteres.")
                .Must(IsLetter).WithMessage("El nombre de contacto solo debe de tener letras");

            RuleFor(usuario => usuario.TelefonoContacto)
                .NotNull().NotEmpty().WithMessage("El número de teléfono de contacto no puede estar vacío.")
                .Length(10).WithMessage("El número de teléfono de contacto debe de ser de 10 dígitos.")
                .Must(IsNumber).WithMessage("El número de teléfono de contacto solo debe de contener números");
        }

        private static bool IsNumber(string number) {
            return number.All(char.IsNumber);
        }

        private static bool IsLetter(string number) {
            return number.All(char.IsLetter);
        }
    }
}