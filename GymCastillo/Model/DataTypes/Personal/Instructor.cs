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
    /// Clase que contiene los campos y métodos del objeto Instructor
    /// </summary>
    public class Instructor : AbstClientInstructor {
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// La hora de entrada designada al instructor
        /// </summary>
        public DateTime HoraEntrada { get; set; }

        /// <summary>
        /// La hora de salida designada al instructor.
        /// </summary>
        public DateTime HoraSalida { get; set; }

        /// <summary>
        /// La cantidad de dias a trabajar del instructor.
        /// </summary>
        public int DiasATrabajar { get; set; }

        /// <summary>
        /// La cantidad actual de dias trabajados.
        /// </summary>
        public int DiasTrabajados { get; set; }

        /// <summary>
        /// El sueldo del Instructor.
        /// </summary>
        public decimal Sueldo { get; set; }

        /// <summary>
        /// La cantidad del sueldo a descontar por amonestaciones.
        /// </summary>
        public decimal SueldoADescontar { get; set; }

        /// <summary>
        /// El id del tipo de instructor.
        /// </summary>
        public int  IdTipoInstructor { get; set; }

        /// <summary>
        /// El nombre del tipo de instructor.
        /// </summary>
        public string NombreTipoInstructor { get; set; }

        /// <summary>
        /// La lista de clases las cuales estan asignadas al instructor (separados por comas)
        /// </summary>
        public string IdClase { get; set; }

        /// <summary>
        /// La lista de los nombres de las clases asignadas al instructor (separados por comas)
        /// </summary>
        public string NombreClases { get; set; }

        /// <summary>
        /// Indica cuanto dura su paquete Serían 3: 1-Semanal, 2-Quincenal, 3-Mes
        /// </summary>
        public int MétodoFechaPago { get; set; }

        /// <summary>
        /// Método que actualiza la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas.</returns>
        public override async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update de un objeto tipo Instructor.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string updateQuery = @"UPDATE instructor
                                             SET domicilio=@Domicilio, telefono=@Telefono, NombreContacto=@NombreContacto,
                                             telefonocontacto=@TelefonoContacto, foto=@Foto, HoraEntrada=@HoraEntrada,
                                             HoraSalida=@HoraSalida, Sueldo=@Sueldo, SueldoADescontar=@SueldoADescontar,
                                             IdTipoInstructor=@IdTipoInstructor
                                             WHERE idinstructor=@IdInstructor;";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@IdInstructor", Id.ToString());
                command.Parameters.AddWithValue("@Domicilio", Domicilio);
                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);

                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", null); // TODO: Ver que onda con la foto
                command.Parameters.AddWithValue("@HoraEntrada", HoraEntrada.ToString("HHmm"));

                command.Parameters.AddWithValue("@HoraSalida", HoraSalida.ToString("HHmm"));
                command.Parameters.AddWithValue("@Sueldo", Sueldo.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@SueldoADescontar", SueldoADescontar.ToString(CultureInfo.InvariantCulture));

                command.Parameters.AddWithValue("@IdTipoInstructor", IdTipoInstructor.ToString());

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Update Instructor");
                Log.Debug("Se ha actualizado un Instructor.");

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
        /// Método que borra la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas.</returns>
        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de delete en un Instructor.");

            // Fk key constraint check.
            if (InitInfo.ObCoClases.Any(x => x.IdInstructor == Id)) {
                // Este instructor esta dado asignado en alguna clase
                ShowPrettyMessages.InfoOk(
                    "Hay clases asignadas a este instructor, asi que no puedes eliminar al instructor, cambia esas clases a otro instructor para eliminarlo.",
                    "Fk key check.");
                return 0;
            }

            // TODO: ver k pedo con la fk de egresos.
            // if (InitInfo.Lis) {
            //
            // }


            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from instructor where IdInstructor=@IdInstructor";

                await using var command = new MySqlCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("@IdInstructor", Id.ToString());
                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Delete Instructor");
                Log.Debug("Se ha eliminado un Instructor de la tabla.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete del Instructor.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que da de alta la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas.</returns>
        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta un Instructor.");
            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"INSERT INTO instructor
                                           VALUES (default, @Nombre, @ApellidoPaterno, @ApellidoMaterno,
                                           	@Domicilio, @FechaNacimiento, @Telefono, @NombreContacto,
                                           	@TelefonoContacto, @Foto, @FechaUltimoAcceso, @FechaUltimoPago,
                                           	@MontoUltimoPago, @HoraEntrada, @HoraSalida, @DiasATrabajar,
                                           	@DiasTrabajados, @Sueldo, @SueldoADescontar, @MetodoFechaPago,
                                            @IdTipoInstructor);";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@Nombre", Nombre);
                command.Parameters.AddWithValue("@ApellidoPaterno", ApellidoPaterno);
                command.Parameters.AddWithValue("@ApellidoMaterno", ApellidoMaterno);
                command.Parameters.AddWithValue("@Domicilio", Domicilio);

                command.Parameters.AddWithValue("@FechaNacimiento", FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);

                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                command.Parameters.AddWithValue("@Foto", null); //TODO: pendiente
                command.Parameters.AddWithValue("@FechaUltimoAcceso", FechaUltimoAcceso.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@FechaUltimoPago", FechaUltimoPago.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@MontoUltimoPago", MontoUltimoPago.ToString(CultureInfo.InvariantCulture));

                command.Parameters.AddWithValue("@HoraEntrada", HoraEntrada.ToString("HHmm"));
                command.Parameters.AddWithValue("@HoraSalida", HoraSalida.ToString("HHmm"));
                command.Parameters.AddWithValue("@DiasATrabajar", DiasATrabajar.ToString());
                command.Parameters.AddWithValue("@DiasTrabajados", DiasTrabajados.ToString());

                command.Parameters.AddWithValue("@Sueldo", Sueldo.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@SueldoADescontar", SueldoADescontar.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@MetodoFechaPago", MétodoFechaPago.ToString());

                command.Parameters.AddWithValue("@IdTipoInstructor", IdTipoInstructor.ToString());


                Log.Debug("Se ha creado la query.");

                var res =await ExecSql.NonQuery(command, "Alta Instructor");
                Log.Debug("Se ha dado de alta un cliente.");

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
        /// Método que se encarga de dar de alta una nueva asistencia a la instancia actual.
        /// </summary>
        /// <returns>La Cantidad de Columnas afectadas en la bd.</returns>
        public override Task<int> NuevaAsistencia() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que se encarga de actualizar el pago del obtejo actual en la base de datos
        /// </summary>
        /// <param name="cantidad"></param>
        public override void Pago(decimal cantidad) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que obtiene el horario del Instructor de la instancia actual en un string
        /// </summary>
        /// <returns>Un string con el horario del instructor.</returns>
        public override string GetHorarioStr() {
            throw new NotImplementedException();
        }
    }
}