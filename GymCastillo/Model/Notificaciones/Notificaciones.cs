using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.Notificaciones {
    /// <summary>
    /// Clase que contiene los métodos para obtener las notificaciones y los cuales son necesarios para que el programa
    /// Este al tanto de las fechas y los plazos.
    /// </summary>
    public class Notificaciones {

        /// <summary>
        /// La fecha del último reset de los campos de la bd.
        /// </summary>
        public static DateTime FechaUltimoReset { get; set; }

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de obtener los usuarios a los cuales se les expira su paquete en los próximos 15 dias
        /// </summary>
        /// <param name="dias">El número de dias a verificar por la fecha de vencimiento de pago, por defecto 15</param>
        /// <returns>Una lista con clientes</returns>
        public static IEnumerable<Cliente> GetNextExpireUsers(int dias=15) {
            Log.Debug("Se ha iniciado el proceso de ver que clientes están próximos a que se les expire su paquete");

            // Obtenemos los usuarios a los cuales se les vence su paquete en los próximos 15 dias.
            var usuarios =
                InitInfo.ObCoClientes.Where(x => x.FechaVencimientoPago < DateTime.Today + TimeSpan.FromDays(dias));

            return usuarios;
        }

        /// <summary>
        /// Método que obtiene los horarios para el dia de hoy.
        /// </summary>
        /// <returns>Los horarios que corresponden al dia de hoy</returns>
        public static IEnumerable<Horario> GetHorariosToday() {
            var hoy =(int) DateTime.Now.DayOfWeek;
            var horarios = InitInfo.ObCoHorarios.Where(x => x.Dia == hoy);

            return horarios;
        }

        /// <summary>
        /// Método que se encarga de verificar si hay campos para Resetear y resetearlos.
        /// </summary>
        public static async Task CheckResetFields() {
            // Nuevo Plan ====>
            // Checamos si es lunes
            // SI es lunes procedemos a resetear los cupos de la semana y los paquetes semanales y guardamos la fecha en la bd.
            // Si no. bye.
            // Tenemos que dejar expuesto el método de reset para hacerlo con un botón y exponer campo con la fecha de ultimo reset

            var hoy = DateTime.Today;

            // Obtenemos la fechas
            await GetFechas();

            if (FechaUltimoReset == DateTime.Today) {
                Log.Info("Ya se han reseteado los campos de los usuarios y las clases.");
                return;
            }

            if (hoy.DayOfWeek == DayOfWeek.Monday) {
                Log.Info("Se ha detectado que es lunes y se procederá a resetear los campos de los usuarios y las clases.");

                // si es lunes reseteamos los campos
                await ResetFieldsAndUpdate().ConfigureAwait(false);
            }
            else {
                Log.Info("Hoy no toca Resetear campos.");
            }
        }

        /// <summary>
        /// Método que se encarga de resetear los cupos de los horarios y las clases por semana a su valor original.
        /// </summary>
        public static async Task ResetFieldsAndUpdate() {
            Log.Info("Se ha iniciado el proceso de resetear los cupos y las clases por semana.");
            var hoy = DateTime.Today;

            var clasesSemana = ResetClasesSemana();
            var cuposSemana = ResetCupos();
            var updateDate = UpdateDate(hoy);

            var done = await Task.WhenAll(clasesSemana, cuposSemana, updateDate);

            if (done.All(x => x)) {

                // Actualizamos los clientes
                var clientes = await GetFromDb.GetClientes();
                InitInfo.ObCoClientes.Clear();
                foreach (var cliente in clientes) {
                    InitInfo.ObCoClientes.Add(cliente);
                }

                // Actualizamos los cupos
                var horarios = await GetFromDb.GetHorarios();
                InitInfo.ObCoHorarios.Clear();
                foreach (var horario in horarios) {
                    InitInfo.ObCoHorarios.Add(horario);
                }

                Log.Info("Se han reseteado los cambios exitosamente.");
                ShowPrettyMessages.NiceMessageOk(
                    "Se han reseteado los campos  exitosamente, Se recomienda Reiniciar el programa.",
                    "Operación exitosa.");
                return;
            }
            Log.Error("Ha ocurrido un error, el proceso de reset no se ha completado exitosamente.");
        }

        /// <summary>
        /// Método que se encarga de regresar las clases x semana a su valor original.
        /// </summary>
        /// <returns><c>true</c> si el proceso fue exitoso.</returns>
        private static async Task<bool> ResetClasesSemana() {
            // Necesitamos restaurar las clases por semana a su valor original.
            // el número de clase depende del paquete

            // Obtenemos las clases.
            var paquetes = InitInfo.ObCoDePaquetes;

            if (paquetes.Count == 0) {
                return true;
            }

            foreach (var paquete in paquetes) {
                // Por cada clase en clases vamos a resetear el número de clases por semana al valor x defecto
                // dependiendo del paquete.

                try {
                    await using var connection = new MySqlConnection(GetInitData.ConnString);
                    await connection.OpenAsync();
                    Log.Debug("Creamos la conexión.");

                    const string sqlQuery = @"update cliente
                                              set
                                                  ClasesSemanaDisponibles=@Clases
                                              where Activo=true and IdPaquete=@Paquete;";

                    await using var command = new MySqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@Clases",
                        paquete.NumClasesSemanales.ToString());

                    command.Parameters.AddWithValue("@Paquete",
                        paquete.IdPaquete.ToString());

                    Log.Debug("Se ha generado la query.");

                    await ExecSql.NonQuery(command, "Reset Clases Semanales");
                    Log.Debug($"Se ha hecho el Reset de las clases semanales para el paquete {paquete.IdPaquete.ToString()}");
                }
                catch (Exception e) {
                    Log.Error("Ha ocurrido un error desconocido a la hora de resetear las clases semanales.");
                    Log.Error($"Error: {e.Message}");
                    ShowPrettyMessages.ErrorOk(
                        $"Ha ocurrido un error desconocido al resetear las clases semanales, Contacte a " +
                        $"los administradores.. Error: {e.Message}",
                        "Error desconocido");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Obtiene la fecha de último Reseteo.
        /// </summary>
        private static async Task GetFechas() {
            Log.Info("se ha empezado el proceso de obtener la información de las fechas de reseteo.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"select ResetFechaSemana from ResetTimer where id=1";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                while (await reader.Result.ReadAsync()) {
                    FechaUltimoReset = reader.Result.GetDateTime("ResetFechaSemana");
                }

                Log.Info("Se han obtenido las fechas de reseteo con éxito.");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de obtener la información de los timers.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los timer. Error: {e.Message}",
                    "Error desconocido");
            }
        }

        /// <summary>
        /// Método que se encarga de resetear los campos de los cupo en los horarios.
        /// </summary>
        /// <returns><c>true</c> si la operación fue exitosa.</returns>
        private static async Task<bool> ResetCupos() {
            Log.Info("Se ha iniciado el proceso de resetear las fechas de los horarios.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Creamos la conexión.");

                const string sqlQuery = @"update horario set CupoActual=0 where IdHorario > 0";

                await using var command = new MySqlCommand(sqlQuery, connection);

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Reset Cupos");
                Log.Debug("Reset Cupos Done");

                if (res != 0) return true;
                ShowPrettyMessages.ErrorOk(
                    "No se han reseteado los cupos en la base de datos, contacte a los administradores.",
                    "Error al resetear los cupos.");
                return false;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de resetear los cupos.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al resetear los cupos, Contacte a los administradores. Error: {e.Message}",
                    "Error desconocido");
                return false;
            }
        }

        /// <summary>
        /// Método que Hace Update de la fecha de ultimo Reset de Campos
        /// </summary>
        /// <param name="hoy">La fecha de hoy</param>
        /// <returns><c>true</c> si la operación fue exitosa.</returns>
        private static async Task<bool> UpdateDate(DateTime hoy) {
            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string saveDate = @"update ResetTimer set ResetFechaSemana=@Fecha where id=1";

                await using var command = new MySqlCommand(saveDate, connection);

                command.Parameters.AddWithValue("@Fecha",
                    hoy.ToString("yyyy-MM-dd HH:mm:ss"));

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Update Timers");
                Log.Debug("Update Timers done");

                if (res == 0) {
                    ShowPrettyMessages.ErrorOk(
                        "No se ha modificado la fecha de reseteo, contacte a un administrador.",
                        "Error Desconocido");
                    return false;
                }

                return true;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de update el timer de cupos.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido a la hora de resetear los timers, " +
                            $"contacte a un administrador. Error: {e.Message}",
                    "Error desconocido");
                return false;
            }
        }
    }
}