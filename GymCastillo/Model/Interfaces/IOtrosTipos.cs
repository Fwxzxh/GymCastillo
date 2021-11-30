using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GymCastillo.Model.Interfaces {
    /// <summary>
    /// Interfaz que se ocupa de unir los campos comunes de Clase, Paquete
    /// </summary>
    public interface IOtrosTipos {

        /// <summary>
        /// Se encarga de hacer update del objeto en cuestión.
        /// </summary>
        /// <returns>La cantidad de Columnas afectadas en la operación.</returns>
        Task<int> Update();

        /// <summary>
        /// Se encarga de hacer delete del objeto en cuestión.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas en la operación.</returns>
        Task<int> Delete();

        /// <summary>
        /// Se encarga de hacer la alta del objeto en cuestión.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas en la operación. </returns>
        Task<int> Alta();

    }
}