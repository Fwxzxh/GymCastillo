using System;
using System.Threading.Tasks;
using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model.DataTypes.Personal {
    /// <summary>
    /// Clase que contiene todos los métodos y campos del tipo de datos personal
    /// </summary>
    public class Personal : AbstUsuario {

        /// <summary>
        /// La fecha de último pago del Personal.
        /// </summary>
        public DateTime FechaUltimoPago { get; set; }

        /// <summary>
        /// El monto del último pago.
        /// </summary>
        public DateTime MontoUltimoPago { get; set; }

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