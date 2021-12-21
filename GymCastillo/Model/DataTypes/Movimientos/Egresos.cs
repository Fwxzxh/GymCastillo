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
    /// Clase que contiene los campos y métodos de la clase pagos.
    /// </summary>
    public class Egresos : AbstractMovimientos, IOnlyAlta {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Indica que tipo de Egreso es; 1:NominaUsuarios, 2:NominaInstructores, 3:Servicios, 4:Otros
        /// </summary>
        public int Tipo { get; set; }

        /// <summary>
        /// Indica si el movimiento es por un servicio.
        /// </summary>
        public bool Servicios { get; set; }

        /// <summary>
        /// Indica si el movimiento es por una paga de nómina.
        /// </summary>
        public bool Nomina { get; set; }

        /// <summary>
        /// Id del usuario al cual se le paga si es una nómina.
        /// </summary>
        public int IdUsuarioPagar { get; set; }

        /// <summary>
        /// El nombre del usuario que hizo el pago.
        /// </summary>
        public string NombreUsuarioPagar { get; set; }

        /// <summary>
        /// Id del instructor
        /// </summary>
        public int IdInstructor { get; set; }

        /// <summary>
        /// El nombre del instructor.
        /// </summary>
        public string NombreInstructor { get; set; }

        /// <summary>
        /// Id del Personal
        /// </summary>
        public int IdPersonal { get; set; }

        /// <summary>
        /// Nombre del personal.
        /// </summary>
        public string NombrePersonal { get; set; }

        /// <summary>
        /// Método que da de alta un Pago.
        /// </summary>
        public async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta un egreso.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"INSERT INTO egresos
                                           VALUES
                                               (default, @FechaRegistro, @IdUsuario,
                                               @Servicios, @Nomina, @Otros, @IdUsuarioPagar,
                                               @IdInstructor, @IdPersonal, @Concepto, 
                                               @NumeroRecibo, @Monto);";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@FechaRegistro",
                    FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@IdUsuario",
                    Init.Init.LoggedId.ToString());

                command.Parameters.AddWithValue("@Servicios",
                    Convert.ToInt32(Servicios).ToString());
                command.Parameters.AddWithValue("@Nomina",
                    Convert.ToInt32(Nomina).ToString());
                command.Parameters.AddWithValue("@Otros",
                    Convert.ToInt32(Otros).ToString());
                command.Parameters.AddWithValue("@IdUsuarioPagar",
                    IdUsuarioPagar == 0 ? null : IdUsuarioPagar.ToString());

                command.Parameters.AddWithValue("@IdInstructor",
                    IdInstructor == 0 ? null : IdInstructor.ToString());
                command.Parameters.AddWithValue("@IdPersonal",
                    IdPersonal == 0 ? null : IdPersonal.ToString());
                command.Parameters.AddWithValue("@Concepto",
                    Concepto);
                command.Parameters.AddWithValue("@NumeroRecibo", NumeroRecibo);
                command.Parameters.AddWithValue("@Monto",
                    Monto.ToString(CultureInfo.InvariantCulture));

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Nuevo Egreso");
                Log.Debug("Se ha dado de alta un Ingreso.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta un Egreso.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}