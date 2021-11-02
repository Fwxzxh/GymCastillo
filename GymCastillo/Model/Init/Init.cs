using System;
using GymCastillo.Model.DataTypes.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.Init {
    /// <summary>
    /// Clase que se encarga del proseso de LogIn.
    /// </summary>
    public static class Init {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// String que contiene el username del usuario logueado.
        /// </summary>
        public static string LogedUsername { get; set; }

        /// <summary>
        /// Método que se encarga del proceso de logIn.
        /// </summary>
        /// <param name="username">string con el nombre de usuario.</param>
        /// <param name="password">string con la contraseña del usuario.</param>
        /// <returns><c>True</c> si el logIn fue exitoso, si no <c>False</c></returns>
        public static bool LogIn(string username, string password) {
            Log.Info("Testing Logger POG.");
            var connObj = new MySqlConnection(GetInitData.GetConnString());
            connObj.Open();

            var login = new MySqlCommand {
                Connection = connObj,
                CommandText = @"select Nombre
                                from usuario 
                                where Username=@user and Password=@pass"
            };
            login.Parameters.AddWithValue("@user", username);
            login.Parameters.AddWithValue("@pass", password);

            try {
                var cmd = login.ExecuteReader(); // TODO: prob cambiar a async

                if (cmd.HasRows) {
                    LogedUsername = username;
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception e) {
                // TODO:
                Console.WriteLine(e);
                throw;
            }
        }
    }
}