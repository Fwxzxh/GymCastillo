using System;
using System.Configuration;
using GymCastillo.Model.Helpers;
using log4net;

namespace GymCastillo.Model.Init {
    /// <summary>
    /// Helper que expone toda la información necesaria para el inicio del programa.
    /// </summary>
    public static class GetInitData {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Cadena de conexión a la base de datos.
        /// </summary>
        public static string ConnString { get; set; }

        /// <summary>
        /// Método que obtiene el string de conexión y lo guarda en el campo de ConnString.
        /// </summary>
        /// <returns>El string de conexión a la base de datos.</returns>
        public static string GetConnString() {
            Log.Info("Se ha empezado el proceso de obtener ");

            try {
                var user = ConfigurationManager.AppSettings.Get("DbUser");
                var pass = ConfigurationManager.AppSettings.Get("DBPass");
                Log.Info("Se han leído los datos de conexión exitosamente del archivo de configuración.");

                ConnString = $"server=localhost; Uid={user}; pwd={pass}; Database=GymCastillo";

                return ConnString;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al leer la información de conexión del archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de conexión del archivo de configuración. Error: {e.Message}",
                    "Error desconocido");
                throw; // --> Nos llevamos el error al siguiente nivel.
            }
        }
    }
}