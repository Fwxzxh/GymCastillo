using System;
using System.Collections.Generic;

namespace GymCastillo.Model.DataTypes {
    // Clase que contiene los campos y métodos de el objeto tipo Clase.
    public class Clase {

        /// <summary>
        /// Id de la clase.
        /// </summary>
        public int IdClase { get; set; }

        /// <summary>
        /// Id del instructor designado a la clase.
        /// </summary>
        public int IdInstructor { get; set; }

        /// <summary>
        /// Nombre completo del instructor.
        /// </summary>
        public string NombreInstructor { get; set; }

        /// <summary>
        /// Descripción de la clase.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Nombre de la Clase.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Costo por hora de la clase.
        /// </summary>
        public decimal CostoHora { get; set; }

        /// <summary>
        /// El horario de la clase.
        /// </summary>
        // TODO: Ver como va a quedar este pedo.
        public List<Dictionary<Dias, Horario>> Horario { get; set; }

        /// <summary>
        /// El hoario de la clase codificado en un string.
        /// </summary>
        public string HorarioStr { get; set; }


        /// <summary>
        /// Toma el campo de HorarioStr y lo convierte en el campo Horario.
        /// </summary>
        public void DecodeHorairioStr() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Toma el campo de Horario y lo guarda en HorarioStr listo para la base de datos.
        /// </summary>
        public void EncodeHorarioStr() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Actualiza la instancia actual de la clase en la base de datos.
        /// </summary>
        public void Update() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Elimina (Desactiva) la instancia actual de la clase en la base de datos.
        /// </summary>
        public void Delete() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Da de alta una nueva clase de la instancia actual de la clase en la base de datos.
        /// </summary>
        public void Alta() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que valida la instancia de la Clase.
        /// </summary>
        /// <returns><c>True</c> si la instancia es valida.</returns>
        public bool Validate() {
            throw new NotImplementedException();
        }
    }
}