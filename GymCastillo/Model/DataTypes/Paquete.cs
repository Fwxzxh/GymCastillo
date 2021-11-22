using System;
using System.Windows.Documents;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que contiene los campos y métodos del objeto tipo Paquete.
    /// </summary>
    public class Paquete {

        /// <summary>
        /// Id del paquete.
        /// </summary>
        public int IdPaquete { get; set; }

        /// <summary>
        /// Indica si el paquete actual incluye gym.
        /// </summary>
        public bool Gym { get; set; }

        /// <summary>
        /// Indica el nombre del paquete.
        /// </summary>
        public string NombrePaquete { get; set; }

        /// <summary>
        /// Indica el número de clases que incluye el paquete.
        /// </summary>
        public int NumClasesTotales { get; set; }

        /// <summary>
        /// Indica el número de clases que puede tomar el usuario en la semana.
        /// </summary>
        public int NumClasesSemanales { get; set; }

        /// <summary>
        /// Indica el costo mensual del paquete.
        /// </summary>
        public decimal Costo { get; set; }

        /// <summary>
        /// Contiene el id la clases que incluye el paquete.
        /// </summary>
        public int IdClase { get; set; }

        /// <summary>
        /// Contiene el nombre de las clase que incluye el paquete.
        /// </summary>
        public string NombreClase { get; set; }

        /// <summary>
        /// Método que se encarga de hacer update a la clase actual.
        /// </summary>
        public void Update() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de alta en la base de datos esta instancia de clase.
        /// </summary>
        public void Alta() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que se encarga de desactivar la instancia de la clase en la base de datos.
        /// </summary>
        public void Delete() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que se encarga de validar los datos de la instancia de la clase.
        /// </summary>
        public void Validate() {
            throw new NotImplementedException();
        }
    }
}