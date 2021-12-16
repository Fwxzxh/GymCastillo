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

namespace GymCastillo.Model.DataTypes.Personal {

    /// <summary>
    /// Clase que contiene los campos y métodos del objeto Cliente
    /// </summary>
    public class Cliente : AbstClientInstructor {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Retorna el directorio del cliente
        /// </summary>
        public string ClienteDir => $"C:/GymCastillo/Clientes/{Id.ToString()}-{ApellidoPaterno}/";

        /// <summary>
        /// Si el cliente tiene alguna condición especial.
        /// </summary>
        public bool CondicionEspecial { get; set; }

        /// <summary>
        /// La descripción de la condición del cliente.
        /// </summary>
        public string DescripciónCondiciónEspecial { get; set; }

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
        public int DuraciónPaquete { get; set; }

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
                                             SET Telefono=@Telefono, CondicionEspecial=@CondicionEspecial, 
                                                 DescripcionCondicionEspecial=@DescripcionCondicionEspecial,
                                                 NombreContacto=@NombreContacto, TelefonoContacto=@TelefonoContacto, Foto=@Foto,
                                                 Activo=@Activo, MedioConocio=@MedioConocio, DuracionPaquete=@DuracionPaquete, Nino=@Nino,
                                                 IdTipoCliente=@IdTipoCliente
                                             WHERE IdCliente=@IdCliente;";


                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@IdCliente", Id.ToString());

                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@CondicionEspecial", Convert.ToInt32(CondicionEspecial).ToString());

                command.Parameters.AddWithValue("@DescripcionCondicionEspecial", DescripciónCondiciónEspecial);

                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);
                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", FotoRaw);

                command.Parameters.AddWithValue("@Activo", Convert.ToInt32(Activo).ToString());
                command.Parameters.AddWithValue("@MedioConocio", MedioConocio);
                command.Parameters.AddWithValue("@DuracionPaquete", DuraciónPaquete.ToString());
                command.Parameters.AddWithValue("@Nino", Convert.ToInt32(Niño).ToString());

                command.Parameters.AddWithValue("@IdTipoCliente", IdTipoCliente.ToString());

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
        /// Método que checa si se puede eliminar el objeto en la base de datos.
        /// </summary>
        /// <returns>False si falla una validación.</returns>
        private bool CheckDeleteConstrains() {
            // Checamos que podamos eliminarlo
            if (DeudaCliente > 0) {
                ShowPrettyMessages.InfoOk(
                    $"No es posible eliminar este cliente ya que tiene una deuda actual de $ {DeudaCliente.ToString(CultureInfo.InvariantCulture)}",
                    "Cliente con deuda");
                return false;
            }

            if (ClasesTotalesDisponibles > 0) {
                ShowPrettyMessages.InfoOk(
                    $"No es posible eliminar este cliente ya que tiene {ClasesTotalesDisponibles.ToString()} clases disponibles.",
                    "Cliente con clases disponibles.");
                return false;
            }

            // Checamos si no hay ingresos para este cliente.
            for (var index = 0; index < InitInfo.ObCoIngresos.Count; index++) {
                var x = InitInfo.ObCoIngresos[index];

                if (x.IdCliente == Id) {
                    ShowPrettyMessages.InfoOk(
                        $"No es posible eliminar este cliente ya que tiene ingresos registrados y al eliminarlo se perdería la información sobre sus ingresos.",
                        "Cliente con Ingresos registrados.");
                    return false;
                }
            }

            return true;
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

                // Checamos si podemos eliminar
                if (!CheckDeleteConstrains()) {
                    return 0;
                }

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

                const string altaQuery = @"INSERT INTO cliente
                                               (IdCliente, Nombre, ApellidoPaterno, ApellidoMaterno,
                                                FechaNacimiento, Telefono, CondicionEspecial,
                                                DescripcionCondicionEspecial, NombreContacto,
                                                TelefonoContacto, Foto, Activo, MedioConocio,
                                                Nino, IdTipoCliente)
                                           VALUES
                                               (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
                                                @FechaNacimiento, @Telefono, @CondicionEspecial,
                                                @DescripcionCondicionEspecial, @NombreContacto,
                                                @TelefonoContacto, @Foto, @Activo, @MedioConocio,
                                                @Nino, @IdTipoCliente)";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@Nombre", Nombre);
                command.Parameters.AddWithValue("@ApellidoPaterno", ApellidoPaterno);
                command.Parameters.AddWithValue("@ApellidoMaterno", ApellidoMaterno);

                command.Parameters.AddWithValue("@FechaNacimiento",
                    FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@CondicionEspecial",
                    Convert.ToInt32(CondicionEspecial).ToString());

                command.Parameters.AddWithValue("@DescripcionCondicionEspecial",
                    DescripciónCondiciónEspecial);
                command.Parameters.AddWithValue("@NombreContacto",
                    NombreContacto);

                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", FotoRaw);
                command.Parameters.AddWithValue("@Activo", "1"); // Siempre True al dar de alta.
                command.Parameters.AddWithValue("@MedioConocio", MedioConocio);

                command.Parameters.AddWithValue("@Nino", Convert.ToInt32(Niño).ToString());
                command.Parameters.AddWithValue("@IdTipoCliente", IdTipoCliente.ToString());

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