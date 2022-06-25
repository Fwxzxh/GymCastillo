using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.Reportes {
    /// <summary>
    /// Clase que se encarga de
    /// </summary>
    public static class GetReportes {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que obtiene los ingresos, por defecto toma los últimos 7 días
        /// </summary>
        /// <returns>Una lista de elementos tipo ingreso</returns>
        public static async Task<List<Ingresos>> GetReporteIngresos(int dias = 7) {
            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"SELECT
                                          i.IdIngresos, i.FechaRegistro AS FechaRegistroIngreso,
                                          i.IdUsuario, CONCAT(u.Nombre, ' ', u.ApellidoPaterno, ' ', u.ApellidoMaterno) as NombreUsuario,
                                          i.IdRenta, CONCAT(cr.Nombre, ' ', cr.ApellidoPaterno, ' ', cr.ApellidoMaterno) as NombreClienteRenta,
                                          r.FechaRenta AS FechaRenta,
                                          i.IdCliente, CONCAT(c.Nombre, ' ', c.ApellidoPaterno, ' ', c.ApellidoMaterno) as NombreCliente,
                                          i.IdVenta, v.Concepto AS ConceptoVenta,
                                          i.IdClienteRenta, CONCAT(cr.Nombre, ' ', cr.ApellidoPaterno, ' ', cr.ApellidoMaterno) as NombreClienteRentaDeuda,
                                          i.Otros, i.Concepto AS ConceptoIngreso, i.IdPaquete, p.NombrePaquete, i.IdLocker,
                                          i.NumeroRecibo, i.Monto, i.MontoRecibido
                                      FROM ingresos i
                                          INNER JOIN usuario u ON i.IdUsuario = u.IdUsuario
                                          LEFT JOIN rentas r ON i.IdRenta = r.IdRenta
                                          LEFT JOIN cliente c ON i.IdCliente = c.IdCliente
                                          LEFT JOIN ClienteRenta cr ON i.IdClienteRenta = cr.IdClienteRenta
                                          LEFT JOIN ventas v ON i.IdVenta = v.IdVenta
                                          LEFT JOIN paquete p ON i.IdPaquete = p.IdPaquete
                                          LEFT JOIN locker l ON i.IdLocker = l.IdLocker
                                      WHERE	i.FechaRegistro >= NOW() + INTERVAL -@days DAY
                                          AND i.FechaRegistro < NOW() + INTERVAL 0 DAY;";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@days", dias.ToString());
                
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listIngresos = new List<Ingresos>();

                while (await reader.Result.ReadAsync()) {
                    var ingreso = new Ingresos() {
                        IdMovimiento = reader.Result.GetInt32("IdIngresos"),
                        FechaRegistro = reader.Result.GetDateTime("FechaRegistroIngreso"),

                        IdUsuario = await reader.Result.IsDBNullAsync("IdUsuario")
                            ? 0
                            : reader.Result.GetInt32("IdUsuario"),
                        NombreUsuario = await reader.Result.IsDBNullAsync("NombreUsuario")
                            ? ""
                            : reader.Result.GetString("NombreUsuario"),

                        IdRenta = await reader.Result.IsDBNullAsync("IdRenta")
                            ? 0
                            : reader.Result.GetInt32("IdRenta"),
                        NombreClienteRenta = await reader.Result.IsDBNullAsync("NombreClienteRenta")
                            ? ""
                            : reader.Result.GetString("NombreClienteRenta"),

                        FechaRenta = await reader.Result.IsDBNullAsync("FechaRenta")
                            ? default
                            : reader.Result.GetDateTime("FechaRenta"),

                        IdCliente = await reader.Result.IsDBNullAsync("IdCliente")
                            ? 0
                            : reader.Result.GetInt32("IdCliente"),
                        NombreCliente = await reader.Result.IsDBNullAsync("NombreCliente")
                            ? ""
                            : reader.Result.GetString("NombreCliente"),

                        IdVenta = await reader.Result.IsDBNullAsync("IdVenta")
                            ? 0
                            : reader.Result.GetInt32("IdVenta"),
                        Concepto = await reader.Result.IsDBNullAsync("ConceptoIngreso")
                            ? ""
                            : reader.Result.GetString("ConceptoIngreso"),

                        IdClienteRenta = await reader.Result.IsDBNullAsync("IdClienteRenta")
                            ? 0
                            : reader.Result.GetInt32("IdClienteRenta"),
                        // TODO: preguntar por NombreClienteRentaDeuda

                        Otros = !await reader.Result.IsDBNullAsync("Otros") &&
                                reader.Result.GetBoolean("Otros"),

                        IdPaquete = await reader.Result.IsDBNullAsync("IdPaquete")
                            ? 0
                            : reader.Result.GetInt32("IdPaquete"),
                        NombrePaquete = await reader.Result.IsDBNullAsync("NombrePaquete")
                            ? ""
                            : reader.Result.GetString("NombrePaquete"),

                        IdLocker = await reader.Result.IsDBNullAsync("IdLocker")
                            ? 0
                            : reader.Result.GetInt32("IdLocker"),
                        // NombreLocker = await reader.Result.IsDBNullAsync("Nombre")
                        //     ? ""
                        //     : reader.Result.GetString("Nombre"),

                        NumeroRecibo = await reader.Result.IsDBNullAsync("NumeroRecibo")
                            ? ""
                            : reader.Result.GetString("NumeroRecibo"),
                        Monto = await reader.Result.IsDBNullAsync("Monto")
                            ? 0
                            : reader.Result.GetDecimal("Monto"),
                        MontoRecibido = await reader.Result.IsDBNullAsync("MontoRecibido")
                            ? 0
                            : reader.Result.GetDecimal("MontoRecibido"),
                    };

                    listIngresos.Add(ingreso);
                }
                Log.Debug("Se han obtenido con éxito la información de los Ingresos.");

                return listIngresos;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los Ingresos.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los clientes. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }


        /// <summary>
        /// Método que obtiene los egresos de la ultima semana.
        /// </summary>
        /// <returns>Una lista de elementos tipo egresos.</returns>
        public static async Task<List<Egresos>> GetReporteEgresos(int days = 7) {
            Log.Debug("Se ha empezado el proceso de obtener todos los ingresos.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"SELECT
                                          p.IdPagosGeneral, p.FechaRegistro,
                                          p.IdUsuario, CONCAT(u.Nombre, ' ', u.ApellidoPaterno, ' ', u.ApellidoMaterno) as NombreUsuario,
                                          p.Servicios, p.Nomina, p.Otros, p.IdUsuarioPagar,
                                          CONCAT(up.Nombre, ' ', up.ApellidoPaterno, ' ', up.ApellidoMaterno) as NombreUsuarioPagar,
                                          p.IdInstructor, CONCAT(i.Nombre, ' ', i.ApellidoPaterno, ' ', i.ApellidoMaterno) as NombreInstructor,
                                          p.IdPersonal, CONCAT(ps.Nombre, ' ', ps.ApellidoPaterno, ' ', ps.ApellidoMaterno) as NombrePersonal,
                                          p.Concepto, p.NumeroRecibo, p.Monto
                                      FROM egresos p
                                          INNER JOIN usuario u ON p.IdUsuario = u.IdUsuario
                                          LEFT JOIN usuario up ON p.IdUsuarioPagar = up.IdUsuario
                                          LEFT JOIN instructor i ON p.IdInstructor = i.IdInstructor
                                          LEFT JOIN personal ps ON p.IdPersonal = ps.IdPersonal
                                      WHERE	p.FechaRegistro >= NOW() + INTERVAL -@days DAY
                                          AND	p.FechaRegistro < NOW() + INTERVAL 0 DAY;";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                
                command.Parameters.AddWithValue("@days", days.ToString());
                
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listEgresos = new List<Egresos>();

                while (await reader.Result.ReadAsync()) {
                    var egreso = new Egresos() {
                        IdMovimiento = reader.Result.GetInt32("IdPagosGeneral"),
                        FechaRegistro = reader.Result.GetDateTime("FechaRegistro"),

                        IdUsuario = await reader.Result.IsDBNullAsync("IdUsuario")
                            ? 0
                            : reader.Result.GetInt32("IdUsuario"),
                        NombreUsuario = await reader.Result.IsDBNullAsync("NombreUsuario")
                            ? ""
                            : reader.Result.GetString("NombreUsuario"),

                        Servicios = !await reader.Result.IsDBNullAsync("Servicios") &&
                                    reader.Result.GetBoolean("Servicios"),
                        Nomina = !await reader.Result.IsDBNullAsync("Nomina") &&
                                 reader.Result.GetBoolean("Nomina"),
                        Otros = !await reader.Result.IsDBNullAsync("Otros") &&
                                reader.Result.GetBoolean("Otros"),

                        IdUsuarioPagar = await reader.Result.IsDBNullAsync("IdUsuarioPagar")
                            ? 0
                            : reader.Result.GetInt32("IdUsuarioPagar"),
                        NombreUsuarioPagar = await reader.Result.IsDBNullAsync("NombreUsuarioPagar")
                            ? ""
                            : reader.Result.GetString("NombreUsuarioPagar"),

                        IdInstructor = await reader.Result.IsDBNullAsync("IdInstructor")
                            ? 0
                            : reader.Result.GetInt32("IdInstructor"),
                        NombreInstructor = await reader.Result.IsDBNullAsync("NombreInstructor")
                            ? ""
                            : reader.Result.GetString("NombreInstructor"),

                        IdPersonal = await reader.Result.IsDBNullAsync("IdPersonal")
                            ? 0
                            : reader.Result.GetInt32("IdPersonal"),
                        NombrePersonal = await reader.Result.IsDBNullAsync("NombrePersonal")
                            ? ""
                            : reader.Result.GetString("NombrePersonal"),

                        Concepto = await reader.Result.IsDBNullAsync("Concepto")
                            ? ""
                            : reader.Result.GetString("Concepto"),
                        NumeroRecibo = await reader.Result.IsDBNullAsync("NumeroRecibo")
                            ? ""
                            : reader.Result.GetString("NumeroRecibo"),
                        Monto = await reader.Result.IsDBNullAsync("Monto")
                            ? 0
                            : reader.Result.GetDecimal("Monto"),
                    };

                    listEgresos.Add(egreso);
                }
                Log.Debug("Se han obtenido con éxito la información de los Ingresos.");

                return listEgresos;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los Ingresos.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los clientes. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que regresa una lista de los egresos del dia.
        /// </summary>
        /// <returns>Una lista de egresos</returns>
        public static Task<List<Egresos>> GetEgresosToday() {
            var listEgresos = new List<Egresos>();

            try {
                var egresos =
                    InitInfo.ObCoEgresos.Where(x => x.FechaRegistro.Date == DateTime.Today.Date).ToList();

                return Task.FromResult(egresos);
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los Ingresos.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los clientes. Error: {e.Message}",
                    "Error desconocido");
                return Task.FromResult(listEgresos);
            }
        }

        /// <summary>
        /// Método que regresa una lista de los ingresos del dia.
        /// </summary>
        /// <returns>Una lista de egresos</returns>
        public static Task<List<Ingresos>> GetIngresosToday() {
            var listIngresos = new List<Ingresos>();

            try {
                var ingresosList =
                    InitInfo.ObCoIngresos.Where(x => x.FechaRegistro.Date == DateTime.Today.Date).ToList();

                return Task.FromResult(ingresosList);
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los Ingresos.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los clientes. Error: {e.Message}",
                    "Error desconocido");
                return Task.FromResult(listIngresos);
            }
        }
        
    }
}