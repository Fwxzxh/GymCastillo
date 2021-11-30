namespace GymCastillo.Model.DataTypes {

    /// <summary>
    /// Clase que contiene los campos del objeto tipo config.
    /// </summary>
    public class ConfigFields {

        /// <summary>
        /// El usuario con el que se accede a la base de datos.
        /// </summary>
        public string DbUser { get; set; }

        /// <summary>
        /// El password con el que se accede a la base de datos.
        /// </summary>
        public string DbPass { get; set; }

        /// <summary>
        /// El costo a aplicar para cobrar el locker.
        /// </summary>
        public string CostoLocker { get; set; }

        /// <summary>
        /// El monto a descontar por el retardo en la entrada de un instructor.
        /// </summary>
        public string DescuentoRetardo { get; set; }
    }
}