using System;
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
        /// Id del tipo de cliente.
        /// </summary>
        public int IdTipoCliente { get; set; }

        /// <summary>
        /// Nombre el tipo de cliente.
        /// </summary>
        public string NombreTipoCliente { get; set; }

        /// <summary>
        /// La deuda actual del cliente.
        /// </summary>
        public decimal DeudaCliente { get; set; }

        /// <summary>
        /// Las últimas asistencias del cliente.
        /// </summary>
        // TODO: ver como manejar esto en la bd.
        public string Asistencias { get; set; }

        /// <summary>
        /// La fecha en la que se vence el pago del cliente.
        /// </summary>
        public DateTime FechaVencimientoPago { get; set; }

        /// <summary>
        /// Monto del último pago del cliente.
        /// </summary>
        public decimal MontoUltimoPago { get; set; }

        /// <summary>
        /// Indica si el cliente esta activo.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Medio por el que el cliente conoció el gym.
        /// </summary>
        public string MedioConocio { get; set; }

        /// <summary>
        /// Locker asignado a el cliente.
        /// </summary>
        public string Locker { get; set; }

        /// <summary>
        /// Método que Actualiza la instancia actual del cliente en la Base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas en la bd.</returns>
        public override async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update de un objeto tipo Cliente.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();

                const string updateQuery = @"update cliente set 
                                                Telefono=@Telefono,
                                                CondicionEspecial=@CondicionEspecial,
                                                NombreContacto=@NombreContacto, 
                                                TelefonoContacto=@TelefonoContacto, 
                                                IdTipoCliente=@IdTipoCliente, Activo=@Activo,
                                                Domicilio=@domicilio
                                            where IdCliente=@IdCliente";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@IdCliente", Id.ToString());
                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@CondicionEspecial", CondicionEspecial.ToString());
                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);
                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                //command.Parameters.AddWithValue("@Foto", Foto); TODO: Abr k pdo con esto
                command.Parameters.AddWithValue("@IdTipoCliente", IdTipoCliente.ToString());
                command.Parameters.AddWithValue("@Activo", Activo.ToString());
                command.Parameters.AddWithValue("@Domicilio", Domicio);

                var res = ExecSql.NonQuery(command, "Update Cliente").Result;

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconcoido a la hora de hacer update.");
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
            // checamos si esta activo y si si hacemos querry para cambiar el status de activo y cambiamos el status de activo en la instancia.
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

                    var res = ExecSql.NonQuery(command, "Delete Cliente").Result;
                    Log.Debug("Se ha eliminado un cliente de la tabla.");
                    return res;
                }
                catch (Exception e) {
                    Log.Error("Ha ocurrido un error desconcoido a la hora de hacer el delete del cliente.");
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

                    var res = ExecSql.NonQuery(command, "Update Cliente").Result;

                    // Desactivamos la instancia actual
                    Activo = false;

                    Log.Debug("Se ha desactivado un cliente de la tabla.");

                    return res;
                }
                catch (Exception e) {
                    Log.Error("Ha ocurrido un error desconcoido a la hora de desactivar el cliente.");
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
            // se actualizan todos los datos principales (de contacto)
            Log.Debug("Se ha iniciado el proceso de dar de alta un cliente.");
            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();

                const string altaQuery = @"INSERT INTO cliente
                                           VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Domicilio,
                                                   @FechaNacimiento, @Telefono, @CondicionEspecial, @NombreContacto,
                                                   @TelefonoContacto, @Foto, @FechaUltimoAcceso, @MontoUltimoPago,
                                                   @Activo, @Asistencias, @FechaVencimientoPago, @IdTipoCliente,
                                                   @DeudaCliente, @MedioConocido, @Locker);";
                // Se actualizan:
                // Nombre, ApellidoPaterno, ApellidoMaterno, Domicilio,
                // FechaNacimiento, Telefono, CondicionEspecial, NombreContacto,
                // TelefonoContacto, Foto, FechaUltimoAcceso, MontoUltimoPago,
                // Activo, Asitencias, FechaVencimientoPago, IdTipoCliente,
                // DeudaCliente, MedioConocido, Locker

                await using var command = new MySqlCommand(altaQuery, connection);
                command.Parameters.AddWithValue("@Nombre", Nombre);
                command.Parameters.AddWithValue("@ApellidoPaterno", ApellidoPaterno);
                command.Parameters.AddWithValue("@ApellidoMaterno", ApellidoMaterno);
                command.Parameters.AddWithValue("@Domicilio", Domicio);

                command.Parameters.AddWithValue("@FechaNacimiento", FechaNacimiento.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@CondicionEspecial", CondicionEspecial.ToString());
                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);

                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                //command.Parameters.AddWithValue("@Foto", Foto); TODO: pendiente
                command.Parameters.AddWithValue("@FechaUltimoAcceso", FechaUltimoAcceso.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@MontoUltimoPago", MontoUltimoPago.ToString(CultureInfo.InvariantCulture));

                command.Parameters.AddWithValue("@Activo", true.ToString()); // True al dar de alta.
                command.Parameters.AddWithValue("@Asistencias", ""); // Vacias porque es nuevo
                command.Parameters.AddWithValue("@FechaVencimientoPago", FechaVencimientoPago.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@IdTipoCliente", IdTipoCliente.ToString());

                command.Parameters.AddWithValue("@DeudaCliente", DeudaCliente.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@MedioConocio", MedioConocio);
                command.Parameters.AddWithValue("Locker", Locker);

                var res = ExecSql.NonQuery(command, "Alta Cliente").Result;
                Log.Debug("Se ha dado de alta un cliente.");
                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconcoido a la hora de desactivar el cliente.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que obtiene el horario de la instancia del cliente y lo da en un string.
        /// </summary>
        /// <returns>El horario en un string.</returns>
        public override string GetHorarioStr() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de alta una clase al cliente que corresponde a la instancia actual.
        /// </summary>
        /// <param name="clase"></param>
        public override async Task<int> AltaClase(Clase clase) {
            Log.Debug("Se ha iniciado el proceso de alta de clase.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();

                const string altaClaseQuery = @"INSERT INTO clienteclase VALUES (@IdCliente, @IdClase);";

                await using var command = new MySqlCommand(altaClaseQuery, connection);

                command.Parameters.AddWithValue("@IdCliente", Id.ToString());
                command.Parameters.AddWithValue("@IdClase", clase.IdClase.ToString());

                var res = ExecSql.NonQuery(command, "Alta Clase a Cliente").Result;

                Log.Debug("Se ha dado de alta un cliente.");
                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconcoido a la hora de dar de alta una clase a un cliente.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que da de baja una lcase al cliente que corresponde a la insancia actual.
        /// </summary>
        /// <param name="clase"></param>
        public override Task<int> BajaClase(Clase clase) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que guarda una nueva asistencia al cliente de la instancia actual.
        /// </summary>
        /// <param name="fecha">La fecha de la asistencia</param>
        // TODO: ver que onda con las asistencias para ver si son al entrar al gym o por clase.
        public override Task<int> NuevaAsistencia(DateTime fecha) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que cobra la cantidad correspondiente a el tipo de usuario
        /// o una cantidad dada al cliente de la instancia actual.
        /// </summary>
        public void Cobrar() {
            throw new NotImplementedException();
        }
    }
}