using System;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que tiene los campos y métodos de los objetos tipo Horario.
    /// </summary>
    public class Horario {

        /// <summary>
        /// Id del horario
        /// </summary>
        public int IdHorario { get; set; }

        /// <summary>
        /// Id de la clase a la que pertenece.
        /// </summary>
        public int IdClase { get; set; }

        /// <summary>
        /// Dia en el que ocurre la clase.
        /// </summary>
        public Dias Dia { get; set; }

        /// <summary>
        /// Hora de inicio de la clase.
        /// </summary>
        public TimeSpan HoraInicio { get; set; }

        /// <summary>
        /// Hora final de la clase.
        /// </summary>
        public TimeSpan HoraFin { get; set; }

        /// <summary>
        /// El Cupo actual de la clase.
        /// </summary>
        public int CupoActual { get; set; }

        /// <summary>
        /// Método que actualiza la cantidad del cupo actual.
        /// </summary>
        public void NuevaAsistencia() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que resetea el cupo actual del horario actual.
        /// </summary>
        public void ResetAsistencia() {
            throw new NotImplementedException();
        }
    }
}