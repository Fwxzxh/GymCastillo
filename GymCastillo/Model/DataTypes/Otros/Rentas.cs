using System;
using System.Threading.Tasks;
using GymCastillo.Model.Interfaces;

namespace GymCastillo.Model.DataTypes.Otros {

    /// <summary>
    /// Clase que contiene los métodos y campos de la clase de rentas.
    /// </summary>
    public class Rentas : IOnlyAlta{

        /// <summary>
        /// Id de la renta.
        /// </summary>
        public int IdRenta { get; set; }

        /// <summary>
        /// Id del cliente al que se le hizo la renta.
        /// </summary>
        public int IdClienteRenta { get; set; }

        /// <summary>
        /// El nombre del cliente al que se le hizo la renta.
        /// </summary>
        public string NombreClienteRenta { get; set; }

        /// <summary>
        /// El id del espacio a rentar.
        /// </summary>
        public int IdEspacio { get; set; }

        /// <summary>
        /// El nombre del espacio a rentar.
        /// </summary>
        public string NombreEspacio { get; set; }

        /// <summary>
        /// Int que representa el dia en que se renta el espacio.
        /// </summary>
        public int Dia { get; set; }

        /// <summary>
        /// La hora de inicio de la renta.
        /// </summary>
        public DateTime HoraInicio { get; set; }

        /// <summary>
        /// La hora en la que termina la renta.
        /// </summary>
        public DateTime HoraFin { get; set; }

        /// <summary>
        /// El costo de la renta.
        /// </summary>
        public decimal Costo { get; set; }

        public Task<int> Alta() {
            throw new NotImplementedException();
        }
    }
}