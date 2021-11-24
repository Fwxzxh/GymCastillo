using System;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes {

    /// <summary>
    /// Clase que contiene los campos y métodos del objeto Cliente
    /// </summary>
    public class Cliente : AbstClientInstructor {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Si el cliente tiene alguna condición especial.
        /// </summary>
        public bool CondicionEspecial { get; set; }

        /// <summary>
        /// Indica si el cliente esta activo.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// La fecha en la que se vence el pago del cliente.
        /// </summary>
        public DateTime FechaVencimientoPago { get; set; }

        /// <summary>
        /// La deuda actual del cliente.
        /// </summary>
        public decimal DeudaCliente { get; set; }

        /// <summary>
        /// Medio por el que el cliente conoció el gym.
        /// </summary>
        public string MedioConocio { get; set; }

        /// <summary>
        /// La cantidad total de clases disponibles del cliente
        /// </summary>
        public int ClasesTotalesDisponibles { get; set; }

        /// <summary>
        /// La cantidad de clases disponibles del cliente en la semana actual.
        /// </summary>
        public int ClasesSemanaDisponibles { get; set; }

        /// <summary>
        /// La cantidad del descuento aplicado al cliente.
        /// </summary>
        public decimal Descuento { get; set; }

        /// <summary>
        /// Indica si el usuario es un niño o no.
        /// </summary>
        public bool Niño { get; set; }

        /// <summary>
        /// El id del paquete asignado al cliente.
        /// </summary>
        public int IdPaquete { get; set; }

        /// <summary>
        /// Nombre del paquete del usuario.
        /// </summary>
        public string NombrePaquete { get; set; }

        /// <summary>
        /// Id del tipo de cliente.
        /// </summary>
        public int IdTipoCliente { get; set; }

        /// <summary>
        /// Nombre el tipo de cliente.
        /// </summary>
        public string NombreTipoCliente { get; set; }

        /// <summary>
        /// El id del Locker
        /// </summary>
        public int IdLocker { get; set; }

        /// <summary>
        /// Locker asignado a el cliente.
        /// </summary>
        public string NombreLocker { get; set; }

        /// <summary>
        /// Método que Actualiza la instancia actual del cliente en la Base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas en la bd.</returns>
        public override async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update de un objeto tipo Cliente.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();

                const string updateQuery = @"UPDATE cliente
                                             SET Domicilio=@Domicilio, Telefono=@Telefono, CondicionEspecial=@CondicionEspecial,
                                                 NombreContacto=@NombreContacto, TelefonoContacto=@TelefonoContacto, Foto=@Foto,
                                                 Activo=@Activo, MedioConocio=@MedioConocio, Descuento=@Descuento, Nino=@Nino,
                                                 IdTipoCliente=@IdTipoCliente, IdPaquete=@IdPaquete
                                             WHERE IdCliente=@IdCliente";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@IdCliente", Id.ToString());

                command.Parameters.AddWithValue("@Domicilio", Domicilio);
                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@CondicionEspecial", Convert.ToInt32(CondicionEspecial).ToString());

                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);
                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", Foto); // TODO: Abr k pdo con esto

                command.Parameters.AddWithValue("@Activo", Convert.ToInt32(Activo).ToString());
                command.Parameters.AddWithValue("@MedioConocio", MedioConocio);
                command.Parameters.AddWithValue("@Descuento", Descuento.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@Nino", Convert.ToInt32(Niño).ToString());

                command.Parameters.AddWithValue("@IdTipoCliente", IdTipoCliente.ToString());
                command.Parameters.AddWithValue("@IdPaquete", IdPaquete.ToString());

                var res = await ExecSql.NonQuery(command, "Update Cliente");

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
        /// Método que borra (desactiva) la instancia actual del cliente en la Base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas en la bd.</returns>
        public override async Task<int> Delete() {
            // checamos si esta activo y si si hacemos query para cambiar el status de activo y cambiamos el status de activo en la instancia.
            // si ya esta inactivo hacemos la query para borrarlo.
            Log.Debug("Se ha iniciado el proceso de Delete en cliente.");
            if (Activo == false) {
                // eliminamos
                try {
                    await using var connection = new MySqlConnection(GetInitData.ConnString);
                    await connection.OpenAsync();

                    const string deleteQuery = @"delete from cliente where IdCliente=@IdCliente";

                    await using var command = new MySqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@IdCliente", Id.ToString());

                    var res = await ExecSql.NonQuery(command, "Delete Cliente");
                    Log.Debug("Se ha eliminado un cliente de la tabla.");
                    return res;
                }
                catch (Exception e) {
                    Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete del cliente.");
                    Log.Error($"Error: {e.Message}");
                    ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                        "Error desconocido");
                    return 0;
                }
            }
            else {
                // desactivamos
                try {
                    await using var connection = new MySqlConnection(GetInitData.ConnString);
                    await connection.OpenAsync();

                    const string deleteQuery = @"update cliente set Activo=false where IdCliente=@IdCliente";

                    await using var command = new MySqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@IdCliente", Id.ToString());

                    var res = await ExecSql.NonQuery(command, "Update Cliente");

                    // Desactivamos la instancia actual
                    Activo = false;

                    Log.Debug("Se ha desactivado un cliente de la tabla.");

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

        /// <summary>
        /// Método que da de alta la instancia actual del cliente en la base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas de la bd.</returns>
        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta un cliente.");
            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                // TODO: ver como manejar el insert simultaneo de locker.
                const string altaQuery = @"INSERT INTO cliente 
                                           VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno, 
                                           	@Domicilio, @FechaNacimiento, @Telefono, @CondicionEspecial, 
                                           	@NombreContacto, @TelefonoContacto, @Foto, @FechaUltimoAcceso, 
                                           	@MontoUltimoPago, @Activo, @FechaVencimientoPago, @DeudaCliente, 
                                           	@MedioConocio, @ClasesTotalesDisponibles, @ClasesSemanaDisponibles, 
                                           	@Descuento, @Nino, @IdTipoCliente, @IdPaquete)";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@Nombre", Nombre);
                command.Parameters.AddWithValue("@ApellidoPaterno", ApellidoPaterno);
                command.Parameters.AddWithValue("@ApellidoMaterno", ApellidoMaterno);

                command.Parameters.AddWithValue("@Domicilio", Domicilio);
                command.Parameters.AddWithValue("@FechaNacimiento", FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@CondicionEspecial", Convert.ToInt32(CondicionEspecial).ToString());

                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);
                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", null); // TODO: pendiente
                command.Parameters.AddWithValue("@FechaUltimoAcceso", FechaUltimoAcceso.ToString("yyyy-MM-dd HH:mm:ss"));

                command.Parameters.AddWithValue("@MontoUltimoPago", MontoUltimoPago.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@Activo", "1"); // Siempre True al dar de alta.
                command.Parameters.AddWithValue("@FechaVencimientoPago", FechaVencimientoPago.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@DeudaCliente", DeudaCliente.ToString(CultureInfo.InvariantCulture));

                command.Parameters.AddWithValue("@MedioConocio", MedioConocio);
                command.Parameters.AddWithValue("@ClasesTotalesDisponibles", ClasesTotalesDisponibles.ToString());
                command.Parameters.AddWithValue("@ClasesSemanaDisponibles", ClasesSemanaDisponibles.ToString());

                command.Parameters.AddWithValue("@Descuento", Descuento.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@Nino", Convert.ToInt32(Niño).ToString());
                command.Parameters.AddWithValue("@IdTipoCliente", IdTipoCliente.ToString());
                command.Parameters.AddWithValue("@IdPaquete", IdPaquete.ToString());

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Alta Cliente");
                Log.Debug("Se ha dado de alta un cliente.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta el cliente.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que se encarga de dar de alta una nueva asistencia a la instancia actual.
        /// </summary>
        /// <returns>La Cantidad de Columnas afectadas en la bd.</returns>
        public override Task<int> NuevaAsistencia() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que se encarga de actualizar el pago del objeto actual en la base de datos
        /// </summary>
        /// <param name="cantidad"></param>
        public override void Pago(decimal cantidad) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que obtiene el horario de la instancia del cliente y lo da en un string.
        /// </summary>
        /// <returns>El horario en un string.</returns>
        public override string GetHorarioStr() {
            throw new NotImplementedException();
        }
    }
}