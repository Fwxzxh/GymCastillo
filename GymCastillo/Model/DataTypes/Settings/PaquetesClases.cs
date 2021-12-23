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
    /// clase que contiene los campos y métodos del tipo de datos PaquetesClases
    /// </summary>
    public class PaquetesClases : AbstOtrosTipos{
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// El id del paquete
        /// </summary>
        public int IdPaquete { get; set; }

        /// <summary>
        /// El did de la clase
        /// </summary>
        public int IdClase { get; set; }

        /// <summary>
        /// Para este método no hay Update, no Usar
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        public override Task<int> Update() {
            throw new NotImplementedException();
        }

        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de delete de una clase a un paquete.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from PaquetesClases where IdClase=@IdClase and IdPaquete=@IdPaquete";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdClase", IdClase.ToString());
                command.Parameters.AddWithValue("@IdPaquete", IdPaquete.ToString());

                var res = await ExecSql.NonQuery(command, "Delete PaquetesClases");
                Log.Debug("Se ha eliminado un PaqueteClase de la tabla.");
                return res;

            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete de la Clase en el paquete.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de alta de una clase a un paquete.");

            // Checamos que no haya sido dada de alta anteriormente.
            if (InitInfo.ListPaquetesClases.Any(
                    x => x.IdClase == IdClase && x.IdPaquete == IdPaquete)) {
                ShowPrettyMessages.ErrorOk(
                    "No puedes dar de alta esta clase porque ya fue registrada en este paquete con anterioridad.",
                    "Clase duplicada.");
                return 0;
            }

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"insert into PaquetesClases values (@IdPaquete, @IdClase)";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdClase", IdClase.ToString());
                command.Parameters.AddWithValue("@IdPaquete", IdPaquete.ToString());

                var res = await ExecSql.NonQuery(command, "Alta PaquetesClases");
                Log.Debug("Se ha dado de alta un PaqueteClase de la tabla.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta la clase en el paquete..");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al dar de alta la clase en el paquete, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}