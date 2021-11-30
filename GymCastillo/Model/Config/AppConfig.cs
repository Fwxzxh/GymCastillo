using System;
using System.Configuration;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Helpers;
using log4net;

namespace GymCastillo.Model.Config {

    /// <summary>
    /// Clase que se encarga de manejar todas las configuraciones de la aplicación.
    /// </summary>
    public static class AppConfig {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de leer, validar y actualizar la configuración del programa.
        /// </summary>
        public static void UpdateConfig(ConfigFields appConfig) {
            try {
                // TODO: validamos

                // Guardamos
                ConfigurationManager.AppSettings.Set("DbUser", appConfig.DbUser);
                ConfigurationManager.AppSettings.Set("DbPass", appConfig.DbPass);
                ConfigurationManager.AppSettings.Set("PrecioLocker", appConfig.CostoLocker);
                ConfigurationManager.AppSettings.Set("DescuentoRetardo", appConfig.DescuentoRetardo);

                Log.Debug("Se han guardado las configuraciones.");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al guardar el archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al guardar las configuraciones, contacte a los administradores.",
                    "Error desconocido");
            }
        }
    }
}