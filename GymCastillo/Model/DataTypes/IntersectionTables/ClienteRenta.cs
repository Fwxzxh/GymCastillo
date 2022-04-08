using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.IntersectionTables {
    /// <summary>
    /// Clase que se encarga de guardar los campos y métodos del obtejo tipo ClienteRenta
    /// </summary>
    public class ClienteRenta : AbstUsuario {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// La fecha del ultimo pago al usuario.
        /// </summary>
        public DateTime FechaUltimoPago { get; set; }

        /// <summary>
        /// La cantidad del último Pago.
        /// </summary>
        public decimal MontoUltimoPago { get; set; }

        /// <summary>
        /// La deuda del cliente.
        /// </summary>
        public decimal DeudaCliente { get; set; }

        /// <summary>
        /// Método que actualiza la instancia actual del objeto en la base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas en la operación.</returns>
        public override async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update en un objeto tipo ClienteRenta.");
            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();

                const string updateQuery = @"UPDATE ClienteRenta
                                             SET Nombre=@Nombre, ApellidoMaterno=@ApellidoMaterno, ApellidoPaterno=@ApellidoPaterno,
                                                 Domicilio=@Domicilio, Telefono=@Telefono, 
                                                 NombreContacto=@NombreContacto, TelefonoContacto=@TelefonoContacto, 
                                                 Foto=@Foto
                                             WHERE IdClienteRenta=@IdClienteRenta;";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@IdClienteRenta", Id.ToString());

                command.Parameters.AddWithValue("@Nombre", Nombre);
                command.Parameters.AddWithValue("@ApellidoPaterno", ApellidoPaterno);
                command.Parameters.AddWithValue("@ApellidoMaterno", ApellidoMaterno);

                command.Parameters.AddWithValue("@Domicilio", Domicilio);
                command.Parameters.AddWithValue("@Telefono", Telefono);

                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);
                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", FotoRaw);
                command.Parameters.AddWithValue("@DeudaCliente", DeudaCliente.ToString(CultureInfo.InvariantCulture));

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
        /// Método que se encarga de borrar la instancia actual del objeto en la base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas por la operación.</returns>
        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de Delete de un ClienteRenta.");

            // checks
            if (DeudaCliente > 0) {
                ShowPrettyMessages.InfoOk(
                    $"No se puede eliminar este cliente ya que tiene una deuda activa de: $ {DeudaCliente.ToString(CultureInfo.InvariantCulture)}",
                    "Cliente con deuda");
                return 0;
            }

            if (InitInfo.ObCoRentas.Any(x => x.IdClienteRenta == Id)) {
                ShowPrettyMessages.InfoOk(
                    $"No se puede eliminar este cliente ya que tiene registros de ventas pasadas y al eliminarlo podría perder información.",
                    "Cliente con registros.");
                return 0;
            }

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from ClienteRenta where IdClienteRenta=@IdClienteRenta";

                await using var command = new MySqlCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("@IdClienteRenta", Id.ToString());
                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Delete ClienteRente");
                Log.Debug("Se ha eliminado un Cliente Renta de la tabla.");

                return res;
            }
            catch (Exception e) {
                    Log.Error("Ha ocurrido un error desconcoido a la hora de hacer el delete del cliente renta.");
                    Log.Error($"Error: {e.Message}");
                    ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                        "Error desconocido");
                    return 0;
            }
        }

        /// <summary>
        /// Método que da de alta la instancia actual del objeto en la base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas en la operación.</returns>
        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta un ClienteRenta.");
            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"INSERT INTO clienterenta
                                           VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
                                                   @Domicilio, @FechaNacimiento, @Telefono, @NombreContacto,
                                                   @TelefonoContacto, @Foto, @FechaUltimoPago, @MontoUltimoPago,
                                                   @DeudaCliente)";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@Nombre", Nombre);
                command.Parameters.AddWithValue("@ApellidoPaterno", ApellidoPaterno);
                command.Parameters.AddWithValue("@ApellidoMaterno", ApellidoMaterno);

                command.Parameters.AddWithValue("@Domicilio", Domicilio);
                command.Parameters.AddWithValue("@FechaNacimiento", FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);

                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", FotoRaw);
                command.Parameters.AddWithValue("@FechaUltimoPago", FechaUltimoPago.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@MontoUltimoPago", MontoUltimoPago.ToString(CultureInfo.InvariantCulture));

                command.Parameters.AddWithValue("@DeudaCliente", DeudaCliente.ToString(CultureInfo.InvariantCulture));
                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Alta ClienteRenta");
                Log.Debug("Se ha dado de alta un ClienteRenta.");

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

        public override async Task<int> Pago() {
            Log.Debug("Se ha iniciado el proceso de actualizar los campos de pago a un clienteRenta");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string pagoQuery = @"update ClienteRenta
                                           set
                                               FechaUltimoPago=@FechaUltimoPago, MontoUltimoPago=@MontoUltimoPago,
                                               DeudaCliente=@DeudaCliente
                                           where IdClienteRenta=@IdClienteRenta;";

                await using var command = new MySqlCommand(pagoQuery, connection);

                command.Parameters.AddWithValue("@MontoUltimoPago",
                    MontoUltimoPago.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@FechaUltimoPago",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                command.Parameters.AddWithValue("@DeudaCliente",
                    DeudaCliente.ToString(CultureInfo.InvariantCulture));

                command.Parameters.AddWithValue("@IdClienteRenta",
                    Id.ToString());

                var res = await ExecSql.NonQuery(command, "Alta Pago Cliente Renta");
                Log.Debug("Se han actualizado los datos del Cliente Renta por una renta");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de actualizar los datos del clienteRenta en el pago.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al actualizar los datos del cliente Renta en el pago. Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}