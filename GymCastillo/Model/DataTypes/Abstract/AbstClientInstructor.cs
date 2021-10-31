using System;
using System.Collections.Generic;

namespace GymCastillo.Model.DataTypes.Abstract {
    /// <summary>
    /// Clase abtracta que contiene los campos y métodos comúnes entre clientes e instructores.
    /// </summary>
    public abstract class AbstClientInstructor : AbstUsuario {
        
        /// <summary>
        /// Lista que contiene los Id de las clases a las cuales este Cliente esta inscrito
        /// </summary>
        public List<int> IdClases { get; set; }

        /// <summary>
        /// Lista que contiene todos los objetos tipo clase del Cliente.
        /// </summary>
        public List<Clase> Clases { get; set; }

        /// <summary>
        /// Método que obtiene el horario del cliente o Instructor.
        /// </summary>
        /// <returns>El horario en string</returns>
        public abstract string GetHorarioStr();

        /// <summary>
        /// Método que da de alta una clase al cliente o Instructor.
        /// </summary>
        /// <param name="clase">Clase a dar de Alta</param>
        public abstract void AltaClase(Clase clase);

        /// <summary>
        /// Método que da de baja una clase al cliente o al instructor.
        /// </summary>
        /// <param name="clase">Clase a dar de baja.</param>
        public abstract void BajaClase(Clase clase);

        /// <summary>
        /// Método que da de alta una nueva asitencia al cliente o instructor.
        /// </summary>
        /// <param name="fecha">Fecha a la cual poner la asistencia.</param>
        public abstract void NuevaAsistencia(DateTime fecha);
    }
}