using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.Settings {
    /// <summary>
    /// Clase que contiene los campos y métodos del objeto tipo Paquete.
    /// </summary>
    public class Paquete : AbstOtrosTipos{
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Id del paquete.
        /// </summary>
        public int IdPaquete { get; set; }

        /// <summary>
        /// Indica si el paquete actual incluye gym.
        /// </summary>
        public bool Gym { get; set; }

        /// <summary>
        /// Indica el nombre del paquete.
        /// </summary>
        public string NombrePaquete { get; set; }

        /// <summary>
        /// La descripción del paquete.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Indica el número de clases que incluye el paquete.
        /// </summary>
        public int NumClasesTotales { get; set; }

        /// <summary>
        /// Indica el número de clases que puede tomar el usuario en la semana.
        /// </summary>
        public int NumClasesSemanales { get; set; }

        /// <summary>
        /// Indica el costo mensual del paquete.
        /// </summary>
        public decimal Costo { get; set; }

        /// <summary>
        /// Contiene el id la clases que incluye el paquete separados por comas.
        /// </summary>
        public string IdClase { get; set; }

        /// <summary>
        /// Contiene el nombre de las clases que incluyen el paquete separados por comas.
        /// </summary>
        public string NombreClase { get; set; }

        public override async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update de un objeto tipo Clase.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string updateQuery = @"UPDATE paquete
                                             SET Gym=@Gym, NombrePaquete=@NombrePaquete,
                                                 Descripcion=@Descripcion,
                                                 NumClasesTotales=@NumClasesTotales, 
                                                 NumClasesSemanales=@NumClasesSemanales,
                                                 Costo=@Costo
                                             WHERE IdPaquete=@IdPaquete";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@Gym", Convert.ToInt32(Gym).ToString());
                command.Parameters.AddWithValue("@NombrePaquete", NombrePaquete);

                command.Parameters.AddWithValue("@Descripcion", Descripcion);
                command.Parameters.AddWithValue("@NumeroClasesTotales", NumClasesTotales.ToString());
                command.Parameters.AddWithValue("@NumeroClasesSemanales", NumClasesSemanales.ToString());

                command.Parameters.AddWithValue("@Costo", Costo.ToString(CultureInfo.InvariantCulture));

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Update Paquete");
                Log.Debug("Se ha actualizado un Paquete.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el update de un paquete.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de alta ");

            if (NumClasesSemanales * 4 != NumClasesTotales) {
                ShowPrettyMessages.ErrorOk(
                    "El número de clases totales debe de tomar en cuenta 4 semanas. Ej: 2 Clases semanales para 8 clases totales.",
                    "Número de clases invalido");
                return 0;
            }

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"INSERT INTO paquete
                                           VALUES
                                               (default, @Gym, @NombrePaquete,
                                               @Descripcion, @NumClasesTotales, 
                                               @NumClasesSemanales, @Costo)";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@Gym", Convert.ToInt32(Gym).ToString());
                command.Parameters.AddWithValue("@NombrePaquete", NombrePaquete);

                command.Parameters.AddWithValue("@Descripcion", Descripcion);
                command.Parameters.AddWithValue("@NumClasesTotales", NumClasesTotales.ToString());

                command.Parameters.AddWithValue("@NumClasesSemanales", NumClasesSemanales.ToString());
                command.Parameters.AddWithValue("@Costo", Costo.ToString(CultureInfo.InvariantCulture));

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Alta Paquete");
                Log.Debug("Se ha dado de alta un paquete.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta un paquete.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que checa si se puede borrar el paquete.
        /// </summary>
        /// <returns>True si pasa todas las validaciones.</returns>
        private bool CheckDeleteConstrains() {
            // Checamos que no tenga clases.
            if (InitInfo.ListPaquetesClases.Any(x => x.IdPaquete == IdPaquete)) {
                ShowPrettyMessages.InfoOk(
                    "Hay clases asignadas a este paquete, antes de eliminarlo debe eliminar estas clases del paquete.",
                    "Clases asignadas al paquete");
                return false;
            }

            // Checamos que no haya clientes con el paquete.
            if (InitInfo.ObCoClientes.Any(x => x.IdPaquete == IdPaquete)) {
                ShowPrettyMessages.InfoOk(
                    "Hay clientes que tienen asignado este paquete, antes de eliminarlo debe cambiar el paquete de estos clientes.",
                    "Clientes con paquete asignado.");
                return false;

            }

            if (InitInfo.ObCoIngresos.Any(x => x.IdPaquete == IdPaquete)) {
                ShowPrettyMessages.ErrorOk(
                    "No se puede eliminar este paquete porque tiene ingresos registrados y si se elimina se puede perder información.",
                    "Paquete con Ingresos.");
                return false;
            }

            return true;
        }

        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de delete en una clase.");

            // Checamos si podemos eliminar
            if (!CheckDeleteConstrains()) {
                return 0;
            }

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from paquete where IdPaquete=@IdPaquete";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdPaquete", IdPaquete.ToString());

                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Delete Paquete");
                Log.Debug("Se ha eliminado un un paquete de la tabla.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete de la clase.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}