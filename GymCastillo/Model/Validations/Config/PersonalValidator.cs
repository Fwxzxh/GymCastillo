using FluentValidation;

namespace GymCastillo.Model.Validations.Config {
    public class PersonalValidator : AbstractValidator<DataTypes.Personal.Personal> {

        public PersonalValidator() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(personal => personal.Puesto)
                .NotEmpty().WithMessage("El Puesto no Debe estar vacío");
        }
    }
}