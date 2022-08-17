using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using ImageMagick;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.Personal {

    /// <summary>
    /// Clase que contiene los campos y métodos del objeto Cliente
    /// </summary>
    public class Cliente : AbstClientInstructor {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Retorna la ruta al directorio personal del cliente
        /// </summary>
        public string ClienteDir =>
            $"C:/GymCastillo/Clientes/{Id.ToString()}-{ApellidoPaterno}-{Nombre.Split(" ").First()}/";

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
        /// El Id del chat de este cliente
        /// </summary>
        public string ChatId { get; set; }

        /// <summary>
        /// La fecha de registro del cliente
        /// </summary>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Array de bytes de la firma.
        /// </summary>
        private byte[] firma = Array.Empty<byte>();

        /// <summary>
        /// Firma del usuario en formato para manipular.
        /// </summary>
        public MagickImage Firma {
            get => new(firma);
            set {
                // Creamos la geometría con el tamaño deseado.
                var size = new MagickGeometry(355, 355) {
                    IgnoreAspectRatio = true
                };

                // La ponemos en el tamaño adecuado
                value.Resize(size);
                var bytes = value.ToByteArray();

                // Verificamos que la imagen pueda caber en la base de datos y si no bajamos la calidad.
                if (bytes.Length >= 64000) {
                    // Quality base 75
                    value.Quality = 60;
                }

                // Si sigue demasiado grande mandamos un mensaje.
                if (bytes.Length >= 64000) {
                    ShowPrettyMessages.WarningOk(
                        "Esta imagen es demasiado grande para ser guardada en la base de datos, elija otra o comprímala.",
                        "Imagen Demasiado Grande");
                    return;
                }

                firma = value.ToByteArray();
            }
        }

        /// <summary>
        /// Firma raw en un array de bytes.
        /// </summary>
        public byte[] FirmaRaw {
            get => firma;
            set => firma = value;
        }

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
                                             SET Nombre=@Nombre, ApellidoPaterno=@ApellidoPaterno, ApellidoMaterno=@ApellidoMaterno, 
                                                 Telefono=@Telefono, CondicionEspecial=@CondicionEspecial, DeudaCliente=@DeudaCliente,
                                                 DescripcionCondicionEspecial=@DescripcionCondicionEspecial, NombreContacto=@NombreContacto, 
                                                 TelefonoContacto=@TelefonoContacto, Foto=@Foto, Activo=@Activo, MedioConocio=@MedioConocio, 
                                                 DuracionPaquete=@DuracionPaquete, Nino=@Nino,
                                                 IdTipoCliente=@IdTipoCliente, Firma=@Firma
                                             WHERE IdCliente=@IdCliente;";


                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@IdCliente", Id.ToString());

                command.Parameters.AddWithValue("@Nombre", Nombre);
                command.Parameters.AddWithValue("@ApellidoPaterno", ApellidoPaterno);
                command.Parameters.AddWithValue("@ApellidoMaterno", ApellidoMaterno);

                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@CondicionEspecial", Convert.ToInt32(CondicionEspecial).ToString());
                command.Parameters.AddWithValue("@DeudaCliente", DeudaCliente.ToString(CultureInfo.InvariantCulture));

                command.Parameters.AddWithValue("@DescripcionCondicionEspecial", DescripciónCondiciónEspecial);

                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);
                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", FotoRaw);

                command.Parameters.AddWithValue("@Activo", Convert.ToInt32(Activo).ToString());
                command.Parameters.AddWithValue("@MedioConocio", MedioConocio);
                command.Parameters.AddWithValue("@DuracionPaquete", DuraciónPaquete.ToString());
                command.Parameters.AddWithValue("@Nino", Convert.ToInt32(Niño).ToString());

                command.Parameters.AddWithValue("@IdTipoCliente", IdTipoCliente.ToString());
                command.Parameters.AddWithValue("@Firma", FirmaRaw);

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
        private bool CheckDeleteConstrains(bool batch = false) {
            // Checamos que podamos eliminarlo
            if (DeudaCliente > 0 ) {
                if (!batch) {
                    ShowPrettyMessages.InfoOk(
                        $"No es posible eliminar el cliente {Id.ToString()} {Nombre} {ApellidoPaterno} ya que tiene una deuda actual de $ {DeudaCliente.ToString(CultureInfo.InvariantCulture)}",
                        "Cliente con deuda");
                    
                }
                return false;
            }

            // Eliminamos el check de las clases disponibles
            // if (ClasesTotalesDisponibles > 0) {
            //     ShowPrettyMessages.InfoOk(
            //         $"No es posible eliminar el cliente {Id.ToString()} {Nombre} {ApellidoPaterno} ya que tiene {ClasesTotalesDisponibles.ToString()} clases disponibles.",
            //         "Cliente con clases disponibles.");
            //     return false;
            // }

            // Checamos si no hay ingresos para este cliente.
            for (var index = 0; index < InitInfo.ObCoIngresos.Count; index++) {
                var x = InitInfo.ObCoIngresos[index];

                if (x.IdCliente == Id) {
                    if (!batch) {
                        ShowPrettyMessages.InfoOk(
                            $"No es posible eliminar el cliente {Id.ToString()} {Nombre} {ApellidoPaterno} ya que tiene ingresos registrados y al eliminarlo se perdería la información sobre sus ingresos.",
                            "Cliente con Ingresos registrados.");
                    }
                    return false;
                }
            }

            return true;
        }

        
        /// <summary>
        /// Borra (desactiva) varias instancias de clientes cuya fecha de ultimo acceso haya sido hace más de 4
        /// meses por default
        /// </summary>
        /// <param name="dias">dias de inactividad para desactivar por default 120 días</param>
        /// <returns>El número de clientes desactivados o eliminados</returns>
        public static async Task BatchDelete(int dias = 120) {
            Log.Debug("Se ha iniciado el proceso de Batch Delete de los clientes");
            
            var fechaLimite = DateTime.Today - TimeSpan.FromDays(120); // 120 dias = ~4 meses, pero puede ser configurable
            
            // Obtenemos los clientes cuya fecha de ultimo acceso sea hace más de 4 meses, no tengan deudas y sean activos
            var clientes =
                InitInfo.ObCoClientes.Where(x => x.FechaUltimoAcceso > fechaLimite && x.DeudaCliente == 0 && x.Activo);


            try {
                // Desactivamos todos los clientes
                var countDeactivated = 0;
                foreach (var cliente in clientes) {
                    await using var connection = new MySqlConnection(GetInitData.ConnString);
                    await connection.OpenAsync();

                    const string deleteQuery = @"update cliente set Activo=false where IdCliente=@IdCliente";

                    await using var command = new MySqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@IdCliente", cliente.Id.ToString());

                    var res = await ExecSql.NonQuery(command, "Update Cliente");

                    Log.Debug("Se ha desactivado un cliente de la tabla.");

                    countDeactivated++;
                }
                ShowPrettyMessages.NiceMessageOk($"Se han desactivado {countDeactivated} clientes",
                    "Clientes desactivados");
                
                // eliminamos los que podamos eliminar
                var countDeleted = 0;
                foreach (var cliente in clientes) {
                    if (!cliente.CheckDeleteConstrains(batch:true)) continue; // Si se puede eliminar eliminamos
                    await using var connection = new MySqlConnection(GetInitData.ConnString);
                    await connection.OpenAsync();

                    const string deleteQuery = @"delete from cliente where IdCliente=@IdCliente";

                    await using var command = new MySqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@IdCliente", cliente.Id.ToString());

                    var res = await ExecSql.NonQuery(command, "Delete Cliente");
                    Log.Debug("Se ha eliminado un cliente de la tabla.");
                    countDeleted++;
                }
                ShowPrettyMessages.NiceMessageOk($"Se han eliminado {countDeleted} clientes",
                    "Clientes eliminados");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete de los clientes.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
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
                                                Nino, IdTipoCliente, FechaRegistro, Firma)
                                           VALUES
                                               (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
                                                @FechaNacimiento, @Telefono, @CondicionEspecial,
                                                @DescripcionCondicionEspecial, @NombreContacto,
                                                @TelefonoContacto, @Foto, @Activo, @MedioConocio,
                                                @Nino, @IdTipoCliente, @FechaRegistro, @Firma)";

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

                command.Parameters.AddWithValue("@FechaRegistro",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Firma", FirmaRaw);

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
        /// Método que se encarga de registrar los campos de una nueva asistencia.
        /// </summary>
        /// <returns>La Cantidad de Columnas afectadas en la bd.</returns>
        public async Task<int> NuevaAsistencia(int numClasesAEntrar) {
            Log.Debug("Se ha empezado el proceso de dar de alta una nueva asistencia en Cliente.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string asistenciaQuery = @"UPDATE cliente
                                                     	SET FechaUltimoAcceso=@FechaUltimoAcceso, 
                                                     	ClasesTotalesDisponibles=@ClasesTotalesDisponibles, 
                                                     	ClasesSemanaDisponibles=@ClasesSemanaDisponibles
                                                     	WHERE IdCliente=@IdCliente";

                await using var command = new MySqlCommand(asistenciaQuery, connection);

                command.Parameters.AddWithValue("@FechaUltimoAcceso",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@ClasesTotalesDisponibles",
                    (ClasesTotalesDisponibles - numClasesAEntrar).ToString());
                command.Parameters.AddWithValue("@ClasesSemanaDisponibles",
                    (ClasesSemanaDisponibles - numClasesAEntrar).ToString());

                command.Parameters.AddWithValue("@IdCliente", Id.ToString());

                Log.Debug("Se ha creado la query.");

                var res =await ExecSql.NonQuery(command, "Nueva Asistencia Cliente");
                Log.Debug("Se ha registrado la asistencia de un cliente.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de registrar la asistencia del cliente.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Pago() {
            Log.Debug("Se ha iniciado el proceso de actualizar los campos del pago.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string pagoQuery = @"update cliente
                                           set
                                               MontoUltimoPago=@MontoUltimoPago, FechaUltimoPago=@FechaUltimoPago,
                                               FechaVencimientoPago=@FechaVencimientoPago, DeudaCliente=@DeudaCliente,
                                               ClasesTotalesDisponibles=@ClasesTotalesDisponibles, 
                                               ClasesSemanaDisponibles=@ClasesSemanaDisponibles,
                                               DuracionPaquete=@DuracionPaquete, IdLocker=@IdLocker, IdPaquete=@IdPaquete
                                           where IdCliente=@IdCliente";

                await using var command = new MySqlCommand(pagoQuery, connection);

                command.Parameters.AddWithValue("@MontoUltimoPago",
                    MontoUltimoPago.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@FechaUltimoPago",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                command.Parameters.AddWithValue("@FechaVencimientoPago",
                    FechaVencimientoPago.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@DeudaCliente",
                    DeudaCliente.ToString(CultureInfo.InvariantCulture));

                command.Parameters.AddWithValue("@ClasesTotalesDisponibles",
                    ClasesTotalesDisponibles.ToString());
                command.Parameters.AddWithValue("@ClasesSemanaDisponibles",
                    ClasesSemanaDisponibles.ToString());

                command.Parameters.AddWithValue("@DuracionPaquete",
                    DuraciónPaquete.ToString());
                command.Parameters.AddWithValue("@IdLocker",
                    IdLocker == 0 ? null : IdLocker.ToString());
                command.Parameters.AddWithValue("@IdPaquete",
                    IdPaquete == 0 ? null : IdPaquete.ToString());

                command.Parameters.AddWithValue("@IdCliente",
                    Id.ToString());

                var res = await ExecSql.NonQuery(command, "Alta Pago Cliente");
                Log.Debug("Se han actualizado los datos del cliente por un pago.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de actualizar los datos del cliente en el pago.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al actualizar los datos del cliente en el pago. Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que Obtiene la lista de horarios a los que esta registrado el cliente
        /// </summary>
        /// <returns>La lista de objetos tipo horario del cliente</returns>
        public List<Horario> GetHorariosCliente() {

            // Obtenemos a que clases esta registrado este cliente.
            var horariosCliente =
                InitInfo.ObCoClienteHorario.Where(x => x.IdCliente == Id)
                    .Select(x => x.IdHorario);

            // Obtenemos los horarios del cliente
            var horarios =
                InitInfo.ObCoHorarios.Where(x => horariosCliente.Contains(x.IdHorario));

            return horarios.ToList();
        }
    }
}