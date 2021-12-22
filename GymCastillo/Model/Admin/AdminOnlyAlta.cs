using System;
using System.Threading.Tasks;
using FluentValidation;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Interfaces;
using GymCastillo.Model.Validations.Pagos;
using log4net;

namespace GymCastillo.Model.Admin {
    /// <summary>
    /// Clase que se encarga de aplicar el método only alta en las interfaces donde se ocupan
    /// </summary>
    public static class AdminOnlyAlta {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de dar de alta una instancia del objeto dado en la base de datos.
        /// </summary>
        public static async Task Alta(IOnlyAlta objeto) {
            Log.Debug("Se ha iniciado el proceso de alta de un objeto de solo alta.");

            try {
                // validamos
                await ValidateAgain(objeto);

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

        /// <summary>
        /// Método que se encarga de averiguar el tipo de objeto que es y validar los campos especializados.
        /// </summary>
        /// <param name="objeto"></param>
        /// <exception cref="Exception"></exception>
        private static async Task ValidateAgain(IOnlyAlta objeto) {
            var tipo = objeto.GetType().Name;

            // TODO: check si esto funciona bien y no regresa el nombre de Ingresos.
            switch (tipo) {
                case "AbstractMovimientos":
                    var movimientoValidator = new PagosValidation();
                    await movimientoValidator.ValidateAndThrowAsync((AbstractMovimientos)objeto);
                    break;

                case "Rentas":
                    var rentasValidator = new RentasValidation();
                    await rentasValidator.ValidateAndThrowAsync((Rentas)objeto);
                    break;

                case "Alta":
                    break;

                default:
                    throw new Exception("Error: no se pudo identificar al objeto.");
            }
        }
    }
}