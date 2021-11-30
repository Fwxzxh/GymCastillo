using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que contiene todos los campos y métodos de la clase Ingresos.
    /// </summary>
    public class Ingresos : AbstractMovimientos {

        /// <summary>
        /// El id de la renta si el ingreso es por una renta.
        /// </summary>
        public int IdRenta { get; set; }

        /// <summary>
        /// El id de la venta si el ingreso es por una venta
        /// </summary>
        public int IdVenta { get; set; }

        /// <summary>
        /// El id del cliente si el ingreso es por un cliente.
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Da de alta un Ingreso.
        /// </summary>
        public override void Alta() {
            throw new System.NotImplementedException();
        }
    }
}