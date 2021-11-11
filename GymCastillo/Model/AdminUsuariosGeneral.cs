using System;
using FluentValidation;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Validations;
using log4net;

namespace GymCastillo.Model {
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
        public static void Update(AbstUsuario objeto) {
            try {
                // Valida el objeto.
                var validator = new UsuarioValidation();
                validator.ValidateAndThrowAsync(objeto);

                // Hacemos Update.
                var res = objeto.Update().Result;

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
        public static void Delete(AbstUsuario objeto) {
            try {
                // validamos el objeto
                var validator = new UsuarioValidation();
                validator.ValidateAndThrowAsync(objeto);

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
        public static void Alta(AbstUsuario objeto) {
            try {
                // validamos el objeto
                var validator = new UsuarioValidation();
                validator.ValidateAndThrowAsync(objeto);

                // Hacemos el delete.
                var res = objeto.Alta().Result;

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
                Log.Error("Ha ocurrio un error desconocido a la hora de hacer el proceso de Alta.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido, Error: {e.Message}",
                    "Error desconocido");
            }
        }
    }
}