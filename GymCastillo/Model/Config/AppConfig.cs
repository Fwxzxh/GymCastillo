using System;
using FluentValidation;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Validations.Config;
using log4net;

namespace GymCastillo.Model.Config {

    /// <summary>
    /// Clase que se encarga de manejar todas las configuraciones de la aplicación.
    /// </summary>
    public static class AppConfig {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de validar y actualizar la configuración del programa.
        /// </summary>
        /// <param name="appConfig">Un objeto tipo <c>ConfigFields</c> con las configuraciones.</param>
        public static void UpdateConfig(ConfigFields appConfig) {
            try {
                // Validamos
                var configFieldsValidation = new ConfigFieldsValidation();
                configFieldsValidation.ValidateAndThrowAsync(appConfig);

                // Guardamos
                var ini = new IniFile(@"C:\GymCastillo\config.ini");

                ini.Write("DbUser", appConfig.DbUser, "Config");
                ini.Write("DbPass", appConfig.DbPass, "Config");
                ini.Write("PrecioLocker", appConfig.PrecioLocker, "Settings");
                ini.Write("DescuentoRetardo", appConfig.DescuentoRetardo, "Settings");


                Log.Debug("Se han guardado las configuraciones.");
            }
            catch (ValidationException msg) {
                ShowPrettyMessages.WarningOk($"{msg.Message}", "Datos erróneos");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al guardar el archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al guardar las configuraciones, contacte a los administradores.",
                    "Error desconocido");
            }
        }

        /// <summary>
        /// Método que lee los campos del archivo de configuración y retorna un objeto de configuración.
        /// </summary>
        /// <returns>Un objeto tipo <c>ConfigFields</c> con las configuraciones.</returns>
        public static ConfigFields GetConfig() {
            Log.Debug("Se ha iniciado el proceso de leer todos los campos del archivo de configuración.");
            try {
                var ini = new IniFile(@"C:\GymCastillo\config.ini");

                var config = new ConfigFields() {
                    DbUser = ini.Read("DbUser", "Config"),
                    DbPass = ini.Read("DbPass", "Config"),
                    PrecioLocker = ini.Read("PrecioLocker", "Settings"),
                    DescuentoRetardo = ini.Read("DescuentoRetardo", "Settings")
                };
                Log.Debug("Se han leído todos los campos del archivo de configuración exitosamente.");

                return config;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al leer todos los campos del archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    "Ha ocurrido un error desconocido al leer del archivo de configuración, " +
                    $"contacte a los administradores. Error {e.Message}",
                    "Error desconocido");
                throw;
            }
        }
    }
}