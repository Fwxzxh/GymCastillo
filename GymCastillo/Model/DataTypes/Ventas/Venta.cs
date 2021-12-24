using System;
using System.Globalization;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.Ventas {
    /// <summary>
    /// Clase que contiene los métodos y campos del tipo de dato venta.
    /// </summary>
    public class Venta : IOnlyAlta{
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// El id de la venta.
        /// </summary>
        public int IdVenta { get; set; }

        /// <summary>
        /// La fecha en la que ha ocurrido la venta.
        /// </summary>
        public DateTime FechaVenta { get; set; }

        /// <summary>
        /// El id del producto a vender
        /// </summary>
        public int IdProducto { get; set; }

        /// <summary>
        /// El concepto de la venta.
        /// </summary>
        // TODO: igual y esto no va.
        public string Concepto { get; set; }

        /// <summary>
        /// El costo final de la venta.
        /// </summary>
        public decimal Costo { get; set; }

        public async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta una venta.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@FechaVenta",
                    FechaVenta.ToString("yyyy-MM-dd HH:mm:ss"));

                command.Parameters.AddWithValue("@Concepto", Concepto);

                command.Parameters.AddWithValue("@Costo",
                    Costo.ToString(CultureInfo.InvariantCulture));

                Log.Debug("Se ha creado la query.");

                var res =await ExecSql.NonQuery(command, "Nueva Alta");
                Log.Debug("Se ha dado de alta una nueva venta.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta una venta.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}