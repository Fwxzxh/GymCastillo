using System;
using GymCastillo.Model.Helpers;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.Init {
    /// <summary>
    /// Clase que se encarga del proceso de LogIn.
    /// </summary>
    public static class Init {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// String que contiene el username del usuario que inicio sección.A
        /// </summary>
        public static int LoggedId { get; set; }
        
        /// <summary>
        /// El número del rol que tiene el usuario que inicio sección.
        /// </summary>
        public static int LoggedRol { get; set; }

        /// <summary>
        /// Método que se encarga del proceso de logIn.
        /// </summary>
        /// <param name="username">string con el nombre de usuario.</param>
        /// <param name="password">string con la contraseña del usuario.</param>
        /// <returns><c>True</c> si el logIn fue exitoso, si no <c>False</c></returns>
        public static bool LogIn(string username, string password) {

            Log.Debug("Se ha empezado el proceso de LogIn");
            var connObj = new MySqlConnection(GetInitData.GetConnString());
            connObj.Open();

            var login = new MySqlCommand {
                Connection = connObj,
                CommandText = @"select IdUsuario, Rol
                                from usuario 
                                where Username=@user and Password=@pass"
            };
            login.Parameters.AddWithValue("@user", username);
            login.Parameters.AddWithValue("@pass", password);

            try {
                var cmd = login.ExecuteReader();
                Log.Debug("Se ha realizado la query de LogIn con éxito.");

                if (cmd.HasRows) {
                    while (cmd.Read()) {
                        LoggedId = cmd.GetInt32("IdUsuario");
                        LoggedRol = cmd.GetInt32("Rol");
                    }
                    Log.Debug("LogIn Exitoso");
                    return true;
                }
                else {
                    Log.Debug("LogIn Fallido, credenciales erróneas");
                    return false;
                }
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al hacer la query de logIn.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Error: {e.Message}", "Error desconocido en Init");
                return false;
            }
            finally {
                connObj.Close();
            }
        }
    }
}