using System;
using System.Threading.Tasks;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.Admin {
    /// <summary>
    /// Clase que se encarga de aplicar el método only alta en las interfaces donde se ocupan
    /// </summary>
    public class AdminOnlyAlta {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de
        /// </summary>
        public static async Task Alta(IOnlyAlta obtejo) {
            Log.Debug("Se ha iniciado el proceso de alta de un objeto de solo alta.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}