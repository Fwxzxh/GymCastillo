using FluentValidation;
using GymCastillo.Model.DataTypes.Settings;

namespace GymCastillo.Model.Validations.Config {
    public class ClaseValidation : AbstractValidator<Clase> {

        public ClaseValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(clase => clase.IdClase)
                .NotNull().WithMessage("El id de clase no debe de ser nulo.");

            RuleFor(clase => clase.NombreClase)
                .NotEmpty().WithMessage("El nombre de la clase no puede estar vacío.")
                .Length(3, 50).WithMessage("El nombre de la clase debe de ser de entre 3 y 50 caracteres.");

            RuleFor(clase => clase.Descripcion)
                .NotEmpty().WithMessage("La descripción no puede estar vacía")
                .Length(3, 500).WithMessage("La descripción de la clase debe de ser de entre 3 y 500 caracteres.");

            RuleFor(clase => clase.CupoMaximo)
                .NotEmpty().WithMessage("El cupo maximo no puede estar vacío.");

            RuleFor(clase => clase.IdEspacio)
                .NotNull().WithMessage("El Espacio de la clase no puede ser nulo.");

            RuleFor(clase => clase.Activo)
                .NotNull().WithMessage("Se debe de indicar si la clase esta activa o no.");
        }
    }
}