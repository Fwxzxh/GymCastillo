using System;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.Init {
    /// <summary>
    /// Clase que se encarga del proseso de LogIn.
    /// </summary>
    public class Init {

        /// <summary>
        /// String que contiene el username del usuario logueado.
        /// </summary>
        public string LogedUsername { get; set; }

        /// <summary>
        /// Método que se encarga del proceso de logIn.
        /// </summary>
        /// <param name="username">string con el nombre de usuario.</param>
        /// <param name="password">string con la contraseña del usuario.</param>
        /// <returns><c>True</c> si el logIn fue exitoso, si no <c>False</c></returns>
        public static bool LogIn(string username, string password) {
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

                // True si el user/passwrd es correcto false si no
                return cmd.HasRows;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}