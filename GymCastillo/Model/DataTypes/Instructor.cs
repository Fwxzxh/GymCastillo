using System;
using System.Threading.Tasks;
using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que contiene los campos y métodos del objeto Instructor
    /// </summary>
    public class Instructor : AbstClientInstructor {

        /// <summary>
        /// Si el Instructor tiene alguna condición especial.
        /// </summary>
        // TODO: preguntar si esto también aplica a instructores.
        public bool CondicionEspecial { get; set; }

        /// <summary>
        /// El pago por hora del instructor.
        /// </summary>
        /// TODO: Mover este campo al campo de clase ya que cada instructor puede tener más de una clase y cobrar diferente por cada una.
        public decimal Pagohora { get; set; }

        /// <summary>
        /// Las asistencias del instructor.
        /// </summary>
        // TODO: preguntar como se manejan las asistencias para instructores, al entrar al gym o x clase.
        // Igual comvendria guardar las asistenacias en la tabla de clases xd.
        public string Asistencias { get; set; }

        /// <summary>
        /// Método que actualiza la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas.</returns>
        public override Task<int> Update() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que borra la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas.</returns>
        public override Task<int> Delete() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de alta la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <returns>La cantidad de columanas afectadas.</returns>
        public override Task<int> Alta() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que obtiene el horario del Instructor de la instancia actual en un string
        /// </summary>
        /// <returns>Un string con el horario del instructor.</returns>
        public override string GetHorarioStr() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de alta una clase a la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <param name="clase">La clase a dar de alta.</param>
        public override Task<int> AltaClase(Clase clase) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de baja una clase a la instancia actual del instructor en la base de datos.
        /// </summary>
        /// <param name="clase"></param>
        public override Task<int> BajaClase(Clase clase) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que registra una nueva asitencia al instructor.
        /// </summary>
        /// <param name="fecha">La fecha a dar de alta.</param>
        public override Task<int> NuevaAsistencia(DateTime fecha) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que le paga a un Instructor de acuerdo a la clase.
        /// </summary>
        public void Pagar() {
            throw new NotImplementedException();
        }
    }
}