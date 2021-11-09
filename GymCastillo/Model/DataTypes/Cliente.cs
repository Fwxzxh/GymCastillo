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
    public class Cliente : AbstClientInstructor{
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

                // TODO: Hacer la query de verdad.
                const string updateQuery = @"update cliente set 
                                    Nombre=@Nombre, ApellidoPaterno=@APaterno, ApellidoMaterno=@AMaterno, 
                                    FechaNacimiento=@NacFecha, Telefono=@Tel, CondicionEspecial=@Cond, 
                                    NombreContacto=@NombreContacto, TelefonoContacto=@TelContacto, 
                                    -- Foto=@Foto,
                                    FechaUltimoAcceso=@FechaUltimoAcceso, MontoUltimoPago=@Monto,
                                    Activo=@Act, Asistencias=@Asistencias,
                                    IdTipoCliente=@IdTipoCliente, DeudaCliente=@DeudaCliente
                                    where IdCliente=@Id";

                await using var command = new MySqlCommand(updateQuery, connection);

                // TODO: Agregar los campos que faltan.
                command.Parameters.AddWithValue("@Id", Id.ToString());
                command.Parameters.AddWithValue("@Nombre", Nombre);
                command.Parameters.AddWithValue("@APaterno", ApellidoPaterno);
                command.Parameters.AddWithValue("@AMaterno", ApellidoMaterno);
                command.Parameters.AddWithValue("@NacFecha",
                    FechaNacimiento.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@Tel", Telefono);
                command.Parameters.AddWithValue("@Cond", CondicionEspecial.ToString());
                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);
                command.Parameters.AddWithValue("@TelContacto", TelefonoContacto);
                //command.Parameters.AddWithValue("@Foto", Foto); TODO: Abr k pdo con esto
                command.Parameters.AddWithValue("@FechaUltimoAcceso",
                    FechaUltimoAcceso.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@Monto",
                    MontoUltimoPago.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@Act", Activo.ToString());
                command.Parameters.AddWithValue("@Asistencias", Asistencias);
                command.Parameters.AddWithValue("@IdTipoCliente", IdTipoCliente.ToString());
                command.Parameters.AddWithValue("@DeudaCliente",
                    DeudaCliente.ToString(CultureInfo.InvariantCulture));

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
        public override int Delete() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de alta la instancia actual del cliente en la base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas de la bd.</returns>
        public override int Alta() {
            throw new NotImplementedException();
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
        public override void AltaClase(Clase clase) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de baja una lcase al cliente que corresponde a la insancia actual.
        /// </summary>
        /// <param name="clase"></param>
        public override void BajaClase(Clase clase) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que guarda una nueva asistencia al cliente de la instancia actual.
        /// </summary>
        /// <param name="fecha">La fecha de la asistencia</param>
        // TODO: ver que onda con las asistencias para ver si son al entrar al gym o por clase.
        public override void NuevaAsistencia(DateTime fecha) {
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