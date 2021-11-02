using MySqlConnector;

namespace GymCastillo.Model.Database {
    /// <summary>
    /// Clase que se encarga de ejecutar comandos SQL comúnes.
    /// </summary>
    public static class ExecSql {

        /// <summary>
        /// Clase que se encarga de ejecutar <c>NonQuerys</c> Ej. Update, Delete, Insert.
        /// </summary>
        /// <param name="query">Objeto tipo <c>MySqlCommand</c> que contiene el string de conección y la query SQL a ejecutar.</param>
        /// <param name="tipo"><c>string</c> que contiene informacion de la query, para mensjaes de error y logging.</param>
        /// <returns>El número de columnas afectadas por la querry.</returns>
        public static int NonQuery(MySqlCommand query, string tipo) {
            return 0;
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