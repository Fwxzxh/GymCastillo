using FluentValidation;
using GymCastillo.Model.DataTypes;

namespace GymCastillo.Model.Validations {
    public class InstructorValidations : AbstractValidator<Instructor> {

        public InstructorValidations() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(instructor => instructor.HoraEntrada.ToString())
                .NotEmpty().WithMessage("La hora de entrada no puede estar vacía")
                .Length(4).WithMessage("La hora de entrada debe ser en formato militar, ej: 0700 para las 7:00 am.");

            RuleFor(instructor => instructor.HoraSalida.ToString())
                .NotEmpty().WithMessage("La hora de salida no puede estar vacía")
                .Length(4).WithMessage("La hora de salida debe ser en formato militar, ej: 0700 para las 7:00 am.");

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