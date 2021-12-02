using System;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que tiene los campos y métodos de los objetos tipo Horario.
    /// </summary>
    public class Horario : IOtrosTipos {
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

        public async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update de un horario.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string updateQuery = @"UPDATE horario
                                             SET Dia=@Dia, HoraInicio=@HoraInicio,
                                                 HoraFin=@HoraFin, IdClase=@IdClase
                                             WHERE IdHorario=@IdHorario";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@Dia", Dia.ToString());
                command.Parameters.AddWithValue("@HoraInicio", HoraInicio.ToString("HHmm"));
                command.Parameters.AddWithValue("@HoraFin", HoraFin.ToString("HHmm"));
                command.Parameters.AddWithValue("@IdClase", IdClase.ToString());

                Log.Debug("Se ha creado la query.");

                var res =await ExecSql.NonQuery(command, "Update Horario");
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

        public async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de delete de un horario.");
            // TODO: Hacer FK check

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from horario where IdHorario=@IdHorario";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdHorario", IdHorario.ToString());

                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Delete Horario");
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

        public async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta un horario.");

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

                var res = await ExecSql.NonQuery(command, "Alta Horario");
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
        public void NuevaAsistencia() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que resetea el cupo actual del horario actual.
        /// </summary>
        public void ResetAsistencia() {
            throw new NotImplementedException();
        }
    }
}
