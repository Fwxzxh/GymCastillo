﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.Settings {
    // Clase que contiene los campos y métodos de el objeto tipo Clase.
    public class Clase : AbstOtrosTipos {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Id de la clase.
        /// </summary>
        public int IdClase { get; set; }

        /// <summary>
        /// Nombre de la Clase.
        /// </summary>
        public string NombreClase { get; set; }

        /// <summary>
        /// Descripción de la clase.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Ids de los instructores asignados a la clase.
        /// </summary>
        public int IdInstructor { get; set; }

        /// <summary>
        /// Nombres completos de los instructores asignados a la clase.
        /// </summary>
        public string NombreInstructor { get; set; }

        /// <summary>
        /// Cupo máximo de la clase.
        /// </summary>
        public int CupoMaximo { get; set; }

        /// <summary>
        /// El id del espacio donde va a ocurrir la clase.
        /// </summary>
        public int IdEspacio { get; set; }

        /// <summary>
        /// El nombre del espacio donde va a ocurrir la clase.
        /// </summary>
        public string NombreEspacio { get; set; }

        /// <summary>
        /// Indica si la clase esta activa o no.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// los ids de los paquetes en los que se encuentra esta clase.
        /// </summary>
        public string IdsPaquetes { get; set; }

        /// <summary>
        /// Los nombres de los paquetes en los que se encuentra esta clase.
        /// </summary>
        public string NombresPaquetes { get; set; }

        public override async Task<int> Update() {
            Log.Debug("Se ha iniciado el proceso de update de un objeto tipo Clase.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string updateQuery = @"UPDATE clase
                                             SET NombreClase=@NombreClase, 
                                                 CupoMaximo=@CupoMaximo, Activo=@Activo,
                                                 IdEspacio=@IdEspacio,
                                                 Descripcion=@Descripcion
                                             WHERE IdClase=@IdClase;";

                await using var command = new MySqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@NombreClase", NombreClase);
                command.Parameters.AddWithValue("@IdClase", IdClase.ToString());
                command.Parameters.AddWithValue("@CupoMaximo", CupoMaximo.ToString());
                command.Parameters.AddWithValue("@Activo", Convert.ToInt32(Activo).ToString());

                command.Parameters.AddWithValue("@IdEspacio", IdEspacio.ToString());

                command.Parameters.AddWithValue("@Descripcion", Descripcion);

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Update Clase");
                Log.Debug("Se ha actualizado un Instructor.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el update de la clase.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que checa si se puede borrar este objeto en la base de datos.
        /// </summary>
        /// <returns>False si falla una validación.</returns>
        private bool CheckDeleteConstrains() {
            if (InitInfo.ObCoHorarios.Any(x => x.IdClase == IdClase)) {
                ShowPrettyMessages.InfoOk(
                    "No se puede eliminar esta clase ya que esta asignada a un horario, " +
                    "intente eliminar los horarios primero.",
                    "Clase asignada a un horario.");
                return false;
            }

            if (InitInfo.ObCoClaseInstructores.Any(x => x.IdClase == IdClase)) {
                ShowPrettyMessages.InfoOk(
                    "No se puede eliminar esta clase ya que esta asignada a un instructor, " +
                    "intente cambiar esa clase primero.",
                    "Clase asignada a un instructor.");
                return false;
            }

            return true;
        }

        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de delete en una clase.");

            if (!Activo) {
                ShowPrettyMessages.InfoOk(
                    "No se permiten eliminar clases inactivas esto porque es probable que se vuelvan a ocupar.",
                    "Clase ya inactiva");
                return 0;
            }

            // Checamos si podemos eliminar
            if (!CheckDeleteConstrains()) {
                return 0;
            }


            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"update Clase set Activo=false where IdClase=@IdClase";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdClase", IdClase.ToString());

                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Delete Clase");
                Log.Debug("Se ha eliminado un una clase de la tabla.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete de la clase.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de update de un objeto tipo Clase.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"INSERT INTO clase
                                           VALUES
                                               (default, @NombreClase, @Descripcion,
                                               @CupoMaximo, @Activo, @IdEspacio);";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@NombreClase", NombreClase);
                command.Parameters.AddWithValue("@Descripcion", Descripcion);

                command.Parameters.AddWithValue("@CupoMaximo", CupoMaximo.ToString());
                command.Parameters.AddWithValue("@Activo", Convert.ToInt32(Activo).ToString());

                command.Parameters.AddWithValue("@IdEspacio", IdEspacio.ToString());

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Alta Clase");
                Log.Debug("Se ha dado de alta una clase.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta un cliente.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}