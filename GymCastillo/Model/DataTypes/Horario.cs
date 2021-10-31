using System;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que tiene los campos y métodos de los objetos tipo Horario.
    /// </summary>
    public class Horario {

        /// <summary>
        /// Hora de inicio de la clase.
        /// </summary>
        public TimeSpan HoraInicio { get; set; }

        /// <summary>
        /// Hora final de la clase.
        /// </summary>
        public TimeSpan HoraFin { get; set; }

        /// <summary>
        /// Id del espacio donde se hace la clase.
        /// </summary>
        public int IdEspacio { get; set; }

        /// <summary>
        /// El nombre del espacio donde se hace la clase.
        /// </summary>
        public string NombreEspacio { get; set; }

        /// <summary>
        /// Método que da el horario en un string
        /// </summary>
        /// <returns>Un string con el horario y el IdEspacio.</returns>
        public string GetHorarioStr() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checa si el objeto horario dado choca con el de la instancia actual.
        /// </summary>
        /// <param name="horario"></param>
        /// <returns><c>True</c> Si el horario esta disponible <c>False</c> si no.</returns>
        public bool CheckHorario(Horario horario) {
            throw new NotImplementedException();
        }
    }
}