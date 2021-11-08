﻿using System;
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

            const string sqlQuery = "select * from cliente"; // TODO: hacer la query de verdad.

            var listUsuario = new List<Cliente>();

            try {
                await using var command = new MySqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReaderAsync();

                while (await reader.Result.ReadAsync()) {
                    var cliente = new Cliente() {
                        Id = await reader.Result.IsDBNullAsync("IdCliente") ? 0 : reader.Result.GetInt32("IdCliente"),
                        Nombre = await reader.Result.IsDBNullAsync("Nombre") ? "" : reader.Result.GetString("Nombre"),
                        ApellidoPaterno = await reader.Result.IsDBNullAsync("ApellidoPaterno") ? "" : reader.Result.GetString("ApellidoPaterno"),
                        ApellidoMaterno = await reader.Result.IsDBNullAsync("ApellidoMaterno") ? "" : reader.Result.GetString("ApellidoMaterno"),
                        FechaNacimiento = reader.Result.GetDateTime("FechaNacimiento"),
                        Telefono = await reader.Result.IsDBNullAsync("Telefono") ? "" : reader.Result.GetString("Telefono"),
                        NombreContacto = await reader.Result.IsDBNullAsync("NombreContacto") ? "" : reader.Result.GetString("NombreContacto"),
                        TelefonoContacto = await reader.Result.IsDBNullAsync("TelefonoContacto") ? "" : reader.Result.GetString("TelefonoContacto"),
                        //Foto = await reader.Result.IsDBNullAsync("Foto") ? null : reader.Result.GetBytes("Foto"), TODO: Ver como obtener la foto.
                        FechaUltimoAcceso = reader.Result.GetDateTime("FechaUltimoAcceso"),
                        //IdClases = await reader.Result.IsDBNullAsync("MontoUltimoPago") ? 0 : reader.Result.GetDecimal("MontoUltimoPago"),
                        // TODO agregar campo de nombre de clsaes List<Clase>
                        //Clases = await reader.Result.IsDBNullAsync("MontoUltimoPago") ? 0 : reader.Result.GetDecimal("MontoUltimoPago"),
                        CondicionEspecial = !await reader.Result.IsDBNullAsync("CondicionEspecial") && reader.Result.GetBoolean("CondicionEspecial"),
                        IdTipoCliente = await reader.Result.IsDBNullAsync("IdTipoCliente") ? 0 : reader.Result.GetInt32("IdTipoClient"),
                        //NombreTipoCliente = await reader.Result.IsDBNullAsync("IdTipoCliente") ? "" : reader.Result.GetString("Nombre"),
                        DeudaCliente = await reader.Result.IsDBNullAsync("DeudaCliente") ? 0 : reader.Result.GetDecimal("DeudaCliente"),
                        Asistencias = await reader.Result.IsDBNullAsync("Asistencias") ? "" : reader.Result.GetString("Asistencias"),
                        MontoUltimoPago = await reader.Result.IsDBNullAsync("MontoUltimoPago") ? 0 : reader.Result.GetDecimal("MontoUltimoPago"),
                        //FechaUltimoPago = Convert.ToDateTime(await reader.Result.IsDBNullAsync("FechaUltimoPago") ? "" : reader.Result.GetString("FechaUltimoPago")),
                        Activo = !await reader.Result.IsDBNullAsync("Activo") && reader.Result.GetBoolean("Activo"),
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