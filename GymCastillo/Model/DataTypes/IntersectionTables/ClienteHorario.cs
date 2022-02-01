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
    /// Clase que contiene los campos y los métodos de la tabla ClienteHorario la cual permite asignarle
    /// horarios a los clientes.
    /// </summary>
    public class ClienteHorario : AbstOtrosTipos{
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?
        .DeclaringType);

        /// <summary>
        /// El id del cliente
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// El id del horario
        /// </summary>
        public int IdHorario { get; set; }

        public override Task<int> Update() {
            // No update
            throw new NotSupportedException();
        }

        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de delete de un horario a un cliente.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from ClienteHorario where IdCliente=@IdCliente and IdHorario=@IdHorario";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdCliente", IdCliente.ToString());
                command.Parameters.AddWithValue("@IdHorario", IdHorario.ToString());

                var res = await ExecSql.NonQuery(command, "Delete ClienteHorario");
                Log.Debug("Se ha eliminado un ClienteHorario de la tabla.");
                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete del horario al cliente.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de alta de un horario a un cliente.");

            // Checamos que no haya sido dada de alta anteriormente.
            if (InitInfo.ObCoClienteHorario.Any(
                    x => x.IdCliente == IdCliente && x.IdHorario == IdHorario)) {
                ShowPrettyMessages.ErrorOk(
                    "No puedes dar de alta a ese horario porque ya fue registrado en este cliente con anterioridad.",
                    "Horario duplicado.");
                return 0;
            }

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"insert into ClienteHorario values (@IdCliente, @IdHorario)";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdCliente", IdCliente.ToString());
                command.Parameters.AddWithValue("@IdHorario", IdHorario.ToString());

                var res = await ExecSql.NonQuery(command, "Alta ClienteHorario");
                Log.Debug("Se ha dado de alta un ClienteHorario de la tabla.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta el horario al Cliente.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al dar de alta el horario al instructor.  Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}