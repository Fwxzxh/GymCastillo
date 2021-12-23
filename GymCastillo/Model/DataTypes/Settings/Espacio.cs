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
    /// Clase que contiene todos los métodos y campos del tipo de datos Espacio.
    /// </summary>
    public class Espacio : AbstOtrosTipos{
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Id del espacio.
        /// </summary>
        public int IdEspacio { get; set; }

        /// <summary>
        /// Nombre del espacio.
        /// </summary>
        public string NombreEspacio { get; set; }

        /// <summary>
        /// Descripción del espacio.
        /// </summary>
        public string Descripción { get; set; }

        public override async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update de un espacio.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string updateQuery = @"update espacio
                                             set NombreEspacio=@NombreEspacio,
                                                 Descripcion=@Descripcion
                                             where IdEspacio=@IdEspacio";

                await using var command = new MySqlCommand(updateQuery, connection);

                //command.Parameters.AddWithValue("@IdEspacio", IdEspacio.ToString());
                command.Parameters.AddWithValue("@NombreEspacio", NombreEspacio);
                command.Parameters.AddWithValue("@Descripcion", Descripción);

                Log.Debug("Se ha creado la query.");

                var res =await ExecSql.NonQuery(command, "Edit Espacio");
                Log.Debug("Se ha editado un espacio.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de actualizar el Espacio.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que valida si se puede eliminar un espacio.
        /// </summary>
        /// <returns>False si falla una validación.</returns>
        private bool CheckDeleteConstrains() {
            // clase
            if (InitInfo.ObCoClases.Any(x => x.IdEspacio == IdEspacio)) {
                ShowPrettyMessages.InfoOk(
                    "No se puede eliminar este espacio ya que esta asignado a una clase, intente cambiar esa clase primero.",
                    "Hay una clase ");
                return false;
            }

            // rentas
            if (InitInfo.ObCoRentas.Any(x => x.IdEspacio == IdEspacio)){
                ShowPrettyMessages.InfoOk(
                    "No se puede eliminar este espacio ya que esta asignado a una Renta y se podría perder información si se borra, debe eliminar la renta antes.",
                    "Hay una clase ");
                return false;
            }

            return true;
        }

        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de delete de un espacio.");

            // Checamos si podemos eliminar
            if (!CheckDeleteConstrains()) {
                return 0;
            }

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from espacio where IdEspacio=@IdEspacio";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdEspacio", IdEspacio.ToString());

                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Delete Espacio");
                Log.Debug("Se ha eliminado un espacio de la tabla.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete del espacio.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de alta de un espacio.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"insert into espacio
                                           values
                                               (default, @NombreEspacio,
                                                @Descripcion);";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@NombreEspacio", NombreEspacio);
                command.Parameters.AddWithValue("@Descripcion", Descripción);

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Alta Espacio");
                Log.Debug("Se ha dado de alta un Espacio.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta un espacio.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}