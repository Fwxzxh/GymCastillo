using System;
using System.Globalization;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que contiene los campos y métodos del objeto Instructor
    /// </summary>
    public class Instructor : AbstClientInstructor {
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// La hora de entrada designada al instructor
        /// </summary>
        public DateTime HoraEntrada { get; set; }

        /// <summary>
        /// La hora de salida designada al instructor.
        /// </summary>
        public DateTime HoraSalida { get; set; }

        /// <summary>
        /// La cantidad de dias a trabajar del instructor.
        /// </summary>
        public int DiasATrabajar { get; set; }

        /// <summary>
        /// La cantidad actual de dias trabajados.
        /// </summary>
        public int DiasTrabajados { get; set; }

        /// <summary>
        /// El sueldo del Instructor.
        /// </summary>
        public decimal Sueldo { get; set; }

        /// <summary>
        /// La cantidad del sueldo a descontar por amonestaciones.
        /// </summary>
        public decimal SueldoDescontar { get; set; }

        /// <summary>
        /// El id del tipo de instructor.
        /// </summary>
        public int  IdTipoInstructor { get; set; }

        /// <summary>
        /// El nombre del tipo de instructor.
        /// </summary>
        public string NombreTipoInstructor { get; set; }

        /// <summary>
        /// Método que actualiza la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas.</returns>
        public override async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update de un objeto tipo Instructor.");
            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();

                // TODO: poner la query.
                const string updateQuery = @"";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@IdInstructor", Id.ToString());
                command.Parameters.AddWithValue("@Domicilio", Domicio);
                command.Parameters.AddWithValue("@Telefono", Telefono);
                command.Parameters.AddWithValue("@NombreContacto", NombreContacto);
                command.Parameters.AddWithValue("@TelefonoContacto", TelefonoContacto);
                //command.Parameters.AddWithValue("@Foto", Foto);
                command.Parameters.AddWithValue("@HoraEntrada", HoraEntrada.TimeOfDay.ToString());// TODO checar tipos.
                command.Parameters.AddWithValue("@HoraSalida", HoraSalida.TimeOfDay.ToString());// TODO checar tipos.
                command.Parameters.AddWithValue("@Sueldo", Sueldo.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@SueldoADescontar", SueldoDescontar.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@IdTipoInstructor", IdTipoInstructor.ToString());

                var res = ExecSql.NonQuery(command, "Update Instructor").Result;

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconcoido a la hora de hacer update.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que borra la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas.</returns>
        public override Task<int> Delete() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de alta la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <returns>La cantidad de columanas afectadas.</returns>
        public override Task<int> Alta() {
            throw new NotImplementedException();
        }

        public override Task<int> NuevaAsistencia() {
            throw new NotImplementedException();
        }

        public override void Pago(decimal cantidad) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que obtiene el horario del Instructor de la instancia actual en un string
        /// </summary>
        /// <returns>Un string con el horario del instructor.</returns>
        public override string GetHorarioStr() {
            throw new NotImplementedException();
        }
    }
}