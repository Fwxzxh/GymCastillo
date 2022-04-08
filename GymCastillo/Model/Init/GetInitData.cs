using System;
using System.Globalization;
using System.IO;
using GymCastillo.Model.Helpers;
using log4net;

namespace GymCastillo.Model.Init {
    /// <summary>
    /// Helper que expone toda la información necesaria para el inicio del programa.
    /// </summary>
    public static class GetInitData {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?
            .DeclaringType);

        /// <summary>
        /// Cadena de conexión a la base de datos.
        /// </summary>
        public static string ConnString { get; set; }

        /// <summary>
        /// api key para conectarse al bot de telegram
        /// </summary>
        public static string TelegramApiKey { get; set; }

        /// <summary>
        /// Costo de la visita al gym.
        /// </summary>
        public static decimal VisitaGym { get; set; }

        /// <summary>
        /// Costo de la visita al box.
        /// </summary>
        public static decimal VisitaBox { get; set; }

        /// <summary>
        /// Costo de una visita a la alberca.
        /// </summary>
        public static decimal VisitaAlberca { get; set; }


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

                GetPreciosVisitas();

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
        /// Método que lee los precios de las visitas.
        /// </summary>
        public static void GetPreciosVisitas() {
            Log.Info("Se ha iniciado el proceso de obtener los precios de las visitas.");

            try {

                var ini = new IniFile(IniPath);
                var gym = ini.Read("VisitaGym", "Precios");
                var box = ini.Read("VisitaBox", "Precios");
                var alberca = ini.Read("VisitaAlberca", "Precios");

                VisitaGym = gym == "" ? 0 : decimal.Parse(gym);
                VisitaBox = box == "" ? 0 : decimal.Parse(box);
                VisitaAlberca = alberca == "" ? 0 : decimal.Parse(alberca);

                Log.Info("Se han obtenido los precios de las visitas con éxito.");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al leer la información de los precios de visita del archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los precios de visita del archivo de " +
                    $"configuración. Error: {e.Message}",
                    "Error desconocido");
            }
        }

        /// <summary>
        /// Método que se encarga de leer el numero del movimiento en el mes
        /// </summary>
        public static int GetMonthMovNumerator() {
            Log.Info("Se ha iniciado el proceso de obtener el numero de movimiento del mes.");

            try {
                var ini = new IniFile(IniPath);

                // Checamos si existe la key
                if (!ini.KeyExists("NumMov", "Movimientos")) {
                    // SI no existe la escribimos en el 1
                    ini.Write("NumMov", "1", "Movimientos");
                    return 1;
                }

                var res = ini.Read("NumMov", "Movimientos");
                var num = res == "" ? 1 : int.Parse(res);
                return num;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al leer la información de el numero de movimiento del archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de el numero de movimiento del archivo de " +
                    $"configuración. Error: {e.Message}",
                    "Error desconocido");
                throw;
            }
        }

        /// <summary>
        /// Método que se encarga de escribir el siguiente núm de movimiento del mes
        /// </summary>
        /// <param name="reset">default false, usar true para resetearlo al inicio del mes</param>
        public static void SetNextMonthMovNumerator(bool reset = false) {
            Log.Info("Se ha iniciado el proceso de escribir el siguiente num de movimiento del mes.");

            try {
                var ini = new IniFile(IniPath);

                if (reset) {
                    ini.Write("NumMov", "1", "Movimientos");
                    return;
                }

                var numActual = GetMonthMovNumerator();
                ini.Write("NumMov", $"{(numActual + 1).ToString()}", "Movimientos");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al leer la información de el numero de movimiento del archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de el numero de movimiento del archivo de " +
                    $"configuración. Error: {e.Message}",
                    "Error desconocido");
                throw;
            }
        }

        /// <summary>
        /// Método que se encarga de guardar los precios de las visitas en el ini.
        /// </summary>
        public static void SavePreciosVisitas() {
            Log.Info("Se ha iniciado el proceso de guardar los precios de las visitas.");

            try {
                var ini = new IniFile(IniPath);
                ini.Write("VisitaGym", $"{VisitaGym.ToString(CultureInfo.InvariantCulture)}", "Precios");
                ini.Write("VisitaBox", $"{VisitaBox.ToString(CultureInfo.InvariantCulture)}", "Precios");
                ini.Write("VisitaAlberca", $"{VisitaAlberca.ToString(CultureInfo.InvariantCulture)}", "Precios");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al escribir la información de los precios de visita del archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al escribir la información de los precios de visita del archivo de " +
                    $"configuración. Error: {e.Message}",
                    "Error desconocido");
            }
            ShowPrettyMessages.InfoOk("Se actualizaron los precios correctamente.", "Actualización");
        }

        /// <summary>
        /// Método que se encarga de obtener el apiKey del ini
        /// </summary>
        /// <returns>La api key</returns>
        public static string GetApiKey() {
            Log.Info("Se ha empezado el proceso de obtener el api key de telegram.");

            try {
                var ini = new IniFile(IniPath);
                var key = ini.Read("TlgApi", "Config");
                TelegramApiKey = key;
                Log.Info("Se ha leído los datos de la api key exitosamente del archivo de configuración.");

                return TelegramApiKey;
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
        /// Método que se encarga de escribir en el campo de la api key.
        /// </summary>
        /// <param name="key">String con la api key.</param>
        public static void WriteApikey(string key) {
            Log.Info("Se ha iniciado el proceso de escribir la api key de telegram.");

            try {
                var ini = new IniFile(IniPath);

                ini.Write("TlgApi", key, "Config");
                Log.Info("Se ha escrito la api key de telegram.");
                ShowPrettyMessages.NiceMessageOk(
                    $"Se ha guardado la api key de manera exitosa",
                    "Api key guardada");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al escribir la información de api key del archivo de configuración.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al escribir la información de la api key de telegram en el archivo de " +
                    $"configuración. Error: {e.Message}",
                    "Error desconocido");
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
                ini.Write("TlgApi", "", "Config");

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

            if (!ini.KeyExists("TlgApi", "Config")) {
                ini.Write("TlgApi", "", "Config");
                ShowPrettyMessages.WarningOk(
                    "Se han encontrado inconsistencias en el Campo TlgApi del archivo de configuración, " +
                    "Se ha restaurado con los valores por defecto.",
                    "Error en archivo de configuración.");
            }
        }
    }
}