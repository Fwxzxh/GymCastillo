using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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

            // TODO: Hacer la query de verdad ahora si
            const string sqlQuery = @"SELECT c.IdCliente, c.Nombre, c.ApellidoMaterno, 
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

            // Obtiene: IdCLiente, Nombre, ApellidoMaterno,
            // ApellidoPaterno, Domicilio, FechaNaciemiento
            // Telefono, NombreContacto, TelefonoContacto
            // Foto, CondicionEspecial, FechaUltimoAcceso,
            // MontoUltimoPago, Activo, FechaVencimientoPago,
            // DeudaCliente, MedioConocio,
            // ClasesTotalesDisponibles, ClasesSemanaDisponibles,
            // Descuento, Nino,
            // IdPaquete, NombrePaquete,
            // IdTipoCliente, NombreTipoCliente
            // IdLocker, Nombre

            var listUsuario = new List<Cliente>();

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();

                while (await reader.Result.ReadAsync()) {
                    var cliente = new Cliente() {
                        Id = reader.Result.GetInt32("IdCliente"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre") ? "" : reader.Result.GetString("Nombre"),
                        ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno") ? "" : reader.Result.GetString("ApellidoMaterno"),

                        ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno") ? "" : reader.Result.GetString("ApellidoPaterno"),
                        Domicio = await reader.Result.IsDBNullAsync("Domicilio") ? "" : reader.Result.GetString("Domicilio"),
                        FechaNacimiento = reader.Result.GetDateTime("FechaNacimiento").Date,

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

                    listUsuario.Add(cliente);
                }
                Log.Debug("Se han obtenido con éxito la información de los clientes.");

                return listUsuario;
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
        public static List<Instructor> GetInstructores() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clase que se encarga de obtener todos los datos de los Usuarios
        /// </summary>
        /// <returns>Una lista de objetos tipo Usuario.</returns>
        public static async Task<List<Usuario>> GetUsuarios() {
            Log.Debug("se ha empezado el proceso de obtener la información de los usuarios.");

            await using var connection = new MySqlConnection(GetInitData.ConnString);
            await connection.OpenAsync();

            const string sqlQuery = "select * from usuario"; // TODO: hacer la query de verdad.

            await using var command = new MySqlCommand(sqlQuery, connection);
            using var reader = command.ExecuteReaderAsync();

            var listUsuario = new List<Usuario>();

            while (await reader.Result.ReadAsync()) {
                var usuario = new Usuario() {
                    Id = await reader.Result.IsDBNullAsync("IdUsuario") ? 0 : reader.Result.GetInt32("IdUsuario"),
                    Nombre = await reader.Result.IsDBNullAsync("Nombre") ? "" : reader.Result.GetString("Nombre"),
                    ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno") ? "" : reader.Result.GetString("ApellidoPaterno"),
                    ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno") ? "" : reader.Result.GetString("ApellidoMaterno"),
                    Username = await reader.Result.IsDBNullAsync("Username") ? "" : reader.Result.GetString("Username"),
                };
            }

            throw new NotImplementedException();
        }
    }
}