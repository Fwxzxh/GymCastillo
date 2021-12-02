using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Init;
using log4net;

namespace GymCastillo.Model.Helpers {
    /// <summary>
    /// Clase que contiene métodos para ayudar en la conversión de los horarios en el front a los horarios del back.
    /// </summary>
    public static class HorarioHelper {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que obtiene todos los horarios de un id de clase Dado.
        /// </summary>
        /// <param name="idClase">El id de clase a la cual pertenecen los horarios.</param>
        /// <returns>Un objeto tipo <c>FrontHorario</c>.</returns>
        public static FrontHorario GetHorariosFront(int idClase) {
            Log.Debug("Se ha iniciado el proceso de obtener los horarios para el front.");
            try {

                var horariosFiltrados = GetHorarios(idClase);

                var horarioFront =  ClassifyHorario(horariosFiltrados);
                horarioFront.IdClase = idClase;

                Log.Debug("Se han obtenido los horarios para el front con éxito.");
                return horarioFront;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de obtener los horarios par el front.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    "Ha ocurrido un error desconocido a la hora de obtener los horarios. " +
                    $"Error: {e.Message}",
                    "Error desconocido.");
                return null;
            }
        }

        /// <summary>
        /// Método que obtiene los horarios de memoria y los filtra por un id de clase.
        /// </summary>
        /// <param name="idClase">El id de la clase a la que deben pertenecer los horarios.</param>
        /// <returns>Una lista de objetos tipo horario</returns>
        private static List<Horario> GetHorarios(int idClase) {
            // Leemos la lista de horarios desde la memoria.
            var horarios = InitInfo.ListHorarios;

            // Filtramos los horarios que necesitamos.
            var horariosFiltrados = new List<Horario>();
            for (var index = 0; index < horarios.Count; index++) {
                var horario = horarios[index];

                if (horario.IdClase == idClase) {
                    horariosFiltrados.Add(horario);
                }
            }

            return horariosFiltrados;
        }

        /// <summary>
        /// Método que clasifica los horarios por día.
        /// </summary>
        /// <returns>Un objeto tipo <c>FrontHorario</c>.</returns>
        private static FrontHorario ClassifyHorario(List<Horario> horariosFiltrados) {
            Log.Debug("Se ha iniciado el proceso de clasificar los horarios");

            try {
                var horarioFront = new FrontHorario() {
                    HorasLunes = new List<Horario>(),
                    HorasMartes = new List<Horario>(),
                    HorasMiércoles = new List<Horario>(),
                    HorasJueves = new List<Horario>(),
                    HorasViernes = new List<Horario>(),
                    HorasSábado = new List<Horario>(),
                    HorasDomingo = new List<Horario>()
                };

                for (var index = 0; index < horariosFiltrados.Count; index++) {
                    var horario = horariosFiltrados[index];

                    switch (horario.Dia) {
                        case 1:
                            horarioFront.HorasLunes.Add(horario);
                            break;
                        case 2:
                            horarioFront.HorasMartes.Add(horario);
                            break;
                        case 3:
                            horarioFront.HorasMiércoles.Add(horario);
                            break;
                        case 4:
                            horarioFront.HorasJueves.Add(horario);
                            break;
                        case 5:
                            horarioFront.HorasViernes.Add(horario);
                            break;
                        case 6:
                            horarioFront.HorasSábado.Add(horario);
                            break;
                        case 7:
                            horarioFront.HorasDomingo.Add(horario);
                            break;
                        default:
                            throw new Exception("Datos Incongruentes");
                    }
                }

                Log.Debug("Se han clasificado los horarios exitosamente.");
                return horarioFront;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error fatal a la hora de clasificar los horarios, " +
                          "probables datos incongruentes, dia excedió y/o no corresponde.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    "Ha ocurrido un error desconocido al clasificar los horarios, contacte al administrador " +
                    "Ya que puede haber información corrupta en la base de datos.",
                    "Error fatal desconocido.");
                throw;
            }
        }

        /// <summary>
        /// Método que se encarga de guardar los datos del horario en la base de datos.
        /// </summary>
        /// <param name="frontHorario">El objeto tipo <c>FrontHorario</c> </param>
        public static void SaveHorarios(FrontHorario frontHorario) {
            Log.Debug("Se ha iniciado el proceso de guardar los datos.");

            try {
                var horariosFiltrados = GetHorarios(frontHorario.IdClase);

                var horariosNuevos = new List<Horario>();

                // juntamos todos los horarios en una sola lista.
                horariosNuevos.AddRange(frontHorario.HorasLunes);
                horariosNuevos.AddRange(frontHorario.HorasMartes);
                horariosNuevos.AddRange(frontHorario.HorasMiércoles);
                horariosNuevos.AddRange(frontHorario.HorasJueves);
                horariosNuevos.AddRange(frontHorario.HorasViernes);
                horariosNuevos.AddRange(frontHorario.HorasSábado);
                horariosNuevos.AddRange(frontHorario.HorasDomingo);

                foreach (var horario in horariosNuevos) {
                    if (horario.IdHorario == 0) { // Nuevo horario
                        // hacemos alta.
                        Task.Run(() => AdminOtrosTipos.Alta(horario));
                    }
                    else {
                        // update
                        Task.Run(() => AdminOtrosTipos.Update(horario));
                    }
                }

                // Verificamos que no se haya eliminado alguno.
                if (horariosFiltrados.Count != horariosNuevos.Count) {
                    // se elimino un horario

                }

            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}