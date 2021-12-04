using System;
using System.Threading.Tasks;
using FluentValidation;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using GymCastillo.Model.Validations.Pagos;
using log4net;
using MySqlConnector;

namespace GymCastillo.Model.Admin {
    /// <summary>
    /// Clase que se encarga de aplicar el método only alta en las interfaces donde se ocupan
    /// </summary>
    public class AdminOnlyAlta {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de
        /// </summary>
        public static async Task Alta(AbstractMovimientos objeto) {
            Log.Debug("Se ha iniciado el proceso de alta de un objeto de solo alta.");

            try {
                // validamos
                var movimientoValidator = new PagosValidation();
                await movimientoValidator.ValidateAndThrowAsync(objeto);

                // Hacemos el alta.
                var res = await objeto.Alta();

                // Verificamos la query.
                if (res == 0) {
                    // No se han hecho cambios a la bd
                    ShowPrettyMessages.WarningOk("No se han hecho cambios a la base de datos", "Sin cambios");
                    Log.Warn("No se han hecho cambios a la base de datos.");
                }
                else {
                    ShowPrettyMessages.NiceMessageOk("Se ha actualizado la base de datos.", "Operación Exitosa");
                }
            }
            catch (ValidationException msg) {
                ShowPrettyMessages.WarningOk($"{msg.Message}", "Datos erróneos");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrió un error desconocido a la hora de hacer el proceso de Alta.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
            }
        }
    }
}