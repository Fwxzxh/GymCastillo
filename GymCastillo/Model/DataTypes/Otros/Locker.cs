namespace GymCastillo.Model.DataTypes.Otros {
    /// <summary>
    /// Clase que contiene las propiedades del Tipo Locker
    /// </summary>
    public class Locker {

        /// <summary>
        /// Id del Locker en bd.
        /// </summary>
        public int IdLocker { get; set; }

        /// <summary>
        /// Nombre del locker
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Id del cliente dueño de ese locker.
        /// </summary>
        public bool Ocupado { get; set; }
    }
}