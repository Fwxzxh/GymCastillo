using FluentValidation;
using GymCastillo.Model.DataTypes;

namespace GymCastillo.Model.Validations.Config {
    public class ConfigFieldsValidation : AbstractValidator<ConfigFields> {

        public ConfigFieldsValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(field => field.DbUser)
                .NotEmpty().WithMessage("El campo de Usuario de base de datos no puede estar vacío.")
                .Length(3, 10).WithMessage("El campo de Usuario de base de datos debe tener entre 3 y 10 caracteres");

            RuleFor(field => field.DbPass)
                .NotEmpty().WithMessage("El campo de Contraseña de base de datos no puede estar vacío.")
                .Length(3, 10).WithMessage("El campo de Contraseña de base de datos debe tener entre 3 y 10 caracteres");

            // TODO: verificar que los demás campos puedan ser parseados como decimal y validados como tal.
        }
    }
}