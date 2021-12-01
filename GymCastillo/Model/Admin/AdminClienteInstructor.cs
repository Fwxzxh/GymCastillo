using System;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.DataTypes.Abstract;
using log4net;

namespace GymCastillo.Model.Admin {
    /// <summary>
    /// Clase que se encarga de exponer todos las operaciones communes para objetos tipo <c>AbstClientInstructor</c>.
    /// </summary>
    public static class AdminClienteInstructor {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de dar de alta una asistencia al objeto tipo AbstClientInstructor en la base de datos.
        /// </summary>
        /// <param name="objeto">El objeto al cual se le va a dar de alta la asistencia.</param>
        /// <param name="clase">El objeto tipo Horario al cual se le va a aumentar el cupo actual.</param>
        public static void NuevaAsistencia(AbstClientInstructor objeto, Horario clase) {
            // Descontamos la clase del cliente.
            // Actualizamos la asistencia en el horario indicado.
            // Validamos
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que se encarga de actualizar los campos de deuda y sueldo Por pagar del objeto AbsClientInstructor.
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="cantidad"></param>
        public static void Pago(AbstClientInstructor objeto, decimal cantidad) {
            // Validamos si hay deuda.
            // Si hay deuda descontamos la cantidad al objeto dado.
            // Validamos
            throw new NotImplementedException();
        }
    }
}