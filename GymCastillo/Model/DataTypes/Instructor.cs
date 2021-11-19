using System;
using System.Threading.Tasks;
using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que contiene los campos y métodos del objeto Instructor
    /// </summary>
    public class Instructor : AbstClientInstructor {

        /// <summary>
        /// La hora de entrada designada al instructor
        /// </summary>
        public DateTime HoraEntrada { get; set; }

        /// <summary>
        /// La hora de salida designada al instructor.
        /// </summary>
        public DateTime HoraSalida { get; set; }

        /// <summary>
        /// La cantidad de dias a trabajar del instructor.
        /// </summary>
        public int DiasATrabajar { get; set; }

        /// <summary>
        /// La cantidad actual de dias trabajados.
        /// </summary>
        public int DiasTrabajados { get; set; }

        /// <summary>
        /// El sueldo del Instructor.
        /// </summary>
        public decimal Sueldo { get; set; }

        /// <summary>
        /// La cantidad del sueldo a descontar por amonestaciones.
        /// </summary>
        public decimal SueldoDescontar { get; set; }

        /// <summary>
        /// El id del tipo de instructor.
        /// </summary>
        public int  TipoInstructor { get; set; }

        /// <summary>
        /// El nombre del tipo de instructor.
        /// </summary>
        public string NombreTipoInstructor { get; set; }

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

        public override Task<int> NuevaAsistencia() {
            throw new NotImplementedException();
        }

        public override void Pago(decimal cantidad) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que obtiene el horario del Instructor de la instancia actual en un string
        /// </summary>
        /// <returns>Un string con el horario del instructor.</returns>
        public override string GetHorarioStr() {
            throw new NotImplementedException();
        }
    }
}