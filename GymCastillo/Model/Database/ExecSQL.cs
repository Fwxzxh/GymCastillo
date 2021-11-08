using System;
using System.Threading.Tasks;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.Database {
    /// <summary>
    /// Clase que se encarga de ejecutar comandos SQL comúnes.
    /// </summary>
    public static class ExecSql {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de ejecutar <c>NonQuerys</c> Ej. Update, Delete, Insert.
        /// </summary>
        /// <param name="query">Objeto tipo <c>MySqlCommand</c> que contiene el string de conección y la query SQL a ejecutar.</param>
        /// <param name="tipo"><c>string</c> que contiene informacion de la query, para mensjaes de error y logging.</param>
        /// <returns>El número de columnas afectadas por la querry.</returns>
        public static async Task<int> NonQuery(MySqlCommand query, string tipo) {
            Log.Debug($"Se va a ejecutar una sentencia SQL tipo {tipo}");

            try {
                var res = await query.ExecuteNonQueryAsync();
                Log.Debug("Se ha ejecutado la secuencia SQL exitosamente.");

                return res;
            }
            catch (Exception e) {
                Log.Error($"Ha ocurrido un error desconocido al ejecutar una sequencia SQL tipo: {tipo}");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al ejecutar {tipo}. Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Clase que se enc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static int Query(MySqlCommand query, string tipo) {
            return 0;
        }
    }
}