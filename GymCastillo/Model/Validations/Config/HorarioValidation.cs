using FluentValidation;
using GymCastillo.Model.DataTypes.Settings;

namespace GymCastillo.Model.Validations.Config {
    public class HorarioValidation : AbstractValidator<Horario> {

        public HorarioValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(horario => horario.IdClase)
                .NotNull().WithMessage("El id de clase no debe de ser nulo.");

            RuleFor(horario => horario.Dia)
                .NotNull().WithMessage("El dia no debe de ser nulo.");

            RuleFor(horario => horario.HoraInicio)
                .NotNull().WithMessage("La hora de inicio no debe de ser nula");

            RuleFor(horario => horario.HoraFin)
                .NotNull().WithMessage("La hora de fin no debe de ser nula");
        }

    }
}