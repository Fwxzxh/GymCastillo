using System.Threading.Tasks;
using GymCastillo.Model.Interfaces;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que contiene todos los métodos y campos del tipo de datos Espacio.
    /// </summary>
    public class Espacio : IOtrosTipos{

        /// <summary>
        /// Id del espacio.
        /// </summary>
        public int IdEspacio { get; set; }

        /// <summary>
        /// Nombre del espacio.
        /// </summary>
        public string NombreEspacio { get; set; }

        /// <summary>
        /// Descripción del espacio.
        /// </summary>
        public string Descripción { get; set; }


        public Task<int> Update() {
            throw new System.NotImplementedException();
        }

        public Task<int> Delete() {
            throw new System.NotImplementedException();
        }

        public Task<int> Alta() {
            throw new System.NotImplementedException();
        }
    }
}