using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que contiene los campos y métodos de la clase pagos.
    /// </summary>
    public class Pagos : AbstractMovimientos {

        /// <summary>
        /// Indica si el movimiento es por un servicio.
        /// </summary>
        public bool Servicios { get; set; }

        /// <summary>
        /// Indica si el movimiento es por una paga de nómina.
        /// </summary>
        public bool Nomina { get; set; }

        /// <summary>
        /// Id del usuario al cual se le paga si es una nómina.
        /// </summary>
        public int IdUsuarioPagar { get; set; }

        /// <summary>
        /// Id del instructor
        /// </summary>
        public int IdInstructor { get; set; }

        /// <summary>
        /// Método que da de alta un Pago.
        /// </summary>
        public override void Alta() {
            throw new System.NotImplementedException();
        }
    }
}