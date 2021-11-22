using FluentValidation;
using GymCastillo.Model.DataTypes;

namespace GymCastillo.Model.Validations {
    public class InstructorValidations : AbstractValidator<Instructor> {

        public InstructorValidations() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(instructor => instructor.HoraEntrada)
                .NotEmpty().WithMessage("La hora de entrada no puede estar vacía");

            RuleFor(instructor => instructor.HoraSalida)
                .NotEmpty().WithMessage("La hora de salida no puede estar vacía");

            RuleFor(instructor => instructor.DiasATrabajar)
                .NotNull().WithMessage("Los dias a trabajar no pueden ser nulos.")
                .GreaterThanOrEqualTo(0).WithMessage("Los dias a trabajar deben de ser mayor o igual a 0.");

            RuleFor(instructor => instructor.DiasTrabajados)
                .NotNull().WithMessage("Los dias a trabajar no pueden ser nulos.")
                .GreaterThanOrEqualTo(0).WithMessage("Los dias a trabajados deben de ser mayor o igual a 0.");

            RuleFor(instructor => instructor.Sueldo)
                .NotNull().WithMessage("El sueldo no debe ser nulo.")
                .GreaterThanOrEqualTo(0).WithMessage("El sueldo debe de ser mayor o igual a 0.");

            RuleFor(instructor => instructor.SueldoADescontar)
                .NotNull().WithMessage("El sueldo a descontar no debe ser nulo.")
                .GreaterThanOrEqualTo(0).WithMessage("El sueldo a descontar de ser mayor o igual a 0.");

            RuleFor(instructor => instructor.IdTipoInstructor)
                .NotEmpty().WithMessage("El id de instructor no debe estar vacío.");

            RuleFor(instructor => instructor.IdClase)
                .NotEmpty().WithMessage("El id de clase no debe estar vacío.");
        }
    }
}