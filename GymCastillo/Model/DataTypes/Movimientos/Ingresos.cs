using System;
using System.Globalization;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.Movimientos {
    /// <summary>
    /// Clase que contiene todos los campos y métodos de la clase Ingresos.
    /// </summary>
    public class Ingresos : AbstractMovimientos, IOnlyAlta {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// El id de la renta si el ingreso es por una renta.
        /// </summary>
        public int IdRenta { get; set; }

        /// <summary>
        /// La fecha en la que se hizo la renta.
        /// </summary>
        public DateTime FechaRenta { get; set; }

        /// <summary>
        /// El id de la venta si el ingreso es por una venta
        /// </summary>
        public int IdVenta { get; set; }

        /// <summary>
        /// El id del cliente si el ingreso es por un cliente.
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// El nombre del cliente si el ingreso es por un cliente.
        /// </summary>
        public string NombreCliente { get; set; }

        /// <summary>
        /// El id del paquete que se va a comprar
        /// </summary>
        public int IdPaquete { get; set; }

        /// <summary>
        /// El nombre del paquete que se va a
        /// </summary>
        public string NombrePaquete { get; set; }

        /// <summary>
        /// El id del locker que se planea comprar.
        /// </summary>
        public int IdLocker { get; set; }

        /// <summary>
        /// EL nombre del locker que se planea comprar.
        /// </summary>
        public string NombreLocker { get; set; }

        /// <summary>
        /// Da de alta la instancia actual del ingreso en la base de datos.
        /// </summary>
        public async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta un ingreso.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"INSERT INTO ingresos
                                           VALUES
                                               (default, @FechaRegistro, @IdUsuario,
                                               @IdRenta, @IdCliente, @IdVenta, @Otros, @Concepto,
                                               @IdPaquete, @IdLocker, @NumeroRecibo, @Monto);";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@FechaRegistro", FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@IdUsuario", Init.Init.LoggedId.ToString());

                command.Parameters.AddWithValue("@IdRenta", IdRenta.ToString());
                command.Parameters.AddWithValue("@IdCliente", IdCliente.ToString());
                command.Parameters.AddWithValue("@IdVenta", IdVenta.ToString());
                command.Parameters.AddWithValue("@Otros", Convert.ToInt32(Otros).ToString());
                command.Parameters.AddWithValue("@Concepto", Concepto);

                command.Parameters.AddWithValue("@IdPaquete", IdPaquete.ToString());
                command.Parameters.AddWithValue("@IdLocker", IdLocker.ToString());

                command.Parameters.AddWithValue("@NumeroRecibo", NumeroRecibo);
                command.Parameters.AddWithValue("@Monto", Monto.ToString(CultureInfo.InvariantCulture));

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Nuevo Ingreso");
                Log.Debug("Se ha dado de alta un Ingreso.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta un ingreso.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}