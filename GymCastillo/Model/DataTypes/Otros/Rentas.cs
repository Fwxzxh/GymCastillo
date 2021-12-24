using System;
using System.Globalization;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.Otros {

    /// <summary>
    /// Clase que contiene los métodos y campos de la clase de rentas.
    /// </summary>
    public class Rentas : IOnlyAlta{
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Id de la renta.
        /// </summary>
        public int IdRenta { get; set; }

        /// <summary>
        /// La fecha en la que se va a llevar acabo la renta.
        /// </summary>
        public DateTime FechaRenta { get; set; }

        /// <summary>
        /// Id del cliente al que se le hizo la renta.
        /// </summary>
        public int IdClienteRenta { get; set; }

        /// <summary>
        /// El nombre del cliente al que se le hizo la renta.
        /// </summary>
        public string NombreClienteRenta { get; set; }

        /// <summary>
        /// El id del espacio a rentar.
        /// </summary>
        public int IdEspacio { get; set; }

        /// <summary>
        /// El nombre del espacio a rentar.
        /// </summary>
        public string NombreEspacio { get; set; }

        /// <summary>
        /// Int que representa el dia en que se renta el espacio.
        /// </summary>
        public int Dia { get; set; }

        /// <summary>
        /// La hora de inicio de la renta.
        /// </summary>
        public DateTime HoraInicio { get; set; }

        /// <summary>
        /// La hora en la que termina la renta.
        /// </summary>
        public DateTime HoraFin { get; set; }

        /// <summary>
        /// El costo de la renta.
        /// </summary>
        public decimal Costo { get; set; }

        public async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta Personal.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"insert into rentas
                                           values
                                               (default, @FechaRenta, @IdClienteRenta,
                                                @IdEspacio, @Dia, @HoraInicio, @HoraFin,
                                                @Costo)";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@FechaRenta",
                    FechaRenta.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@IdClienteRenta",
                    IdClienteRenta.ToString());

                command.Parameters.AddWithValue("@IdEspacio",
                    IdEspacio.ToString());
                command.Parameters.AddWithValue("@Dia",
                    Dia.ToString());
                command.Parameters.AddWithValue("@HoraInicio",
                    HoraInicio.ToString("HHmm"));
                command.Parameters.AddWithValue("@HoraFin",
                    HoraFin.ToString("HHmm"));

                command.Parameters.AddWithValue("@Costo",
                    Costo.ToString(CultureInfo.InvariantCulture));

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Alta Renta");
                Log.Debug("Se ha registrado una Renta.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta una renta.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al registrar una renta. Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}