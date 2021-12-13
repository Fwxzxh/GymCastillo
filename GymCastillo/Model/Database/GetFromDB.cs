﻿using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
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
        public static async Task<ObservableCollection<Cliente>> GetClientes() {
            Log.Debug("se ha empezado el proceso de obtener la información de los Clientes.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Se ha creado la conexión.");

            const string sqlQuery = @"SELECT
                                          c.IdCliente, c.Nombre, c.ApellidoMaterno,
                                          c.ApellidoPaterno, c.FechaNacimiento, c.Telefono, 
                                          c.NombreContacto, c.TelefonoContacto, c.Foto, 
                                          c.CondicionEspecial, c.DescripcionCondicionEspecial,
                                          c.FechaUltimoAcceso, c.MontoUltimoPago, c.Activo, 
                                          c.FechaUltimoPago, c.FechaVencimientoPago, 
                                          c.DeudaCliente, c.MedioConocio,
                                          c.ClasesTotalesDisponibles, c.ClasesSemanaDisponibles,
                                          c.DuracionPaquete, c.Nino,
                                          p.IdPaquete, p.NombrePaquete,
                                          tc.IdTipoCliente, tc.NombreTipoCliente,
                                          l.IdLocker, l.Nombre as NombreLocker
                                      FROM cliente c
                                      INNER JOIN paquete p ON c.IdPaquete = p.IdPaquete
                                      INNER JOIN TipoCliente tc ON c.IdTipoCliente = tc.IdTipoCliente
                                      LEFT JOIN locker l ON c.IdLocker = l.IdLocker";

            var listCliente = new ObservableCollection<Cliente>();

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                while (await reader.Result.ReadAsync()) {
                    var cliente = new Cliente() {
                        Id = reader.Result.GetInt32("IdCliente"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre")
                            ? ""
                            : reader.Result.GetString("Nombre"),
                        ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno")
                            ? ""
                            : reader.Result.GetString("ApellidoMaterno"),

                        ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno")
                            ? ""
                            : reader.Result.GetString("ApellidoPaterno"),
                        FechaNacimiento = reader.Result.GetDateTime("FechaNacimiento"),
                        Telefono = await reader.Result.IsDBNullAsync("Telefono")
                            ? ""
                            : reader.Result.GetString("Telefono"),

                        NombreContacto = await reader.Result.IsDBNullAsync("NombreContacto")
                            ? ""
                            : reader.Result.GetString("NombreContacto"),
                        TelefonoContacto = await reader.Result.IsDBNullAsync("TelefonoContacto")
                            ? ""
                            : reader.Result.GetString("TelefonoContacto"),

                        //Foto = await reader.Result.IsDBNullAsync("Foto") ? null : reader.Result.GetBytes("Foto"), TODO: Ver como obtener la foto.

                        CondicionEspecial = !await reader.Result.IsDBNullAsync("CondicionEspecial") &&
                                            reader.Result.GetBoolean("CondicionEspecial"),
                        DescripciónCondiciónEspecial = await reader.Result.IsDBNullAsync("DescripcionCondicionEspecial")
                            ? ""
                            : reader.Result.GetString("DescripcionCondicionEspecial"),

                        FechaUltimoAcceso = reader.Result.GetDateTime("FechaUltimoAcceso"),
                        MontoUltimoPago = await reader.Result.IsDBNullAsync("MontoUltimoPago")
                            ? 0
                            : reader.Result.GetDecimal("MontoUltimoPago"),
                        Activo = !await reader.Result.IsDBNullAsync("Activo") &&
                                 reader.Result.GetBoolean("Activo"),

                        FechaVencimientoPago = reader.Result.GetDateTime("FechaVencimientoPago"),

                        DeudaCliente = await reader.Result.IsDBNullAsync("DeudaCliente")
                            ? 0
                            : reader.Result.GetDecimal("DeudaCliente"),
                        MedioConocio = await reader.Result.IsDBNullAsync("MedioConocio")
                            ? ""
                            : reader.Result.GetString("MedioConocio"),

                        ClasesTotalesDisponibles = await reader.Result.IsDBNullAsync("ClasesTotalesDisponibles")
                            ? 0
                            : reader.Result.GetInt16("ClasesTotalesDisponibles"),
                        ClasesSemanaDisponibles = await reader.Result.IsDBNullAsync("ClasesSemanaDisponibles")
                            ? 0
                            : reader.Result.GetInt16("ClasesSemanaDisponibles"),

                        DuraciónPaquete = await reader.Result.IsDBNullAsync("DuracionPaquete")
                            ? 0
                            : reader.Result.GetInt32("DuracionPaquete"),
                        Niño = reader.Result.GetBoolean("nino"),

                        IdPaquete = await reader.Result.IsDBNullAsync("IdPaquete")
                            ? 0
                            : reader.Result.GetInt32("IdPaquete"),
                        NombrePaquete = await reader.Result.IsDBNullAsync("NombrePaquete")
                            ? ""
                            : reader.Result.GetString("NombrePaquete"),

                        IdTipoCliente = await reader.Result.IsDBNullAsync("IdTipoCliente")
                            ? 0
                            : reader.Result.GetInt32("IdTipoCliente"),
                        NombreTipoCliente = await reader.Result.IsDBNullAsync("NombreTipoCliente")
                            ? ""
                            : reader.Result.GetString("NombreTipoCliente"),

                        IdLocker = await reader.Result.IsDBNullAsync("IdLocker")
                            ? 0
                            : reader.Result.GetInt16("IdLocker"),
                        NombreLocker = await reader.Result.IsDBNullAsync("NombreLocker")
                            ? ""
                            : reader.Result.GetString("NombreLocker")
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
                    $"Ha ocurrido un error desconocido al obtener la información de los clientes. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que se encarga de obtener todos los datos de los instructores
        /// </summary>
        /// <returns>Una lista de objetos tipo Instructor</returns>
        public static async Task<ObservableCollection<Instructor>> GetInstructores() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los Instructores.");

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
                                          i.SueldoADescontar, i.MetodoFechaPago,
                                          ti.IdTipoInstructor, ti.NombreTipoInstructor,
                                          group_concat(c.IdClase) as IdClase, group_concat(c.NombreClase) as NombreClase
                                      FROM instructor i
                                      INNER JOIN TipoInstructor ti ON i.IdTipoInstructor = ti.IdTipoInstructor
                                      LEFT JOIN clase c ON c.IdInstructor = i.IdInstructor
                                      GROUP BY i.IdInstructor";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listInstructores = new ObservableCollection<Instructor>();

                while (await reader.Result.ReadAsync()) {
                    var instructor = new Instructor() {
                        Id = reader.Result.GetInt32("IdInstructor"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre")
                            ? ""
                            : reader.Result.GetString("Nombre"),
                        ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno")
                            ? ""
                            : reader.Result.GetString("ApellidoPaterno"),

                        ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno")
                            ? ""
                            : reader.Result.GetString("ApellidoMaterno"),
                        Domicilio = await reader.Result.IsDBNullAsync("Domicilio")
                            ? ""
                            : reader.Result.GetString("Domicilio"),
                        FechaNacimiento =reader.Result.GetDateTime("FechaNacimiento"),

                        Telefono = await reader.Result.IsDBNullAsync("Telefono")
                            ? ""
                            : reader.Result.GetString("Telefono"),
                        NombreContacto = await reader.Result.IsDBNullAsync("NombreContacto")
                            ? ""
                            : reader.Result.GetString("NombreContacto"),
                        TelefonoContacto = await reader.Result.IsDBNullAsync("TelefonoContacto")
                            ? ""
                            : reader.Result.GetString("TelefonoContacto"),

                        // Foto = reader.Result.GetDateTime("FechaUltimoAcceso"), TODO Foto
                        FechaUltimoAcceso = reader.Result.GetDateTime("FechaUltimoAcceso"),
                        FechaUltimoPago = reader.Result.GetDateTime("FechaUltimoPago"),

                        MontoUltimoPago = await reader.Result.IsDBNullAsync("MontoUltimoPago")
                            ? 0
                            : reader.Result.GetDecimal("MontoUltimoPago"),
                        HoraEntrada = DateTime.ParseExact(reader.Result.GetString("HoraEntrada"), "HHmm",
                            CultureInfo.InvariantCulture),
                        HoraSalida = DateTime.ParseExact(reader.Result.GetString("HoraSalida"), "HHmm",
                            CultureInfo.InvariantCulture),

                        DiasATrabajar = await reader.Result.IsDBNullAsync("DiasATrabajar")
                            ? 0
                            : reader.Result.GetInt32("DiasATrabajar"),
                        DiasTrabajados = await reader.Result.IsDBNullAsync("DiasTrabajados")
                            ? 0
                            : reader.Result.GetInt32("DiasTrabajados"),
                        Sueldo = await reader.Result.IsDBNullAsync("Sueldo")
                            ? 0
                            : reader.Result.GetDecimal("Sueldo"),

                        SueldoADescontar = await reader.Result.IsDBNullAsync("SueldoADescontar")
                            ? 0
                            : reader.Result.GetDecimal("SueldoADescontar"),

                        MétodoFechaPago = await reader.Result.IsDBNullAsync("MetodoFechaPago")
                            ? 0
                            : reader.Result.GetInt32("MetodoFechaPago"),

                        IdTipoInstructor = await reader.Result.IsDBNullAsync("IdTipoInstructor")
                            ? 0
                            : reader.Result.GetInt32("IdTipoInstructor"),
                        NombreTipoInstructor = await reader.Result.IsDBNullAsync("NombreTipoInstructor")
                            ? ""
                            : reader.Result.GetString("NombreTipoInstructor"),

                        IdClase = await reader.Result.IsDBNullAsync("IdClase")
                            ? ""
                            : reader.Result.GetString("IdClase"),
                        NombreClases = await reader.Result.IsDBNullAsync("NombreClase")
                            ? ""
                            : reader.Result.GetString("NombreClase"),
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
                    $"Ha ocurrido un error desconocido al obtener la información de los instructores. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Clase que se encarga de obtener todos los datos de los Usuarios
        /// </summary>
        /// <returns>Una lista de objetos tipo Usuario.</returns>
        public static async Task<ObservableCollection<Usuario>> GetUsuarios() {
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
                Log.Debug("Ejecutamos la query.");

                var listUsuario = new ObservableCollection<Usuario>();

                while (await reader.Result.ReadAsync()) {
                    var usuario = new Usuario() {
                        Id = reader.Result.GetInt32("IdUsuario"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre")
                            ? ""
                            : reader.Result.GetString("Nombre"),
                        ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno")
                            ? ""
                            : reader.Result.GetString("ApellidoPaterno"),

                        ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno")
                            ? ""
                            : reader.Result.GetString("ApellidoMaterno"),
                        Domicilio = await reader.Result.IsDBNullAsync("Domicilio")
                            ? ""
                            : reader.Result.GetString("Domicilio"),
                        Username = await reader.Result.IsDBNullAsync("Username")
                            ? ""
                            : reader.Result.GetString("Username"),

                        Password = await reader.Result.IsDBNullAsync("Password")
                            ? ""
                            : reader.Result.GetString("Password"),
                        FechaNacimiento = await reader.Result.IsDBNullAsync("FechaNacimiento")
                            ? DateTime.Parse("00:00")
                            : reader.Result.GetDateTime("FechaNacimiento"),
                        Telefono = await reader.Result.IsDBNullAsync("Telefono")
                            ? ""
                            : reader.Result.GetString("Telefono"),

                        NombreContacto = await reader.Result.IsDBNullAsync("NombreContacto")
                            ? ""
                            : reader.Result.GetString("NombreContacto"),
                        TelefonoContacto = await reader.Result.IsDBNullAsync("TelefonoContacto")
                            ? ""
                            : reader.Result.GetString("TelefonoContacto"),

                        //Foto = reader.Result.get("Foto"), // Ver que onda con la foto.
                        FechaUltimoAcceso = await reader.Result.IsDBNullAsync("FechaUltimoAcceso")
                            ? DateTime.Parse("00:00")
                            : reader.Result.GetDateTime("FechaUltimoAcceso"),

                        FechaUltimoPago = await reader.Result.IsDBNullAsync("FechaUltimoPago")
                            ? DateTime.Parse("00:00")
                            : reader.Result.GetDateTime("FechaUltimoPago"),
                        MontoUltimoPago = await reader.Result.IsDBNullAsync("MontoUltimoPago")
                            ? 0
                            : reader.Result.GetInt32("MontoUltimoPago"),
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
                    $"Ha ocurrido un error desconocido al obtener la información de los usuarios. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene toda la información sobre los clientes renta.
        /// </summary>
        /// <returns>Una lista de objetos tipo ClienteRenta</returns>
        public static async Task<ObservableCollection<ClienteRenta>> GetClientesRenta() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los clientes renta.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"select * from ClienteRenta";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listClienteRenta = new ObservableCollection<ClienteRenta>();

                while (await reader.Result.ReadAsync()) {
                    var clienteRenta = new ClienteRenta() {
                        Id = reader.Result.GetInt32("IdClienteRenta"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre")
                            ? ""
                            : reader.Result.GetString("Nombre"),
                        ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno")
                            ? ""
                            : reader.Result.GetString("ApellidoPaterno"),

                        ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno")
                            ? ""
                            : reader.Result.GetString("ApellidoMaterno"),
                        Domicilio = await reader.Result.IsDBNullAsync("Domicilio")
                            ? ""
                            : reader.Result.GetString("Domicilio"),
                        FechaNacimiento = reader.Result.GetDateTime("FechaNacimiento"),

                        Telefono = await reader.Result.IsDBNullAsync("Telefono")
                            ? ""
                            : reader.Result.GetString("Telefono"),
                        NombreContacto = await reader.Result.IsDBNullAsync("NombreContacto")
                            ? ""
                            : reader.Result.GetString("NombreContacto"),
                        TelefonoContacto = await reader.Result.IsDBNullAsync("TelefonoContacto")
                            ? ""
                            : reader.Result.GetString("TelefonoContacto"),

                        //Foto = reader.Result.get("Foto"), // Ver que onda con la foto.
                        FechaUltimoPago = reader.Result.GetDateTime("FechaUltimoPago"),
                        MontoUltimoPago = await reader.Result.IsDBNullAsync("MontoUltimoPago")
                            ? 0
                            : reader.Result.GetInt32("MontoUltimoPago"),

                        DeudaCliente = await reader.Result.IsDBNullAsync("DeudaCliente")
                            ? 0
                            : reader.Result.GetInt32("DeudaCliente"),
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
                    $"Ha ocurrido un error desconocido al obtener la información de los clientes renta. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene toda la información sobre los paquetes.
        /// </summary>
        /// <returns>Una lista de objetos tipo Paquete.</returns>
        public static async Task<ObservableCollection<Paquete>> GetPaquetes() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los paquetes.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"SELECT
                                          p.IdPaquete, p.Gym, p.NombrePaquete,
                                          p.Descripcion, p.NumClasesTotales,
                                          p.NumClasesSemanales, p.Costo,
                                          c.IdClase, c.NombreClase
                                      FROM paquete p
                                      LEFT JOIN clase c ON c.IdPaquete = p.IdPaquete";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listPaquetes = new ObservableCollection<Paquete>();

                while (await reader.Result.ReadAsync()) {
                    var paquete = new Paquete() {
                        IdPaquete = reader.Result.GetInt32("IdPaquete"),
                        Gym = !await reader.Result.IsDBNullAsync("Gym") &&
                              reader.Result.GetBoolean("Gym"),
                        NombrePaquete = await reader.Result.IsDBNullAsync("NombrePaquete")
                            ? ""
                            : reader.Result.GetString("NombrePaquete"),

                        Descripcion = await reader.Result.IsDBNullAsync("Descripcion")
                            ? ""
                            : reader.Result.GetString("Descripcion"),
                        NumClasesTotales = await reader.Result.IsDBNullAsync("NumClasesTotales")
                            ? 0
                            : reader.Result.GetInt32("NumClasesTotales"),

                        NumClasesSemanales = await reader.Result.IsDBNullAsync("NumClasesSemanales")
                            ? 0
                            : reader.Result.GetInt32("NumClasesSemanales"),
                        Costo = await reader.Result.IsDBNullAsync("Costo")
                            ? 0
                            : reader.Result.GetInt32("Costo"),

                        IdClase = await reader.Result.IsDBNullAsync("IdClase")
                            ? 0
                            : reader.Result.GetInt32("IdClase"),
                        NombreClase = await reader.Result.IsDBNullAsync("NombreClase")
                            ? ""
                            : reader.Result.GetString("NombreClase"),
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
                    $"Ha ocurrido un error desconocido al obtener la información de los paquetes. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene toda la información sobre los tipos de cliente.
        /// </summary>
        /// <returns>Una lista de objetos tipo Tipo</returns>
        public static async Task<ObservableCollection<Tipo>> GetTipoCliente() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los tipos de cliente.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"select * from TipoCliente";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listTipo = new ObservableCollection<Tipo>();

                while (await reader.Result.ReadAsync()) {
                    var tipo = new Tipo() {
                        IdTipo = reader.Result.GetInt32("IdTipoCliente"),
                        NombreTipo = await reader.Result.IsDBNullAsync("NombreTipoCliente")
                            ? ""
                            : reader.Result.GetString("NombreTipoCliente"),
                        Descripcion = await reader.Result.IsDBNullAsync("Descripcion")
                            ? ""
                            : reader.Result.GetString("Descripcion"),
                    };
                    listTipo.Add(tipo);

                }
                Log.Debug("Se han obtenido con éxito la información de los tipos de cliente.");

                return listTipo;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los tipos de clientes.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los tipos de cliente. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene toda la información de los tipos de instructor.
        /// </summary>
        /// <returns>Una lista de objetos tipo</returns>
        public static async Task<ObservableCollection<Tipo>> GetTipoInstructor() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los tipos de Instructor.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"select * from TipoInstructor";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listTipo = new ObservableCollection<Tipo>();

                while (await reader.Result.ReadAsync()) {
                    var tipo = new Tipo() {
                        IdTipo = reader.Result.GetInt32("IdTipoInstructor"),
                        NombreTipo = await reader.Result.IsDBNullAsync("NombreTipoInstructor")
                            ? ""
                            : reader.Result.GetString("NombreTipoInstructor"),
                        Descripcion = await reader.Result.IsDBNullAsync("Descripcion")
                            ? ""
                            : reader.Result.GetString("Descripcion"),
                    };
                    listTipo.Add(tipo);

                }
                Log.Debug("Se han obtenido con éxito la información de los tipos de instructor.");

                return listTipo;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los tipos de instructor.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los tipos de instructor. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que se encarga de obtener toda la información de los lockers.
        /// </summary>
        /// <param name="onlyOpen">Indica si se deben de dar solo los lockers disponibles.</param>
        /// <returns>Una lista de objetos tipo Locker</returns>
        public static async Task<ObservableCollection<Locker>> GetLockers(bool onlyOpen=false) {
            Log.Debug("Se ha empezado el proceso de obtener la información de los Lockers.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            var sqlQuery = !onlyOpen ?
                @"select * from locker" :
                @"select * from locker where Ocupado=false";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listLocker = new ObservableCollection<Locker>();

                while (await reader.Result.ReadAsync()) {
                    var locker = new Locker() {
                        IdLocker = reader.Result.GetInt32("IdLocker"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre")
                            ? ""
                            : reader.Result.GetString("Nombre"),
                        Ocupado = !await reader.Result.IsDBNullAsync("Ocupado")
                                  && reader.Result.GetBoolean("Ocupado"),
                    };
                    listLocker.Add(locker);
                }
                Log.Debug("Se han obtenido con éxito la información de los lockers.");

                return listLocker;

            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los lockers..");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los tipos de instructor. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene todas las clases.
        /// </summary>
        /// <returns>Una lista que contiene objetos tipo Clase.</returns>
        public static async Task<ObservableCollection<Clase>> GetClases() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los Lockers.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"select
                                          c.IdClase, c.NombreClase, c.Descripcion,
                                          c.CupoMaximo, c.Activo,
                                          i.IdInstructor, CONCAT(i.Nombre, ' ', i.ApellidoPaterno, ' ', i.ApellidoMaterno) as NombreInstructor,
                                          e.IdEspacio, e.NombreEspacio,
                                          c.IdPaquete, p.NombrePaquete
                                      from clase c
                                      left join instructor i on c.IdInstructor = i.IdInstructor
                                      left join espacio e on e.IdEspacio = c.IdEspacio
                                      left join paquete p on c.IdPaquete = p.IdPaquete;";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listClases = new ObservableCollection<Clase>();

                while (await reader.Result.ReadAsync()) {
                    var locker = new Clase() {
                        IdClase = reader.Result.GetInt32("IdClase"),
                        NombreClase = await reader.Result.IsDBNullAsync("NombreClase")
                            ? ""
                            : reader.Result.GetString("NombreClase"),
                        Descripcion = await reader.Result.IsDBNullAsync("Descripcion")
                            ? ""
                            : reader.Result.GetString("Descripcion"),

                        CupoMaximo = await reader.Result.IsDBNullAsync("CupoMaximo")
                            ? 0
                            : reader.Result.GetInt32("CupoMaximo"),
                        Activo = !await reader.Result.IsDBNullAsync("Activo")
                                 && reader.Result.GetBoolean("Activo"),

                        IdInstructor = await reader.Result.IsDBNullAsync("IdInstructor")
                            ? 0
                            : reader.Result.GetInt32("IdInstructor"),
                        NombreInstructor = await reader.Result.IsDBNullAsync("NombreInstructor")
                            ? ""
                            : reader.Result.GetString("NombreInstructor"),

                        IdEspacio = await reader.Result.IsDBNullAsync("IdEspacio")
                            ? 0
                            : reader.Result.GetInt32("IdEspacio"),
                        NombreEspacio = await reader.Result.IsDBNullAsync("NombreEspacio")
                            ? ""
                            : reader.Result.GetString("NombreEspacio"),

                        IdPaquete = await reader.Result.IsDBNullAsync("IdPaquete")
                            ? 0
                            : reader.Result.GetInt32("IdPaquete"),
                        NombrePaquete = await reader.Result.IsDBNullAsync("NombrePaquete")
                            ? ""
                            : reader.Result.GetString("NombrePaquete"),
                    };
                    listClases.Add(locker);
                }
                Log.Debug("Se han obtenido con éxito la información de las clases.");

                return listClases;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los lockers..");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los tipos de instructor. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene todos los horarios.
        /// </summary>
        /// <returns>Una lista con objetos tipo <c>Horarios</c></returns>
        public static async Task<ObservableCollection<Horario>> GetHorarios() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los Horarios.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"select * from horario";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listHorarios = new ObservableCollection<Horario>();

                while (await reader.Result.ReadAsync()) {
                    var horario = new Horario() {
                        IdHorario = reader.Result.GetInt32("IdHorario"),
                        Dia = await reader.Result.IsDBNullAsync("Dia")
                            ? 0
                            : reader.Result.GetInt32("Dia"),

                        HoraInicio = DateTime.ParseExact(reader.Result.GetString("HoraInicio"), "HHmm",
                            CultureInfo.InvariantCulture),
                        HoraFin = DateTime.ParseExact(reader.Result.GetString("HoraFin"), "HHmm",
                            CultureInfo.InvariantCulture),

                        CupoActual = await reader.Result.IsDBNullAsync("CupoActual")
                            ? 0
                            : reader.Result.GetInt32("CupoActual"),
                        IdClase = await reader.Result.IsDBNullAsync("IdClase")
                            ? 0
                            : reader.Result.GetInt32("IdClase"),
                    };
                    listHorarios.Add(horario);
                }
                Log.Debug("Se han obtenido con éxito la información de los horarios.");

                return listHorarios;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los Horarios.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los Horarios. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que se obtiene todos los espacios.
        /// </summary>
        /// <returns>Una lista con objetos tipo <c>Espacio</c></returns>
        public static async Task<ObservableCollection<Espacio>> GetEspacios() {
            Log.Debug("Se ha empezado el proceso de obtener la información de los espacios.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"select * from espacio";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listEspacios = new ObservableCollection<Espacio>();

                while (await reader.Result.ReadAsync()) {
                    var espacio = new Espacio() {
                        IdEspacio = reader.Result.GetInt32("IdEspacio"),
                        NombreEspacio = await reader.Result.IsDBNullAsync("NombreEspacio")
                            ? ""
                            : reader.Result.GetString("NombreEspacio"),

                        Descripción = await reader.Result.IsDBNullAsync("Descripcion")
                            ? ""
                            : reader.Result.GetString("Descripcion"),
                    };
                    listEspacios.Add(espacio);
                }
                Log.Debug("Se han obtenido con éxito la información de los espacios.");

                return listEspacios;

            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener la información de los Espacios.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al obtener la información de los Espacios. Error: {e.Message}",
                    "Error desconocido");
                throw; // -> manejamos el error en el siguiente nivel.
            }
        }

        /// <summary>
        /// Método que obtiene todos los ingresos.
        /// </summary>
        /// <returns>Una lista con objetos tipo ingresos.</returns>
        public static async Task<ObservableCollection<Ingresos>> GetIngresos() {
            Log.Debug("Se ha empezado el proceso de obtener todos los ingresos.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"SELECT
                                          i.IdIngresos, i.FechaRegistro,
                                          i.IdUsuario, CONCAT(u.Nombre, ' ', u.ApellidoPaterno, ' ', u.ApellidoMaterno) as NombreUsuario,
                                          i.IdRenta, r.FechaRenta, i.IdCliente,
                                          CONCAT(c.Nombre, ' ', c.ApellidoPaterno, ' ', c.ApellidoMaterno) as NombreCliente, i.IdVenta,
                                          i.Otros, i.Concepto,
                                          i.IdPaquete, p.NombrePaquete,
                                          i.IdLocker, l.Nombre,
                                          i.NumeroRecibo, i.Monto
                                      FROM ingresos i
                                      INNER JOIN usuario u ON i.IdUsuario = u.IdUsuario
                                      LEFT JOIN rentas r ON i.IdRenta = r.IdRenta
                                      LEFT JOIN cliente c ON i.IdCliente = c.IdCliente
                                      LEFT JOIN ventas v ON i.IdVenta = v.IdVenta
                                      LEFT JOIN paquete p ON i.IdPaquete = p.IdPaquete
                                      LEFT JOIN locker l ON i.IdLocker = l.IdLocker;";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listIngresos = new ObservableCollection<Ingresos>();

                while (await reader.Result.ReadAsync()) {
                    var ingreso = new Ingresos() {
                        IdMovimiento = reader.Result.GetInt32("IdIngresos"),
                        FechaRegistro = reader.Result.GetDateTime("FechaRegistro"),

                        IdUsuario = await reader.Result.IsDBNullAsync("IdUsuario")
                            ? 0
                            : reader.Result.GetInt32("IdUsuario"),
                        NombreUsuario = await reader.Result.IsDBNullAsync("NombreUsuario")
                            ? ""
                            : reader.Result.GetString("NombreUsuario"),

                        IdRenta = await reader.Result.IsDBNullAsync("IdRenta")
                            ? 0
                            : reader.Result.GetInt32("IdRenta"),
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

                        Otros = !await reader.Result.IsDBNullAsync("Otros") &&
                                reader.Result.GetBoolean("Otros"),
                        Concepto = await reader.Result.IsDBNullAsync("Concepto")
                            ? ""
                            : reader.Result.GetString("Concepto"),

                        IdPaquete = await reader.Result.IsDBNullAsync("IdPaquete")
                            ? 0
                            : reader.Result.GetInt32("IdPaquete"),
                        NombrePaquete = await reader.Result.IsDBNullAsync("NombrePaquete")
                            ? ""
                            : reader.Result.GetString("NombrePaquete"),

                        IdLocker = await reader.Result.IsDBNullAsync("IdLocker")
                            ? 0
                            : reader.Result.GetInt32("IdLocker"),
                        NombreLocker = await reader.Result.IsDBNullAsync("Nombre")
                            ? ""
                            : reader.Result.GetString("Nombre"),

                        NumeroRecibo = await reader.Result.IsDBNullAsync("NumeroRecibo")
                            ? ""
                            : reader.Result.GetString("NumeroRecibo"),
                        Monto = await reader.Result.IsDBNullAsync("Monto")
                            ? 0
                            : reader.Result.GetDecimal("Monto"),
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
        /// Método que obtiene una lista de todos los egresos.
        /// </summary>
        /// <returns>Una lista con objetos tipo egresos.</returns>
        public static async Task<ObservableCollection<Egresos>> GetEgresos() {
            Log.Debug("Se ha empezado el proceso de obtener todos los ingresos.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();
            Log.Debug("Creamos la conexión.");

            const string sqlQuery = @"SELECT
                                          p.IdPagosGeneral, p.FechaRegistro,
                                          p.IdUsuario, CONCAT(u.Nombre, ' ', u.ApellidoPaterno, ' ', u.ApellidoMaterno) as NombreUsuario,
                                          p.Servicios, p.Nomina, p.IdUsuarioPagar,
                                          CONCAT(up.Nombre, ' ', up.ApellidoPaterno, ' ', up.ApellidoMaterno) as NombreUsuarioPagar,
                                          p.IdInstructor, CONCAT(i.Nombre, ' ', i.ApellidoPaterno, ' ', i.ApellidoMaterno) as NombreInstructor,
                                          p.Otros, p.Concepto, p.NumeroRecibo, p.Monto
                                      FROM egresos p
                                      INNER JOIN usuario u ON p.IdUsuario = u.IdUsuario
                                      LEFT JOIN usuario up ON p.IdUsuarioPagar = up.IdUsuario
                                      LEFT JOIN instructor i ON p.IdInstructor = i.IdInstructor;";

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();
                Log.Debug("Ejecutamos la query.");

                var listEgresos = new ObservableCollection<Egresos>();

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

                        Otros = !await reader.Result.IsDBNullAsync("Otros") &&
                                reader.Result.GetBoolean("Otros"),
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
    }
}