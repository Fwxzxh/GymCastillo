using System.Threading.Tasks;

namespace GymCastillo.Model.DataTypes.Abstract {
    /// <summary>
    /// Clase abstracta que une los campos y métodos comunes de Clase, Paquete, Espacio y horario
    /// </summary>
    public abstract class AbstOtrosTipos {

        /// <summary>
        /// Se encarga de hacer update del objeto en cuestión.
        /// </summary>
        /// <returns>La cantidad de Columnas afectadas en la operación.</returns>
        public abstract Task<int> Update();

        /// <summary>
        /// Se encarga de hacer delete del objeto en cuestión.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas en la operación.</returns>
        public abstract Task<int> Delete();

        /// <summary>
        /// Se encarga de hacer la alta del objeto en cuestión.
        /// </summary>
        /// <returns>La cantidad de columnas afectadas en la operación. </returns>
        public abstract Task<int> Alta();
    }
}