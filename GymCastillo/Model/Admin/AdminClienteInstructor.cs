﻿using System;
using System.Threading.Tasks;
using GymCastillo.Model.DataTypes.Abstract;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.Helpers;
using log4net;

namespace GymCastillo.Model.Admin {
    /// <summary>
    /// Clase que se encarga de exponer todos las operaciones communes para objetos tipo <c>AbstClientInstructor</c>.
    /// </summary>
    public static class AdminClienteInstructor {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de dar de alta una asistencia al objeto tipo AbstClientInstructor en la base de datos.
        /// </summary>
        /// <param name="asistencia">Objeto con la información necesaria para la asistencia.</param>
        public static async Task NuevaAsistencia(Asistencia asistencia) {
            Log.Debug("Se ha iniciado el proceso de dar de alta una nueva asistencia genérica.");
            try {
                switch (asistencia.Tipo) {
                    case 1: // Cliente

                        // Damos de alta la asistencia
                        var asistenciaTask = asistencia.DatosCliente.NuevaAsistencia(asistencia.NúmeroClasesAEntrar);
                        var resAsistenciaTask = await asistenciaTask;

                        // Verificamos que se hayan dado de alta los cambios en la base de datos.
                        if (resAsistenciaTask == 0) {
                            ShowPrettyMessages.WarningOk(
                                "No se ha actualizado la base de datos a la hora de actualizar los datos de entrada del cliente.",
                                "Sin cambios.");
                        }

                        Log.Debug("Se ha terminado el proceso de dar de alta la asistencia genérica de cliente.");
                        break;

                    case 2: // Instructor

                        // Damos de alta la asistencia
                        var taskAsistencia = asistencia.DatosInstructor.NuevaAsistencia(asistencia.SueldoADescontar);
                        var res = await taskAsistencia;

                        // Verificamos que se hayan dado de alt los campos.
                        if (res == 0) {
                            ShowPrettyMessages.WarningOk(
                                "No se ha actualizado la base de datos a la hora de actualizar los datos de entrada del Instructor.",
                                "Sin cambios.");
                        }
                        break;

                    default:
                        // No se encontró el tipo de la asistencia Error!!!
                        throw new EntryPointNotFoundException("No se encontró el tipo de asistencia.");

                }
            }
            catch (EntryPointNotFoundException e) {
                Log.Error("No se ha podido identificar el tipo de la asistencia.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"No se ha podido identificar el tipo de la asistencia, contacte a un administrador. Error: {e.Message}",
                    "Error Desconocido");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error el el proceso de registrar la asistencia.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido en el proceso de registrar la asistencia. {e.Message}",
                    "Error Desconocido");
            }
        }

        /// <summary>
        /// Método que se encarga de actualizar los campos de deuda y sueldo Por pagar del objeto AbsClientInstructor.
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="cantidad"></param>
        public static void Pago(AbstClientInstructor objeto, decimal cantidad) {
            // Validamos si hay deuda.
            // Si hay deuda descontamos la cantidad al objeto dado.
            // Validamos
            throw new NotImplementedException();
        }
    }
}