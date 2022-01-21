using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.DataTypes.Ventas {
    /// <summary>
    /// Clase que contiene los métodos y campos del tipo de dato Inventario.
    /// </summary>
    public class Inventario : AbstOtrosTipos {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// El id del producto.
        /// </summary>
        public int IdProducto { get; set; }

        /// <summary>
        /// El nombre del producto
        /// </summary>
        public string NombreProducto { get; set; }

        /// <summary>
        /// La descripción del producto.
        /// </summary>
        public string Descripción { get; set; }

        /// <summary>
        /// El costo del producto.
        /// </summary>
        public decimal Costo { get; set; }

        /// <summary>
        /// Las existencias actuales del producto.
        /// </summary>
        public int Existencias { get; set; }

        public override async Task<int> Update() {
            Log.Debug("Se ha empezado el proceso de update de un item del inventario.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string updateQuery = @"UPDATE inventario
                                             SET Descripcion=@Descripcion, Costo=@Costo,
                                                 Existencias=@Existencias
                                             WHERE IdProducto=@IdProducto;";

                await using var command = new MySqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@Descripcion", Descripción);
                command.Parameters.AddWithValue("@Costo", Costo.ToString(CultureInfo.InvariantCulture));

                command.Parameters.AddWithValue("@Existencias", Existencias.ToString());

                command.Parameters.AddWithValue("@IdProducto", IdProducto.ToString());

                Log.Debug("Se ha creado la query.");

                var res =await ExecSql.NonQuery(command, "Update Producto");
                Log.Debug("Se ha editado un producto.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de actualizar el producto.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Delete() {
            Log.Debug("Se ha iniciado el proceso de delete de un producto.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string deleteQuery = @"delete from inventario where IdProducto=@IdProducto";

                await using var command = new MySqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@IdProducto", IdProducto.ToString());

                Log.Debug("Se ha creado la query.");

                var res = await ExecSql.NonQuery(command, "Delete Producto");
                Log.Debug("Se ha eliminado un producto de la tabla.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el delete del producto.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        public override async Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta un producto.");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"INSERT INTO inventario
                                           VALUES 
                                               (default, @NombreProducto,
                                               @Descripcion, @Costo, @Existencias);";

                await using var command = new MySqlCommand(altaQuery, connection);

                command.Parameters.AddWithValue("@NombreProducto", NombreProducto);
                command.Parameters.AddWithValue("@Descripcion", Descripción);

                command.Parameters.AddWithValue("@Costo",
                    Costo.ToString(CultureInfo.InvariantCulture));
                command.Parameters.AddWithValue("@Existencias", Existencias.ToString());

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Alta producto");
                Log.Debug("Se ha dado de alta un producto.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de dar de alta un producto.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }

        /// <summary>
        /// Método que se encarga de actualizar la existencia del producto.
        /// </summary>
        /// <param name="cantidad">La cantidad a sumar/restar de la existencia.</param>
        /// <param name="suma">Si se le manda true, suma la cantidad a la existencia</param>
        /// <returns>La cantidad de columnas afectadas.</returns>
        public async Task<int> UpdateExistencias(int cantidad, bool suma=false) {
            Log.Debug("Se ha iniciado el proceso de dar de actualizar las existencias de un producto");

            try {
                await using var connection = new MySqlConnection(GetInitData.ConnString);
                await connection.OpenAsync();
                Log.Debug("Se ha creado la conexión.");

                const string altaQuery = @"update inventario
                                           set Existencias=@Existencias
                                           where IdProducto=@IdProducto;";

                await using var command = new MySqlCommand(altaQuery, connection);

                var existenciasActualizadas = suma
                    ? Existencias + cantidad
                    : Existencias - cantidad;

                command.Parameters.AddWithValue("@Existencias", existenciasActualizadas.ToString());
                command.Parameters.AddWithValue("@IdProducto", IdProducto.ToString());

                Log.Debug("Se ha generado la query.");

                var res = await ExecSql.NonQuery(command, "Update existencias Inventarios");
                Log.Debug("Se han actualizado las existencias de un producto.");

                return res;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de actualizar las existencias de un producto.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
                return 0;
            }
        }
    }
}