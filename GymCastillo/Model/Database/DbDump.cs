using System;
using System.IO;
using System.Threading.Tasks;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySql.Data.MySqlClient;

namespace GymCastillo.Model.Database {
        
    public static class DbDump {
        private const string Path = @"C:\GymCastillo\Backups\";
        
        private static readonly ILog Log = 
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public static async Task DumpDatabase() {
            Log.Debug("Se ha iniciado el proceso para hacer el dump de la base de datos.");

            CheckBackupFolder();

            var date = DateTime.Now;

            var file = $"Backup-{date.Day.ToString()}-{date.Month.ToString()}-{date.Year.ToString()}_{date.Hour.ToString()}-{date.Minute.ToString()}.sql";
            var fullPath = $"{Path}{file}";

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                
                await using var command = new MySqlCommand();
                
                using var backup = new MySqlBackup(command);
                command.Connection = connection;
                
                await connection.OpenAsync();

                backup.ExportToFile(fullPath);
                
                ShowPrettyMessages.InfoOk(
                    $"Se ha creado el backup de la base de datos exitosamente en la ruta: {fullPath}",
                    "Backup exitoso");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de crear el backup.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al crear el backup. Error: {e.Message}",
                    "Error desconocido");
            }
        }

        /// <summary>
        /// Método que se encarga de crear el directorio de backups
        /// </summary>
        private static void CheckBackupFolder() {
            try {
                if (Directory.Exists(Path)) {
                    return;
                }
                Directory.CreateDirectory(Path);
                Log.Info("Se ha creado el directorio de backups");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de crear el directorio de backups.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al crear el directorio de backups. Error: {e.Message}",
                    "Error desconocido");
            }
        }
    }
}