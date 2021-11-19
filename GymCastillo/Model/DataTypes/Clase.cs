using System;
using System.Collections.Generic;
using log4net;

namespace GymCastillo.Model.DataTypes {
    // Clase que contiene los campos y métodos de el objeto tipo Clase.
    public class Clase {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Id de la clase.
        /// </summary>
        public int IdClase { get; set; }

        /// <summary>
        /// Nombre de la Clase.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción de la clase.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Id del instructor designado a la clase.
        /// </summary>
        public int IdInstructor { get; set; }

        /// <summary>
        /// Nombre completo del instructor.
        /// </summary>
        public string NombreInstructor { get; set; }

        /// <summary>
        /// Cupo máximo de la clase.
        /// </summary>
        public int CupoMaximo { get; set; }

        /// <summary>
        /// El id del espacio donde va a ocurrir la clase.
        /// </summary>
        public int IdEspacio { get; set; }

        /// <summary>
        /// El nombre del espacio donde va a ocurrir la clase.
        /// </summary>
        public string NombreEspacio { get; set; }

        /// <summary>
        /// Indica si la clase esta activa o no.
        /// </summary>
        public bool Activo { get; set; }

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
        public void Validate() {
            throw new NotImplementedException();
        }
    }
}