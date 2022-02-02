using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Horario> HorasLunes { get; set; }

        /// <summary>
        /// Las horas del Martes.
        /// </summary>
        public ObservableCollection<Horario> HorasMartes { get; set; }

        /// <summary>
        /// Las horas del Miércoles.
        /// </summary>
        public ObservableCollection<Horario> HorasMiércoles { get; set; }

        /// <summary>
        /// Las horas del Jueves.
        /// </summary>
        public ObservableCollection<Horario> HorasJueves { get; set; }

        /// <summary>
        /// Las horas del Viernes.
        /// </summary>
        public ObservableCollection<Horario> HorasViernes { get; set; }

        /// <summary>
        /// Las horas del Sábado
        /// </summary>
        public ObservableCollection<Horario> HorasSábado { get; set; }

        /// <summary>
        /// Las horas del Domingo.
        /// </summary>
        public ObservableCollection<Horario> HorasDomingo { get; set; }

        /// <summary>
        /// Constructor de la clase Front Horarios
        /// </summary>
        public FrontHorario() {
            // Inicializamos las listas.
            HorasLunes = new ObservableCollection<Horario>();
            HorasMartes = new ObservableCollection<Horario>();
            HorasMiércoles = new ObservableCollection<Horario>();
            HorasJueves = new ObservableCollection<Horario>();
            HorasViernes = new ObservableCollection<Horario>();
            HorasSábado = new ObservableCollection<Horario>();
            HorasDomingo = new ObservableCollection<Horario>();
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