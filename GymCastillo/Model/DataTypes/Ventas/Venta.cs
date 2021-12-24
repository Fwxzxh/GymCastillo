using System;
using System.Threading.Tasks;
using GymCastillo.Model.Interfaces;
using log4net;

namespace GymCastillo.Model.DataTypes.Ventas {
    /// <summary>
    /// Clase que contiene los métodos y campos del tipo de dato venta.
    /// </summary>
    public class Venta : IOnlyAlta{
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// El id de la venta.
        /// </summary>
        public int IdVenta { get; set; }

        /// <summary>
        /// La fecha en la que ha ocurrido la venta.
        /// </summary>
        public DateTime FechaVenta { get; set; }

        /// <summary>
        /// El id del producto a vender
        /// </summary>
        public int IdProducto { get; set; }

        /// <summary>
        /// El concepto de la venta.
        /// </summary>
        // TODO: igual y esto no va.
        public string Concepto { get; set; }

        /// <summary>
        /// El costo final de la venta.
        /// </summary>
        public decimal Costo { get; set; }

        public Task<int> Alta() {
            Log.Debug("Se ha iniciado el proceso de dar de alta una venta.");
            throw new NotImplementedException();
        }
    }
}