using System;
using System.Threading.Tasks;
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
        public Dias Dia { get; set; }

        /// <summary>
        /// Hora de inicio de la clase.
        /// </summary>
        public TimeSpan HoraInicio { get; set; }

        /// <summary>
        /// Hora final de la clase.
        /// </summary>
        public TimeSpan HoraFin { get; set; }

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

                // command.Parameters.AddWithValue("@Dia", .ToString());
                // command.Parameters.AddWithValue("@HoraInicio", HoraInicio.ToString());

            }
            catch (Exception e) {
                Console.WriteLine(e);

                throw;
            }

            throw new NotImplementedException();
        }

        public Task<int> Delete() {
            throw new NotImplementedException();
        }

        public Task<int> Alta() {
            throw new NotImplementedException();
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
