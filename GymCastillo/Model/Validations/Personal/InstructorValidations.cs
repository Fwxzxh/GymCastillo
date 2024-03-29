﻿using System.Linq;
using FluentValidation;
using GymCastillo.Model.DataTypes.Personal;

namespace GymCastillo.Model.Validations.Personal {
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
            
            RuleFor(usuario => usuario.Telefono)
                .Length(10).WithMessage("El número de teléfono debe de ser de 10 dígitos.")
                .Must(IsNumber).WithMessage("El número de teléfono solo debe de contener números");
            
            
        }
        
        private static bool IsNumber(string number) {
            return number.All(char.IsNumber);
        }
    }
}