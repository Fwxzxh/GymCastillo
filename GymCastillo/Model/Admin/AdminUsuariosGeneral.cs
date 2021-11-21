using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentValidation;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Validations;
using log4net;

namespace GymCastillo.Model.Admin {
    /// <summary>
    /// Clase que se encarga de exponer todas las operaciones comunes entre objetos tipo <c>AbstUsuario</c>.
    /// </summary>
    public static class AdminUsuariosGeneral {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de actualizar y validar los datos del objeto dado tipo AbstUsuario.
        /// en la base de datos.
        /// </summary>
        /// <param name="objeto">El objeto a modificar.</param>
        public static async Task Update(AbstUsuario objeto) {
            try {
                // Valida el objeto.
                var validator = new UsuarioValidation();
                await validator.ValidateAndThrowAsync(objeto);
                var validatorCliente = new ClienteValidations();
                await validatorCliente.ValidateAndThrowAsync((Cliente) objeto);

                // Hacemos Update.
                var res = objeto.Update().Result;

                // Verificamos los cambios.
                if (res == 0) {
                    // No se han hecho cambios a la bd.
                    ShowPrettyMessages.WarningOk("No se han hecho cambios a la base de datos", "Sin cambios");
                }
            }
            catch (ValidationException msg) {
                ShowPrettyMessages.WarningOk($"{msg.Message}", "Datos erroneos");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrio un error desconocido a la hora de hacer el Update.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
            }
        }

        /// <summary>
        /// Método que se encarga de borrar los datos del objeto dado tipo AbstUsuario en la base de datos.
        /// </summary>
        /// <param name="objeto">El objeto a borrar</param>
        public static async Task Delete(AbstUsuario objeto) {
            try {
                // validamos el objeto
                var validator = new UsuarioValidation();
                await validator.ValidateAndThrowAsync(objeto);
                var validatorCliente = new ClienteValidations();
                await validatorCliente.ValidateAndThrowAsync((Cliente) objeto);

                // Hacemos el delete.
                var res = objeto.Delete().Result;

                // Verificamos los cambios.
                if (res == 0) {
                    // No se han hecho cambios a la bd
                    ShowPrettyMessages.WarningOk("No se han hecho cambios a la base de datos", "Sin cambios");
                }
            }
            catch (ValidationException msg) {
                ShowPrettyMessages.WarningOk($"{msg.Message}", "Datos erroneos");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrio un error desconocido a la hora de hacer el proceso de borrado.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
            }
        }

        /// <summary>
        /// Método que se encarga de dar de alta y validar los datos del objeto dado tipo AbstUsuario
        /// en la base de datos.
        /// </summary>
        /// <param name="objeto">El objeto a dar de alta.</param>
        public static async Task Alta(AbstUsuario objeto) {
            try {
                Log.Debug("Se ha iniciado un proceso de alta generico.");
                // validamos el objeto
                var validator = new UsuarioValidation();
                await validator.ValidateAndThrowAsync(objeto);
                var validatorCliente = new ClienteValidations();
                await validatorCliente.ValidateAndThrowAsync((Cliente) objeto);

                // Hacemos la alta.
                var res = await objeto.Alta();

                // Verificamos los cambios.
                if (res == 0) {
                    // No se han hecho cambios a la bd
                    ShowPrettyMessages.WarningOk("No se han hecho cambios a la base de datos", "Sin cambios");
                    Log.Warn("No se han hecho cambios a la base de datos.");
                }
            }
            catch (ValidationException msg) {
                ShowPrettyMessages.WarningOk($"{msg.Message}", "Datos erroneos");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrio un error desconocido a la hora de hacer el proceso de Alta.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
            }
        }
    }
}