using System;
using System.Globalization;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.Personal {
    /// <summary>
    /// Clase que se encarga de guardar los campos y métodos de objeto tipo Usuario
    /// </summary>
    public class Usuario : AbstUsuario {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// La fecha de ultimo acceso al programa del usuario.
        /// </summary>
        public DateTime FechaUltimoAcceso { get; set; }

        /// <summary>
        /// La fecha del ultimo pago al usuario.
        /// </summary>
        public DateTime FechaUltimoPago { get; set; }

        /// <summary>
        /// La cantidad del último Pago.
        /// </summary>
        public decimal MontoUltimoPago { get; set; }

        /// <summary>
        /// El nombre de usuario de el usuario.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// La contraseña del usuario.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Método que Actualiza la instancia actual del objeto en la base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas por la operación.</returns>
        public override async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update en un objeto tipo Usuario.");
            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();

                const string updateQuery = @"UPDATE usuario
                                             SET domicilio=@Domicilio, username=@Username, password=@Password,
                                                 telefono=@Telefono, NombreContacto=@NombreContacto,
                                                 telefonocontacto=@TelefonoContacto, foto=@Foto
                                             WHERE IdUsuario=@IdUsuario";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@IdUsuario", Id.ToString());

                command.Parameters.AddWithValue("@Domicilio", Domicilio);
                command.Parameters.AddWithValue("@Username", Username);
                command.Parameters.AddWithValue("@Password", Password);

                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);

                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", null); // TODO foto

                var res = await ExecSql.NonQuery(command, "Update Usuario");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer update.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que borra la instancia actual del objeto en la base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas por la operación.</returns>
        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de Delete en usuario.");

            if (Id == 1) {
                ShowPrettyMessages.ErrorOk(
                    "No puedes borrar el usuario admin, ya que podrías no poder entrar al programa después.",
                    "Error al borrar usuario admin.");
                return 0;
            }

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from usuario where IdUsuario=@IdUsuario";

                await using var command = new MySqlCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("@IdUsuario", Id.ToString());
                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Delete Usuario");
                Log.Debug("Se ha eliminado un Usuario de la tabla.");

                return res;
            }
            catch (Exception e) {
                    Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete del Usuario.");
                    Log.Error($"Error: {e.Message}");
                    ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                        "Error desconocido");
                    return 0;
            }
        }

        /// <summary>
        /// Método que da de alta la instancia actual del objeto en la base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas por la operación</returns>
        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta un Usuario.");
            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"INSERT INTO usuario
                                           VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
                                           	    @Domicilio, @Username, @Password, @FechaNacimiento,
                                           	    @Telefono, @NombreContacto, @TelefonoContacto, @Foto,
                                           	    @FechaUltimoAcceso, @FechaUltimoPago, @MontoUltimoPago);";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@Nombre", Nombre);
                command.Parameters.AddWithValue("@ApellidoPaterno", ApellidoPaterno);
                command.Parameters.AddWithValue("@ApellidoMaterno", ApellidoMaterno);

                command.Parameters.AddWithValue("@Domicilio", Domicilio);
                command.Parameters.AddWithValue("@Username", Username);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@FechaNacimiento",
                    FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss"));

                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);
                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", null); //TODO: pendiente

                command.Parameters.AddWithValue("@FechaUltimoAcceso",
                    FechaUltimoAcceso.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@FechaUltimoPago",
                    FechaUltimoPago.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@MontoUltimoPago",
                    MontoUltimoPago.ToString(CultureInfo.InvariantCulture));
                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Alta Usuario");
                Log.Debug("Se ha dado de alta un Usuario.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de desactivar el cliente.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}