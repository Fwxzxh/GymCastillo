using FluentValidation;
using GymCastillo.Model.DataTypes.Settings;

namespace GymCastillo.Model.Validations.Config {
    public class PaqueteValidation : AbstractValidator<Paquete> {

        public PaqueteValidation() {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(paquete => paquete.Gym)
                .NotNull().WithMessage("El gym no puede ser nulo.");

            RuleFor(paquete => paquete.NombrePaquete)
                .NotEmpty().WithMessage("El nombre del paquete no puede estar vacío")
                .Length(3, 100).WithMessage("El nombre del paquete debe de tener entre 3 y 100 caracteres.");

            RuleFor(paquete => paquete.Descripcion)
                .NotEmpty().WithMessage("La descripción del paquete no puede estar vacío")
                .Length(3, 300).WithMessage("La descripción del paquete debe de tener entre 3 y 300 caracteres.");

            RuleFor(paquete => paquete.NumClasesTotales)
                .NotEmpty().WithMessage("El número de clases totales no puede ser 0");

            RuleFor(paquete => paquete.NumClasesSemanales)
                .NotEmpty().WithMessage("El número de clases semanales no puede ser 0");

            RuleFor(paquete => paquete.Costo)
                .NotEmpty().WithMessage("El costo del paquete no puede estar vacío o ser 0.");
        }
    }
}