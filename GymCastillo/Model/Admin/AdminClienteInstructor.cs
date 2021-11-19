using System;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using log4net;

namespace GymCastillo.Model.Admin {
    /// <summary>
    /// Clase que se encarga de exponer todos las operaciones comúnes para objetos tipo <c>AbstClientInstructor</c>.
    /// </summary>
    public static class AdminClienteInstructor {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de validar y dar de alta una clase al objeto tipo AbstClientInstructor en la
        /// base de datos.
        /// </summary>
        /// <param name="objeto">El objeto al cual se le va a dar de alta la clase</param>
        /// <param name="clase">La clase a dar de alta.</param>
        public static void AltaClase(AbstClientInstructor objeto, Clase clase) {
            try {
                // Validamos que se pueda dar de alta la clase.

                // Damos de alta
                var res = objeto.AltaClase(clase).Result;

                // Verificamos los cambios
                if (res == 0) {
                    // No se han hecho cambios a la bd
                    ShowPrettyMessages.WarningOk("No se han hecho cambios a la base de datos", "Sin cambios");
                }
            }
            // TODO: ver como manejar el escenario donde no se puede dar de alta una clase porque chocan.
            catch (Exception e) {
                Log.Error("Ha ocurrio un error desconocido a la hora de hacer el el alta de clase.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
            }
        }

        /// <summary>
        /// Método que se encarga de validad y dar de baja una clase al objeto tipo AbstClientInstructor en la
        /// base de datos
        /// </summary>
        /// <param name="objeto">El objeto al cual se le va a dar de alta la clase.</param>
        /// <param name="clase">La clase a dar de alta.</param>
        public static void BajaClase(AbstClientInstructor objeto, Clase clase) {
            try {
                // Validamos que se pueda dar de baja la clase.

                // Damos de alta
                var res = objeto.BajaClase(clase).Result;

                // Verificamos los cambios
                if (res == 0) {
                    // No se han hecho cambios a la bd
                    ShowPrettyMessages.WarningOk("No se han hecho cambios a la base de datos", "Sin cambios");
                }
            }
            catch (Exception e) {
                Log.Error("Ha ocurrio un error desconocido a la hora de hacer la baja de clase.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
            }
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