using System;
using System.Threading.Tasks;

namespace GymCastillo.Model.DataTypes.Abstract {
    /// <summary>
    /// Clase que se encarga de Guardar los campos comunes de los objetos tipo Ingresos y Pagos
    /// </summary>
    public abstract class AbstractMovimientos {

        /// <summary>
        /// Id del movimiento puede ser, IdPago o IdIngreso.
        /// </summary>
        public int IdMovimiento { get; set; }

        /// <summary>
        /// La fecha de registro del movimiento.
        /// </summary>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Id del usuario que hizo ese movimiento.
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nombre del usuario que re
        /// </summary>
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Indica si el motivo es otros.
        /// </summary>
        public bool Otros { get; set; }

        /// <summary>
        /// Indica el concepto del movimiento
        /// </summary>
        public string Concepto { get; set; }

        /// <summary>
        /// Indica el número de recibo.
        /// </summary>
        public string NumeroRecibo { get; set; }

        /// <summary>
        /// Indica el monto del movimiento.
        /// </summary>
        public decimal Monto { get; set; }

        /// <summary>
        /// Método que da de alta un nuevo movimiento.
        /// </summary>
        public abstract Task<int> Alta();
    }
}