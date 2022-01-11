using FluentValidation;
using GymCastillo.Model.DataTypes.IntersectionTables;
using GymCastillo.Model.DataTypes.Settings;

namespace GymCastillo.Model.Validations.Personal {
    public class PaqueteClaseValidation : AbstractValidator<PaquetesClases> {

        public PaqueteClaseValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(paqueteClase => paqueteClase.IdClase)
                .NotEmpty().WithMessage("La clase no puede ser nulo.");

            RuleFor(paqueteClase => paqueteClase.IdPaquete)
                .NotEmpty().WithMessage("El paquete no puede ser nulo.");
        }
    }
}