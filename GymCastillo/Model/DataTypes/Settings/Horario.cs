using System;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.Settings {
    /// <summary>
    /// Clase que tiene los campos y métodos de los objetos tipo Horarios.
    /// </summary>
    public class Horario : AbstOtrosTipos {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Id del horario
        /// </summary>
        public int IdHorario { get; set; }

        /// <summary>
        /// Id de la clase a la que pertenece.
        /// </summary>
        public int IdClase { get; set; }

        /// <summary>
        /// Dia en el que ocurre la clase.
        /// </summary>
        public int Dia { get; set; }

        /// <summary>
        /// Hora de inicio de la clase.
        /// </summary>
        public DateTime HoraInicio { get; set; }

        /// <summary>
        /// Hora final de la clase.
        /// </summary>
        public DateTime HoraFin { get; set; }

        /// <summary>
        /// El Cupo actual de la clase.
        /// </summary>
        public int CupoActual { get; set; }

        /// <summary>
        /// Da el número de clientes registrados a esta clase.
        /// </summary>
        public int NumRegistrados {
            get {
                var num = InitInfo.ObCoClienteHorario
                    .Count(x => x.IdHorario == IdHorario);
                return num;
            }
        }

        public override async Task<int> Update() {
            Log.Warn("Se ha iniciado el proceso de update de un horario.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string updateQuery = @"UPDATE horario
                                             SET Dia=@Dia, HoraInicio=@HoraInicio,
                                                 HoraFin=@HoraFin, IdClase=@IdClase
                                             WHERE IdHorario=@IdHorario";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@IdHorario", IdHorario.ToString());
                command.Parameters.AddWithValue("@Dia", Dia.ToString());
                command.Parameters.AddWithValue("@HoraInicio", HoraInicio.ToString("HHmm"));
                command.Parameters.AddWithValue("@HoraFin", HoraFin.ToString("HHmm"));
                command.Parameters.AddWithValue("@IdClase", IdClase.ToString());

                Log.Debug("Se ha creado la query.");

                var res =await ExecSql.NonQuery(command, "Update Horarios");
                Log.Debug("Se ha editado un horario.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de actualizar el horario.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de delete de un horario.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from horario where IdHorario=@IdHorario";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdHorario", IdHorario.ToString());

                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Delete Horarios");
                Log.Debug("Se ha eliminado un horario de la tabla.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete del horario.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta un horario.");

            if (HoraInicio.TimeOfDay > HoraFin.TimeOfDay) {
                ShowPrettyMessages.ErrorOk(
                    "La hora de fin no puede ser menor que la hora de inicio.",
                    "Horario Invalido");
                return 0;
            }

            if (HoraInicio.TimeOfDay == HoraFin.TimeOfDay) {
                ShowPrettyMessages.ErrorOk(
                    "La hora de inicio y la hora de fin no pueden ser la misma hora.",
                    "Horario Invalido");
                return 0;
            }

            var chocan = false;

            foreach (var horario in InitInfo.ObCoHorarios) {
                if (horario.Dia != Dia) continue;
                Log.Debug($"{horario.HoraInicio.TimeOfDay.ToString()}:{HoraInicio.TimeOfDay.ToString()} {horario.HoraFin.TimeOfDay.ToString()}:{HoraFin.TimeOfDay.ToString()}");
                if (HoraInicio.TimeOfDay == horario.HoraInicio.TimeOfDay && HoraFin.TimeOfDay == horario.HoraFin.TimeOfDay) {
                    chocan = true;
                }

                if (HoraInicio.TimeOfDay > horario.HoraInicio.TimeOfDay && HoraInicio.TimeOfDay < horario.HoraFin.TimeOfDay) {
                    chocan = true;
                }
                if (HoraFin.TimeOfDay > horario.HoraInicio.TimeOfDay && HoraFin.TimeOfDay < horario.HoraFin.TimeOfDay) {
                    chocan = true;
                }
            }

            if (chocan) {
                ShowPrettyMessages.ErrorOk(
                    "No se pudo dar de alta este horario ya que este coincide en tiempo y lugar con otra clase.",
                    "Lugar ocupado a esta hora.");
                return 0;
            }

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"INSERT INTO horario
                                           VALUES 
                                               (default, @Dia, @HoraInicio,
                                               @HoraFin, @CupoActual, @IdClase);";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@Dia", Dia.ToString());
                command.Parameters.AddWithValue("@HoraInicio", HoraInicio.ToString("HHmm"));
                command.Parameters.AddWithValue("@HoraFin", HoraFin.ToString("HHmm"));

                command.Parameters.AddWithValue("@CupoActual", CupoActual.ToString());
                command.Parameters.AddWithValue("@IdClase", IdClase.ToString());

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Alta Horarios");
                Log.Debug("Se ha dado de alta un horario.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta un horario.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que actualiza la cantidad del cupo actual.
        /// </summary>
        public async Task<int> NuevaAsistencia() {
            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string query = @"update horario set CupoActual=@CupoActual where IdHorario=@IdHorario";

                await using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@IdHorario",
                    IdHorario.ToString());
                command.Parameters.AddWithValue("@CupoActual",
                    (CupoActual + 1).ToString());

                Log.Debug("Se ha creado la query.");

                var res =await ExecSql.NonQuery(command, "Update Cupo horario");
                Log.Debug("Se ha actualizado el cupo en horario.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el update del cupo.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que resetea el cupo actual del horario actual.
        /// </summary>
        public void ResetAsistencia() {
            throw new NotImplementedException();
        }
    }
}
