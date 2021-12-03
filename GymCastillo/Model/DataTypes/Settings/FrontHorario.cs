using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Helpers;

namespace GymCastillo.Model.DataTypes.Settings {
    /// <summary>
    /// Clase que se encarga de guardar los campos del horario para el front.
    /// </summary>
    public class FrontHorario {

        /// <summary>
        /// El id de la clase a la que pertenecen los horarios.
        /// </summary>
        public int IdClase { get; set; }

        /// <summary>
        /// las Horas del lunes.
        /// </summary>
        public List<Horario> HorasLunes { get; set; }

        /// <summary>
        /// Las horas del Martes.
        /// </summary>
        public List<Horario> HorasMartes { get; set; }

        /// <summary>
        /// Las horas del Miércoles.
        /// </summary>
        public List<Horario> HorasMiércoles { get; set; }

        /// <summary>
        /// Las horas del Jueves.
        /// </summary>
        public List<Horario> HorasJueves { get; set; }

        /// <summary>
        /// Las horas del Viernes.
        /// </summary>
        public List<Horario> HorasViernes { get; set; }

        /// <summary>
        /// Las horas del Sábado
        /// </summary>
        public List<Horario> HorasSábado { get; set; }

        /// <summary>
        /// Las horas del Domingo.
        /// </summary>
        public List<Horario> HorasDomingo { get; set; }

        /// <summary>
        /// Constructor de la clase Front Horario
        /// </summary>
        public FrontHorario() {
            // Inicializamos las listas.
            HorasLunes = new List<Horario>();
            HorasMartes = new List<Horario>();
            HorasMiércoles = new List<Horario>();
            HorasJueves = new List<Horario>();
            HorasViernes = new List<Horario>();
            HorasSábado = new List<Horario>();
            HorasDomingo = new List<Horario>();
        }

        /// <summary>
        /// Método que agrega horarios al objeto actual.
        /// </summary>
        public void AddHorario(Horario horario, int dia) {
            horario.IdHorario = 0;
            switch (dia) {
                case 1:
                    HorasLunes.Add(horario);
                    break;
                case 2:
                    HorasMartes.Add(horario);
                    break;
                case 3:
                    HorasMiércoles.Add(horario);
                    break;
                case 4:
                    HorasJueves.Add(horario);
                    break;
                case 5:
                    HorasViernes.Add(horario);
                    break;
                case 6:
                    HorasSábado.Add(horario);
                    break;
                case 7:
                    HorasDomingo.Add(horario);
                    break;
                default:
                    ShowPrettyMessages.ErrorOk(
                        "ha ocurrido un error al agregar el horario, dia incongruente, contacte al administrador.",
                        "Error");
                        break;
            }
        }

        /// <summary>
        /// Método que borra un horario de la lista.
        /// </summary>
        public async void DeleteHorario(Horario horario, int dia) {
            if (horario.IdHorario == 0) {
                // Solo eliminamos de la lista, no esta en bd.
                switch (dia) {
                    case 1:
                        HorasLunes.Remove(horario);
                        break;
                    case 2:
                        HorasMartes.Remove(horario);
                        break;
                    case 3:
                        HorasMiércoles.Remove(horario);
                        break;
                    case 4:
                        HorasJueves.Remove(horario);
                        break;
                    case 5:
                        HorasViernes.Remove(horario);
                        break;
                    case 6:
                        HorasSábado.Remove(horario);
                        break;
                    case 7:
                        HorasDomingo.Remove(horario);
                        break;
                    default:
                        ShowPrettyMessages.ErrorOk(
                            "ha ocurrido un error al agregar el horario, dia incongruente, contacte al administrador.",
                            "Error");
                        break;
                }
            }
            else {
                // Esta dado de alta, eliminamos
                await AdminOtrosTipos.Delete(horario, true);
                // ponemos id horario en 0 para mandarlo recursivamente y eliminarlo de la lista.
                horario.IdHorario = 0;
                DeleteHorario(horario, horario.Dia);
            }
        }
    }
}