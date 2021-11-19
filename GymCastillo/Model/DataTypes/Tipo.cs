namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que contiene los campos del objeto Tipo el cual puede ser TipoCliente o TipoInstructor.
    /// </summary>
    public class TipoCliente {

        /// <summary>
        /// Id del tipo de cliente.
        /// </summary>
        public int IdTipo { get; set; }

        /// <summary>
        /// Nombre del tipo de cliente.
        /// </summary>
        public string NombreTipo { get; set; }

        /// <summary>
        /// Descripción del tipo de cliente.
        /// </summary>
        public string Descripcion { get; set; }
    }
}