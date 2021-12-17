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
    /// Clase que contiene todos los métodos y campos del tipo de datos personal
    /// </summary>
    public class Personal : AbstUsuario {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// El puesto que desempeña este cliente.
        /// </summary>
        public string Puesto { get; set; }

        /// <summary>
        /// La fecha de último pago del Personal.
        /// </summary>
        public DateTime FechaUltimoPago { get; set; }

        /// <summary>
        /// El monto del último pago.
        /// </summary>
        public decimal MontoUltimoPago { get; set; }

        public override async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update de un objeto tipo Personal");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string updateQuery = @"UPDATE usuario
                                             SET domicilio=@Domicilio, telefono=@Telefono, 
                                                 NombreContacto=@NombreContacto,
                                                 telefonocontacto=@TelefonoContacto, foto=@Foto
                                             WHERE IdUsuario=@IdUsuario";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@IdUsuario", Id.ToString());
                command.Parameters.AddWithValue("@Domicilio", Domicilio);
                command.Parameters.AddWithValue("@Telefono", Telefono);

                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);

                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", FotoRaw);

                var res = await ExecSql.NonQuery(command, "Update Personal");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer update de Personal.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de Delete en cliente.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();

                const string deleteQuery = @"delete from personal where IdPersonal=@IdPersonal";

                await using var command = new MySqlCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("@IdPersonal", Id.ToString());

                var res = await ExecSql.NonQuery(command, "Delete Personal");
                Log.Debug("Se ha eliminado un cliente de la tabla.");
                return res;

            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete de Personal.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta Personal.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"INSERT INTO personal
                                           VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
                                           	    @Domicilio, @Puesto, @FechaNacimiento, @Telefono, 
                                           	    @NombreContacto, @TelefonoContacto, @Foto,
                                           	    @FechaUltimoPago, @MontoUltimoPago)";


                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@Nombre", Nombre);
                command.Parameters.AddWithValue("@ApellidoPaterno", ApellidoPaterno);
                command.Parameters.AddWithValue("@ApellidoMaterno", ApellidoMaterno);

                command.Parameters.AddWithValue("@Domicilio", Domicilio);
                command.Parameters.AddWithValue("@Puesto", Puesto);
                command.Parameters.AddWithValue("@FechaNacimiento", FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Telefono", Telefono);

                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);
                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", FotoRaw);

                command.Parameters.AddWithValue("@FechaUltimoPago", FechaUltimoPago.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@MontoUltimoPago", MontoUltimoPago.ToString(CultureInfo.InvariantCulture));

                Log.Debug("Se ha creado la query.");

                var res =await ExecSql.NonQuery(command, "Alta Personal");
                Log.Debug("Se ha dado de alta un Personal.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta el personal.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}