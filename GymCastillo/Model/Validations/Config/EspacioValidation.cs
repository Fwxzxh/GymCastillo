using FluentValidation;
using GymCastillo.Model.DataTypes.Settings;

namespace GymCastillo.Model.Validations.Config {
    public class EspacioValidation : AbstractValidator<Espacio> {

        public EspacioValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(espacio => espacio.IdEspacio)
                .NotNull().WithMessage("El id del espacio no debe de ser nulo.");

            RuleFor(espacio => espacio.NombreEspacio)
                .NotEmpty().WithMessage("El nombre del espacio no puede estar vacío.")
                .Length(3, 100).WithMessage("El nombre del espacio debe de tener entre 3 y 100 caracteres.");

            RuleFor(espacio => espacio.Descripción)
                .NotEmpty().WithMessage("La descripción del espacio no puede estar vacía.")
                .Length(3, 100).WithMessage("La descripción del espacio debe de tener entre 3 y 2000 caracteres.");
        }
        
    }
}