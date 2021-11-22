using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Documents;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.Database {
    /// <summary>
    /// Clase que se encarga de obtener los datos necesarios de la base de datos.
    /// </summary>
    public static class GetFromDb {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de obtener todos los datos de los clientes.
        /// </summary>
        /// <returns>Una lista de objetos tipo Cliente.</returns>
        public static async Task<List<Cliente>> GetClientes() {
            Log.Debug("se ha empezado el proceso de obtener la información de los Clientes.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Se ha creado la conexión.");

            const string sqlQuery = @"SELECT 
                                            c.IdCliente, c.Nombre, c.ApellidoMaterno, 
                                            c.ApellidoPaterno, c.Domicilio, c.FechaNacimiento, 
                                            c.Telefono, c.NombreContacto, c.TelefonoContacto, 
                                            c.Foto, c.CondicionEspecial, c.FechaUltimoAcceso,
                                            c.MontoUltimoPago, c.Activo, c.FechaVencimientoPago, 
                                            c.DeudaCliente, c.MedioConocio,
                                            c.ClasesTotalesDisponibles, c.ClasesSemanaDisponibles, 
                                            c.Descuento, c.Nino, 
                                            p.IdPaquete, p.NombrePaquete,
                                            tc.IdTipoCliente, tc.NombreTipoCliente,
                                            l.IdLocker, l.Nombre as NombreLocker
                                      FROM cliente c
                                      INNER JOIN paquete p ON c.IdPaquete = p.IdPaquete
                                      INNER JOIN tipocliente tc ON c.IdTipoCliente = tc.IdTipoCliente
                                      LEFT JOIN locker l ON c.IdCliente = l.IdCliente";

            var listCliente = new List<Cliente>();

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Executamos la query.");

                while (await reader.Result.ReadAsync()) {
                    var cliente = new Cliente() {
                        Id = reader.Result.GetInt32("IdCliente"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre") ? "" : reader.Result.GetString("Nombre"),
                        ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno") ? "" : reader.Result.GetString("ApellidoMaterno"),

                        ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno") ? "" : reader.Result.GetString("ApellidoPaterno"),
                        Domicilio = await reader.Result.IsDBNullAsync("Domicilio") ? "" : reader.Result.GetString("Domicilio"),
                        FechaNacimiento = reader.Result.GetDateTime("FechaNacimiento"),

                        Telefono = await reader.Result.IsDBNullAsync("Telefono") ? "" : reader.Result.GetString("Telefono"),
                        NombreContacto = await reader.Result.IsDBNullAsync("NombreContacto") ? "" : reader.Result.GetString("NombreContacto"),
                        TelefonoContacto = await reader.Result.IsDBNullAsync("TelefonoContacto") ? "" : reader.Result.GetString("TelefonoContacto"),

                        //Foto = await reader.Result.IsDBNullAsync("Foto") ? null : reader.Result.GetBytes("Foto"), TODO: Ver como obtener la foto.
                        CondicionEspecial = !await reader.Result.IsDBNullAsync("CondicionEspecial") && reader.Result.GetBoolean("CondicionEspecial"),
                        FechaUltimoAcceso = reader.Result.GetDateTime("FechaUltimoAcceso"),

                        MontoUltimoPago = await reader.Result.IsDBNullAsync("MontoUltimoPago") ? 0 : reader.Result.GetDecimal("MontoUltimoPago"),
                        Activo = !await reader.Result.IsDBNullAsync("Activo") && reader.Result.GetBoolean("Activo"),
                        FechaVencimientoPago = reader.Result.GetDateTime("FechaVencimientoPago"),

                        DeudaCliente = await reader.Result.IsDBNullAsync("DeudaCliente") ? 0 : reader.Result.GetDecimal("DeudaCliente"),
                        MedioConocio = await reader.Result.IsDBNullAsync("MedioConocio") ? "" : reader.Result.GetString("MedioConocio"),

                        ClasesTotalesDisponibles = await reader.Result.IsDBNullAsync("ClasesTotalesDisponibles") ? 0 : reader.Result.GetInt16("ClasesTotalesDisponibles"),
                        ClasesSemanaDisponibles = await reader.Result.IsDBNullAsync("ClasesSemanaDisponibles") ? 0 : reader.Result.GetInt16("ClasesSemanaDisponibles"),

                        Descuento = await reader.Result.IsDBNullAsync("Descuento") ? 0 : reader.Result.GetDecimal("Descuento"),
                        Niño = reader.Result.GetBoolean("nino"),

                        IdPaquete = await reader.Result.IsDBNullAsync("IdPaquete") ? 0 : reader.Result.GetInt32("IdPaquete"),
                        NombrePaquete = await reader.Result.IsDBNullAsync("NombrePaquete") ? "" : reader.Result.GetString("NombrePaquete"),

                        IdTipoCliente = await reader.Result.IsDBNullAsync("IdTipoCliente") ? 0 : reader.Result.GetInt32("IdTipoCliente"),
                        NombreTipoCliente = await reader.Result.IsDBNullAsync("NombreTipoCliente") ? "" : reader.Result.GetString("NombreTipoCliente"),

                        IdLocker = await reader.Result.IsDBNullAsync("IdLocker") ? 0 : reader.Result.GetInt16("IdLocker"),
                        NombreLocker = await reader.Result.IsDBNullAsync("NombreLocker") ? "" : reader.Result.GetString("NombreLocker"),
                    };

                    listCliente.Add(cliente);
                }
                Log.Debug("Se han obtenido con éxito la información de los clientes.");

                return listCliente;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los clientes.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconcido al obtener la información de los clientes. Error: {e.Message}",
                    "Error desconcido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que se encarga de obtener todos los datos de los instructores
        /// </summary>
        /// <returns>Una lista de objetos tipo Instructor</returns>
        public static async Task<List<Instructor>> GetInstructores() {
            Log.Debug("Se ha empezado el proceso de obtener la inforamión de los Instructores.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"SELECT
                                          i.IdInstructor, i.Nombre, i.ApellidoPaterno,
                                          i.ApellidoMaterno, i.Domicilio, i.FechaNacimiento,
                                          i.Telefono, i.NombreContacto, i.TelefonoContacto,
                                          i.Foto, i.FechaUltimoAcceso, i.FechaUltimoPago,
                                          i.MontoUltimoPago, i.HoraEntrada, i.HoraSalida,
                                          i.DiasATrabajar, i.DiasTrabajados, i.Sueldo,
                                          i.SueldoADescontar,
                                          ti.IdTipoInstructor, ti.NombreTipoInstructor,
                                          group_concat(c.IdClase) as IdClase, group_concat(c.NombreClase) as NombreClase
                                      FROM instructor i
                                      INNER JOIN tipoinstructor ti ON i.IdTipoInstructor = ti.IdTipoInstructor
                                      LEFT JOIN clase c ON c.IdInstructor = i.IdInstructor
                                      GROUP BY i.IdInstructor";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Executamos la query.");

                var listInstructores = new List<Instructor>();

                while (await reader.Result.ReadAsync()) {
                    var instructor = new Instructor() {
                        Id = await reader.Result.IsDBNullAsync("IdInstructor") ? 0 : reader.Result.GetInt32("IdInstructor"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre") ? "" : reader.Result.GetString("Nombre"),
                        ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno") ? "" : reader.Result.GetString("ApellidoPaterno"),

                        ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno") ? "" : reader.Result.GetString("ApellidoMaterno"),
                        Domicilio = await reader.Result.IsDBNullAsync("Domicilio") ? "" : reader.Result.GetString("Domicilio"),
                        FechaNacimiento =reader.Result.GetDateTime("FechaNacimiento"),

                        Telefono = await reader.Result.IsDBNullAsync("Telefono") ? "" : reader.Result.GetString("Telefono"),
                        NombreContacto = await reader.Result.IsDBNullAsync("NombreContacto") ? "" : reader.Result.GetString("NombreContacto"),
                        TelefonoContacto = await reader.Result.IsDBNullAsync("TelefonoContacto") ? "" : reader.Result.GetString("TelefonoContacto"),

                        // Foto = reader.Result.GetDateTime("FechaUltimoAcceso"), TODO Foto
                        FechaUltimoAcceso = reader.Result.GetDateTime("FechaUltimoAcceso"),
                        FechaUltimoPago = reader.Result.GetDateTime("FechaUltimoPago"),

                        MontoUltimoPago = await reader.Result.IsDBNullAsync("MontoUltimoPago") ? 0 : reader.Result.GetDecimal("MontoUltimoPago"),
                        HoraEntrada = DateTime.ParseExact(reader.Result.GetString("HoraEntrada"), "HHmm", CultureInfo.InvariantCulture), // TODO checar si esto da problemas.
                        HoraSalida = DateTime.ParseExact(reader.Result.GetString("HoraSalida"), "HHmm", CultureInfo.InvariantCulture), // TODO checar si esto da problemas.

                        DiasATrabajar = await reader.Result.IsDBNullAsync("DiasATrabajar") ? 0 : reader.Result.GetInt32("DiasATrabajar"),
                        DiasTrabajados = await reader.Result.IsDBNullAsync("DiasTrabajados") ? 0 : reader.Result.GetInt32("DiasTrabajados"),
                        Sueldo = await reader.Result.IsDBNullAsync("Sueldo") ? 0 : reader.Result.GetDecimal("Sueldo"),

                        SueldoADescontar = await reader.Result.IsDBNullAsync("SueldoADescontar") ? 0 : reader.Result.GetDecimal("SueldoADescontar"),

                        IdTipoInstructor = await reader.Result.IsDBNullAsync("idTipoinstructor") ? 0 : reader.Result.GetInt32("IdTipoInstructor"),
                        NombreTipoInstructor = await reader.Result.IsDBNullAsync("NombeTipoInstructor") ? "" : reader.Result.GetString("NombreTipoInstructor"),

                        IdClase = await reader.Result.IsDBNullAsync("IdClase") ? "" : reader.Result.GetString("IdClase"),
                        NombreClases = await reader.Result.IsDBNullAsync("NombreClase") ? "" : reader.Result.GetString("NombreClase"),
                    };
                    listInstructores.Add(instructor);
                }
                Log.Debug("Se han obtenido con éxito la información de los Instructores");

                return listInstructores;

            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los instructores.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconcido al obtener la información de los instructores. Error: {e.Message}",
                    "Error desconcido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Clase que se encarga de obtener todos los datos de los Usuarios
        /// </summary>
        /// <returns>Una lista de objetos tipo Usuario.</returns>
        public static async Task<List<Usuario>> GetUsuarios() {
            Log.Debug("se ha empezado el proceso de obtener la información de los usuarios.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"SELECT
                                          u.IdUsuario, u.Nombre, u.ApellidoPaterno,
                                          u.ApellidoMaterno, u.Domicilio, u.Username,
                                          u.Password, u.FechaNacimiento, u.Telefono,
                                          u.NombreContacto, u.TelefonoContacto,
                                          u.Foto, u.FechaUltimoAcceso,
                                          u.FechaUltimoPago, u.MontoUltimoPago
                                      FROM usuario u";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Executamos la query.");

                var listUsuario = new List<Usuario>();

                while (await reader.Result.ReadAsync()) {
                    var usuario = new Usuario() {
                        Id = await reader.Result.IsDBNullAsync("IdUsuario") ? 0 : reader.Result.GetInt32("IdUsuario"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre") ? "" : reader.Result.GetString("Nombre"),
                        ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno") ? "" : reader.Result.GetString("ApellidoPaterno"),

                        ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno") ? "" : reader.Result.GetString("ApellidoMaterno"),
                        Domicilio = await reader.Result.IsDBNullAsync("Domicilio") ? "" : reader.Result.GetString("Domicilio"),
                        Username = await reader.Result.IsDBNullAsync("Username") ? "" : reader.Result.GetString("Username"),

                        Password = await reader.Result.IsDBNullAsync("Password") ? "" : reader.Result.GetString("Password"),
                        FechaNacimiento = reader.Result.GetDateTime("FechaNacimiento"),
                        Telefono = await reader.Result.IsDBNullAsync("Telefono") ? "" : reader.Result.GetString("Telefono"),

                        NombreContacto = await reader.Result.IsDBNullAsync("NombreContacto") ? "" : reader.Result.GetString("NombreContacto"),
                        TelefonoContacto = await reader.Result.IsDBNullAsync("TelefonoContacto") ? "" : reader.Result.GetString("TelefonoContacto"),

                        //Foto = reader.Result.get("Foto"), // Ver que onda con la foto.
                        FechaUltimoAcceso = reader.Result.GetDateTime("FechaUltimoAcceso"),

                        FechaUltimoPago = reader.Result.GetDateTime("FechaUltimoPago"),
                        MontoUltimoPago = await reader.Result.IsDBNullAsync("MontoUltimoPago") ? 0 : reader.Result.GetInt32("MontoUltimoPago"),
                    };

                    listUsuario.Add(usuario);
                }
                Log.Debug("Se han obtenido con éxito la información de los usuarios.");

                return listUsuario;

            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los usuarios.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconcido al obtener la información de los usuarios. Error: {e.Message}",
                    "Error desconcido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene toda la información sobre los clientes renta.
        /// </summary>
        /// <returns>Una lista de objetos tipo ClienteRenta</returns>
        public static async Task<List<ClienteRenta>> GetClientesRenta() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los clientes renta.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"SELECT
                                          cr.IdClienteRenta, cr.Nombre, cr.ApellidoPaterno,
                                          cr.ApellidoPaterno, cr.Domicilio, cr.FechaNacimiento,
                                          cr.Telefono, cr.NombreContacto, cr.TelefonoContacto,
                                          cr.Foto, cr.FechaUltimoPago, cr.MontoUltimoPago,
                                          cr.DeudaCliente,
                                          group_concat(r.IdRenta) as IdRenta, group_concat(r.FechaRenta) as FechaRenta, group_concat(r.Costo) as CostoRenta
                                      FROM clienterenta cr, rentas r
                                      WHERE cr.IdClienteRenta = r.IdClienteRenta
                                      GROUP BY IdClienteRenta";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Executamos la query.");

                var listClienteRenta = new List<ClienteRenta>();

                while (await reader.Result.ReadAsync()) {
                    var clienteRenta = new ClienteRenta() {
                        Id = await reader.Result.IsDBNullAsync("IdClienteRenta") ? 0 : reader.Result.GetInt32("IdClienteRenta"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre") ? "" : reader.Result.GetString("Nombre"),
                        ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno") ? "" : reader.Result.GetString("ApellidoPaterno"),

                        ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno") ? "" : reader.Result.GetString("ApellidoMaterno"),
                        Domicilio = await reader.Result.IsDBNullAsync("Domicilio") ? "" : reader.Result.GetString("Domicilio"),
                        FechaNacimiento = reader.Result.GetDateTime("FechaNacimiento"),

                        Telefono = await reader.Result.IsDBNullAsync("Telefono") ? "" : reader.Result.GetString("Telefono"),
                        NombreContacto = await reader.Result.IsDBNullAsync("NombreContacto") ? "" : reader.Result.GetString("NombreContacto"),
                        TelefonoContacto = await reader.Result.IsDBNullAsync("TelefonoContacto") ? "" : reader.Result.GetString("TelefonoContacto"),

                        //Foto = reader.Result.get("Foto"), // Ver que onda con la foto.
                        FechaUltimoPago = reader.Result.GetDateTime("FechaUltimoPago"),
                        MontoUltimoPago = await reader.Result.IsDBNullAsync("MontoUltimoPago") ? 0 : reader.Result.GetInt32("MontoUltimoPago"),

                        DeudaCliente = await reader.Result.IsDBNullAsync("DeudaCliente") ? 0 : reader.Result.GetInt32("DeudaCliente"),

                        IdRenta = await reader.Result.IsDBNullAsync("IdRenta") ? "" : reader.Result.GetString("IdRenta"),
                        FechaRenta = await reader.Result.IsDBNullAsync("FechaRenta") ? "" : reader.Result.GetString("FechaRenta"),
                        CostoRenta = await reader.Result.IsDBNullAsync("CostoRenta") ? "" : reader.Result.GetString("CostoRenta"),
                    };

                    listClienteRenta.Add(clienteRenta);
                }
                Log.Debug("Se han obtenido con éxito la información de los usuarios.");

                return listClienteRenta;

            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los clientes renta.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconcido al obtener la información de los clientes renta. Error: {e.Message}",
                    "Error desconcido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene toda la información sobre los paquetes.
        /// </summary>
        /// <returns>Una lista de objetos tipo Paquete.</returns>
        public static async Task<List<Paquete>> GetPaquetes() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los paquetes.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"select
                                          p.IdPaquete, p.Gym, p.NombrePaquete,
                                          p.NumClasesTotales, p.NumClasesSemanales,
                                          p.Costo, p.IdClase, c.NombreClase
                                      from paquete p
                                      left join clase c on p.IdClase = c.IdClase";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Executamos la query.");

                var listPaquetes = new List<Paquete>();

                while (await reader.Result.ReadAsync()) {
                    var paquete = new Paquete() {
                        IdPaquete = await reader.Result.IsDBNullAsync("IdPaquete") ? 0 : reader.Result.GetInt32("IdPaquete"),
                        Gym = !await reader.Result.IsDBNullAsync("Gym") && reader.Result.GetBoolean("Gym"),
                        NombrePaquete = await reader.Result.IsDBNullAsync("NombrePaquete") ? "" : reader.Result.GetString("NombrePaquete"),
                        NumClasesTotales = await reader.Result.IsDBNullAsync("NumClasesTotales") ? 0 : reader.Result.GetInt32("NumClasesTotales"),
                        NumClasesSemanales = await reader.Result.IsDBNullAsync("NumClasesSemanales") ? 0 : reader.Result.GetInt32("NumClasesSemanales"),
                        Costo = await reader.Result.IsDBNullAsync("Costo") ? 0 : reader.Result.GetInt32("Costo"),
                        IdClase = await reader.Result.IsDBNullAsync("IdClase") ? 0 : reader.Result.GetInt32("IdClase"),
                        NombreClase = await reader.Result.IsDBNullAsync("NombreClase") ? "" : reader.Result.GetString("NombreClase"),
                    };
                    listPaquetes.Add(paquete);

                }
                Log.Debug("Se han obtenido con éxito la información de los paquetes.");

                return listPaquetes;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los paquetes.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconcido al obtener la información de los paquetes. Error: {e.Message}",
                    "Error desconcido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene toda la información sobre los tipos de cliente.
        /// </summary>
        /// <returns>Unalista de objetos tipo Tipo</returns>
        public static async Task<List<Tipo>> GetTipoCliente() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los tipos de cliente.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"select * from tipocliente";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Executamos la query.");

                var listTipo = new List<Tipo>();

                while (await reader.Result.ReadAsync()) {
                    var tipo = new Tipo() {
                        IdTipo = await reader.Result.IsDBNullAsync("IdTipoCliente") ? 0 : reader.Result.GetInt32("IdTipoCliente"),
                        NombreTipo = await reader.Result.IsDBNullAsync("NombreTipoCliente") ? "" : reader.Result.GetString("NombreTipoCliente"),
                        Descripcion = await reader.Result.IsDBNullAsync("Descripcion") ? "" : reader.Result.GetString("Descripcion"),
                    };
                    listTipo.Add(tipo);

                }
                Log.Debug("Se han obtenido con éxtio la información de los tipos de cliente.");

                return listTipo;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los tipos de clientes.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconcido al obtener la información de los tipos de cliente. Error: {e.Message}",
                    "Error desconcido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene toda la información de los tipos de instructor.
        /// </summary>
        /// <returns>Una lista de objetos tipo</returns>
        public static async Task<List<Tipo>> GetTipoInstructor() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los tipos de Instructor.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"select * from tipoinstructor";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Executamos la query.");

                var listTipo = new List<Tipo>();

                while (await reader.Result.ReadAsync()) {
                    var tipo = new Tipo() {
                        IdTipo = await reader.Result.IsDBNullAsync("IdTipoInstructor") ? 0 : reader.Result.GetInt32("IdTipoInstructor"),
                        NombreTipo = await reader.Result.IsDBNullAsync("NombreTipoInstructor") ? "" : reader.Result.GetString("NombreTipoInstructor"),
                        Descripcion = await reader.Result.IsDBNullAsync("Descripcion") ? "" : reader.Result.GetString("Descripcion"),
                    };
                    listTipo.Add(tipo);

                }
                Log.Debug("Se han obtenido con éxtio la información de los tipos de instructor.");

                return listTipo;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los tipos de instructor.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconcido al obtener la información de los tipos de instructor. Error: {e.Message}",
                    "Error desconcido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que se encarga de obtener toda la información de los lockers.
        /// </summary>
        /// <param name="onlyOpen">Indica si se deben de dar solo los lockers disponibles.</param>
        /// <returns>Una lista de objetos tipo Locker</returns>
        public static async Task<List<Locker>> GetLockers(bool onlyOpen=false) {

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            var sqlQuery = !onlyOpen ?
                @"select * from locker" :
                @"select * from locker where IdCliente IS NOT NULL";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Executamos la query.");

                var listLocker = new List<Locker>();

                while (await reader.Result.ReadAsync()) {
                    var locker = new Locker() {
                        IdLocker = await reader.Result.IsDBNullAsync("IdLocker") ? 0 : reader.Result.GetInt32("IdLocker"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre") ? "" : reader.Result.GetString("Nombre"),
                        IdCliente = await reader.Result.IsDBNullAsync("IdCliente") ? 0 : reader.Result.GetInt32("IdCliente"),
                    };
                    listLocker.Add(locker);
                }
                Log.Debug("Se han obtenido con éxito la información de los tipos de instructor.");

                return listLocker;

            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los tipos de instructor.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconcido al obtener la información de los tipos de instructor. Error: {e.Message}",
                    "Error desconcido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }
    }
}