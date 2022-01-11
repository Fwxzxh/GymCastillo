using System;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.IntersectionTables {

    /// <summary>
    /// Clase que contiene los campos y métodos de la tabla de la tabla de intersección ClaseInstructores
    /// para que una clase tenga varios instructores.
    /// </summary>
    public class ClaseInstructores : AbstOtrosTipos{
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?
        .DeclaringType);

        /// <summary>
        /// El id de la clase.
        /// </summary>
        public int IdClase { get; set; }

        /// <summary>
        /// El id del instructor.
        /// </summary>
        public int IdInstructor { get; set; }

        public override Task<int> Update() {
            // NO update.
            throw new System.NotSupportedException();
        }

        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de delete de uan clase a un paquete.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from ClaseInstructores where IdClase=@IdClase and IdInstructor=@IdInstructor";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdClase", IdClase.ToString());
                command.Parameters.AddWithValue("@IdInstructor", IdInstructor.ToString());

                var res = await ExecSql.NonQuery(command, "Delete IdInstructor");
                Log.Debug("Se ha eliminado un ClienteInstructor de la tabla.");
                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete del instructor en la clase.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de alta de un instructor a una clase.");

            // Checamos que no haya sido dada de alta anteriormente.
            if (InitInfo.ObCoClaseInstructores.Any(
                    x => x.IdClase == IdClase && x.IdInstructor == IdInstructor)) {
                ShowPrettyMessages.ErrorOk(
                    "No puedes dar de alta a ese instructor porque ya fue registrado en esta clase con anterioridad.",
                    "Instructor duplicado.");
                return 0;
            }

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"insert into PaquetesClases values (@IdClase, @IdInstructor)";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdClase", IdClase.ToString());
                command.Parameters.AddWithValue("@IdInstructor", IdInstructor.ToString());

                var res = await ExecSql.NonQuery(command, "Alta ClaseInstructor");
                Log.Debug("Se ha dado de alta un ClaseInstructor de la tabla.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta el instructor en la clase.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al dar de alta el instructor en la clase.  Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}