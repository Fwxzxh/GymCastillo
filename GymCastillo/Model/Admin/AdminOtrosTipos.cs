using System;
using System.Threading.Tasks;
using FluentValidation;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using log4net;

namespace GymCastillo.Model.Admin {
    /// <summary>
    /// CLase que se encarga de exponer todas las operaciones comunes entre objetos tipo <c>AbstOtrosTipos</c>.
    /// </summary>
    public class AdminOtrosTipos {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);


        /// <summary>
        /// Método que se encarga de actualizar y validar los datos del objeto dado tipo <c>AbstOtrosTipos</c>
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="silent">Si se le da true, no va a mostrar mensajes de confirmación.</param>
        public static async Task Update(AbstOtrosTipos objeto, bool silent=false) {
            try {
                // validamos
                // TODO: hacer validaciones.

                // Hacemos update
                var res = await objeto.Update();

                // Verificamos los cambios.
                if (res == 0) {
                    // No se han hecho cambios a la bd.
                    ShowPrettyMessages.WarningOk("No se han hecho cambios a la base de datos", "Sin cambios");
                }
                else {
                    if (!silent) {
                        ShowPrettyMessages.NiceMessageOk("Se ha actualizado la base de datos.", "Operación Exitosa");
                    }
                }
            }
            catch (ValidationException msg) {
                ShowPrettyMessages.WarningOk($"{msg.Message}", "Datos erróneos");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrió un error desconocido a la hora de hacer el Update.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
            }
        }

        /// <summary>
        /// Método que se encarga de borrar los datos del objeto dado tipo <c>AbstOtrosTipos</c> en la base de datos.
        /// </summary>
        /// <param name="objeto">El objeto a borrar.</param>
        /// <param name="silent">Si se le da true, no va a mostrar mensajes de confirmación.</param>
        public static async Task Delete(AbstOtrosTipos objeto, bool silent=false) {
            try {
                // No es necesario validar el objeto.

                // Hacemos el delete.
                var res = await objeto.Delete();

                // Verificamos los cambios.
                if (res == 0) {
                    // No se han hecho cambios a la bd
                    ShowPrettyMessages.WarningOk("No se han hecho cambios a la base de datos", "Sin cambios");
                }
                else {
                    if (!silent) {
                        ShowPrettyMessages.NiceMessageOk("Se ha actualizado la base de datos.", "Operación Exitosa");
                    }
                }
            }
            catch (ValidationException msg) {
                ShowPrettyMessages.WarningOk($"{msg.Message}", "Datos erróneos");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrió un error desconocido a la hora de hacer el proceso de borrado.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
            }
        }

        /// <summary>
        /// Método que se encarga de dar de alta y validar los datos del objeto dado tipo <c>AbstOtrosTipos</c>
        /// en la base de datos.
        /// </summary>
        /// <param name="objeto">El objeto a validar</param>
        /// <param name="silent">Si se le da true, no va a mostrar mensajes de confirmación.</param>
        public static async Task Alta(AbstOtrosTipos objeto, bool silent=false) {
            try {
                Log.Debug("Se ha iniciado un proceso de alta genérico.");

                // validamos los campos concretos
                // TODO: hacer validaciones.
                //await ValidateAgain(objeto);

                // Hacemos la alta.
                var res = await objeto.Alta();

                // Verificamos los cambios.
                if (res == 0) {
                    // No se han hecho cambios a la bd
                    ShowPrettyMessages.WarningOk("No se han hecho cambios a la base de datos", "Sin cambios");
                    Log.Warn("No se han hecho cambios a la base de datos.");
                }
                else {
                    if (!silent) {
                        ShowPrettyMessages.NiceMessageOk("Se ha actualizado la base de datos.", "Operación Exitosa");
                    }
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