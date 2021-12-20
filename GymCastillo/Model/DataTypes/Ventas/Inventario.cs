
using System.Threading.Tasks;
using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model.DataTypes.Ventas {
    /// <summary>
    /// Clase que contiene los métodos y campos del tipo de dato Inventario.
    /// </summary>
    public class Inventario : AbstOtrosTipos{

        /// <summary>
        /// El id del producto.
        /// </summary>
        public int IdProducto { get; set; }

        /// <summary>
        /// El nombre del producto
        /// </summary>
        public string NombreProducto { get; set; }

        /// <summary>
        /// La descripción del producto.
        /// </summary>
        public string Descripción { get; set; }

        /// <summary>
        /// El costo del producto.
        /// </summary>
        public decimal Costo { get; set; }

        /// <summary>
        /// Las existencias actuales del producto.
        /// </summary>
        public int Existencias { get; set; }

        public override Task<int> Update() {
            throw new System.NotImplementedException();
        }

        public override Task<int> Delete() {
            throw new System.NotImplementedException();
        }

        public override Task<int> Alta() {
            throw new System.NotImplementedException();
        }
    }
}