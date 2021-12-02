using System;
using System.Threading.Tasks;

namespace GymCastillo.Model.DataTypes.Abstract {
    /// <summary>
    /// Clase abstracta que contiene los campos y métodos comúnes entre clientes e instructores.
    /// </summary>
    public abstract class AbstClientInstructor : AbstUsuario {

        /// <summary>
        /// La fecha de la ultima entrada al gym del usuario.
        /// </summary>
        public DateTime FechaUltimoAcceso { get; set; }

        /// <summary>
        /// La fecha del ultimo movimiento monetario hecho al usuario.
        /// </summary>
        public DateTime FechaUltimoPago { get; set; }

        /// <summary>
        /// El monto de el último movimiento monetario hecho al usuario.
        /// </summary>
        public decimal MontoUltimoPago { get; set; }

        /// <summary>
        /// Método que da de alta una nueva asitencia a el usuario
        /// </summary>
        /// <returns>El núemro de columnas afectadas en la query.</returns>
        public abstract Task<int> NuevaAsistencia();

        /// <summary>
        /// Método que aplica
        /// </summary>
        public abstract void Pago(decimal cantidad);

        /// <summary>
        /// Método que obtiene el horario del cliente o Instructor.
        /// </summary>
        /// <returns>El horario en string</returns>
        public abstract string GetHorarioStr();
    }
}