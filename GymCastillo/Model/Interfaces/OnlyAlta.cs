using System.Threading.Tasks;

namespace GymCastillo.Model.Interfaces {
    /// <summary>
    /// Interfaz que implementan los tipos que solo ocupan darse de alta.
    /// </summary>
    public interface IOnlyAlta {

        /// <summary>
        /// Da de alta la instancia actual del objeto en la base de datos.
        /// </summary>
        /// <returns>la cantidad de columnas afectadas por la operación.</returns>
        public Task<int> Alta();
    }
}