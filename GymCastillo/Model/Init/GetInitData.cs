using System;
using System.IO;
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

        // la ruta al archivo ini.
        private const string IniPath = @"C:\GymCastillo\config.ini";

        /// <summary>
        /// Método que obtiene el string de conexión y lo guarda en el campo de ConnString.
        /// </summary>
        /// <returns>El string de conexión a la base de datos.</returns>
        public static string GetConnString() {
            Log.Info("Se ha empezado el proceso de obtener el string de conexión.");

            try {
                SetUpIni();

                var ini = new IniFile(IniPath);
                var user = ini.Read("DbUser", "Config");
                var pass = ini.Read("DbPass", "Config");
                Log.Info("Se han leído los datos de conexión exitosamente del archivo de configuración.");

                ConnString = $"server=localhost; Uid={user}; pwd={pass}; Database=GymCastillo";

                return ConnString;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al leer la información de conexión del archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de conexión del archivo de " +
                    $"configuración. Error: {e.Message}",
                    "Error desconocido");
                throw; // --> Nos llevamos el error al siguiente nivel.
            }
        }

        /// <summary>
        /// Clase que se encarga de verificar si hay un archivo ini,
        /// </summary>
        private static void SetUpIni() {
            Log.Info("Se ha iniciado el proceso para validar la existencia del ini.");

            try {
                // Validamos que el ini exista
                if (!File.Exists(IniPath)) { // No Existe
                    CreateIni();
                    Log.Warn("Se ha creado un nuevo ini ya que no se encontró uno.");
                    ShowPrettyMessages.WarningOk(
                        "Se ha creado un nuevo archivo de configuración con los valores por default, " +
                        "para ajustarlo a los valores anteriores debes ir a la sección de configuración y editarlos, " +
                        $@"El nuevo archivo de configuración se ha creado en {IniPath}",
                        "Se ha creado un archivo de configuración nuevo");
                }
                else {
                    // Verificamos que todos los campos estén.
                    // Si alguno no esta, mandamos un mensaje y lo re-creamos.
                    CheckFields();
                }
            }
            catch (Exception e) {
                Log.Error("ha ocurrido un error al verificar y/o re crear el archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    "Ha ocurrido un error desconocido al verificar y/o re-crear el archivo de configuración." +
                    $" {e.Message}",
                    "Error desconocido.");
                throw;
            }
        }

        /// <summary>
        /// Método que crea el ini y lo llena con la información default.
        /// </summary>
        private static void CreateIni() {
            try {
                Log.Warn("Se ha iniciado el proceso para crear un nuevo ini desde 0.");

                var ini = new IniFile(IniPath);

                ini.Write("DbUser", "root", "Config");
                ini.Write("DbPass", "root", "Config");
                ini.Write("PrecioLocker", "50", "Settings");
                ini.Write("DescuentoRetardo", "50", "Settings");
                // File.SetAttributes(IniPath, FileAttributes.Hidden);

            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido al crear un nuevo ini.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al crear el archivo de configuración ausente, " +
                    $"Contacte a los administradores. {e.Message}",
                    "Error desconocido");
                throw;
            }
        }

        /// <summary>
        /// Método que checa los campos individuales del ini buscando inconsistencias.
        /// </summary>
        private static void CheckFields() {
            var ini = new IniFile(IniPath);

            if (!ini.KeyExists("DbUser", "Config")) {
                ini.Write("DbUser", "root", "Config");
                ShowPrettyMessages.WarningOk(
                    "Se han encontrado inconsistencias en el Campo DbUser del archivo de configuración, " +
                    "Se ha restaurado con los valores por defecto.",
                    "Error en archivo de configuración.");
            }

            if (!ini.KeyExists("DbPass", "Config")) {
                ini.Write("DbPass", "root", "Config");
                ShowPrettyMessages.WarningOk(
                    "Se han encontrado inconsistencias en el Campo DbPass del archivo de configuración, " +
                    "Se ha restaurado con los valores por defecto.",
                    "Error en archivo de configuración.");
            }

            if (!ini.KeyExists("PrecioLocker", "Settings")) {
                ini.Write("PrecioLocker", "50", "Settings");
                ShowPrettyMessages.WarningOk(
                    "Se han encontrado inconsistencias en el Campo PrecioLocker del archivo de configuración, " +
                    "Se ha restaurado con los valores por defecto.",
                    "Error en archivo de configuración.");
            }

            if (ini.KeyExists("DescuentoRetardo", "Settings")) return;
            ini.Write("DescuentoRetardo", "50", "Settings");
            ShowPrettyMessages.WarningOk(
                "Se han encontrado inconsistencias en el Campo PrecioLocker del archivo de configuración, " +
                "Se ha restaurado con los valores por defecto.",
                "Error en archivo de configuración.");
        }
    }
}