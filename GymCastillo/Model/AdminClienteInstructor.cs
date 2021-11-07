using GymCastillo.Model.DataTypes;
using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model {
    /// <summary>
    /// Clase que se encarga de exponer todos las operaciones comúnes para objetos tipo <c>AbstClientInstructor</c>.
    /// </summary>
    public static class AdminClienteInstructor {
        /// <summary>
        /// Método que se encarga de validar y dar de alta una clase al objeto tipo AbstClientInstructor en la
        /// base de datos.
        /// </summary>
        /// <param name="objeto">El objeto al cual se le va a dar de alta la clase</param>
        /// <param name="clase">La clase a dar de alta.</param>
        public static void AltaClase(AbstClientInstructor objeto, Clase clase) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Método que se encarga de validad y dar de baja una clase al objeto tipo AbstClientInstructor en la
        /// base de datos
        /// </summary>
        /// <param name="objeto">El objeto al cual se le va a dar de alta la clase.</param>
        /// <param name="clase">La clase a dar de alta.</param>
        public static void BajaClase(AbstClientInstructor objeto, Clase clase) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Método que se encarga de dar de alta una asistencia al objeto tipo AbstClientInstructor en la base de datos.
        /// </summary>
        /// <param name="objeto">El objeto al cual se le va a dar de alta la asisencia.</param>
        public static void NuevaAsitencia(AbstClientInstructor objeto) {
            throw new System.NotImplementedException();
        }
    }
}