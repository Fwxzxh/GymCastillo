using System.Collections.Generic;
using System.Windows.Documents;

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
    }
}