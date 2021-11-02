namespace GymCastillo.Model.DataTypes.Init {
    /// <summary>
    /// Helper que expone toda la información necesaria para el inicio del programa.
    /// </summary>
    public static class GetInitData {

        /// <summary>
        /// Cadena de conneción a la base de datos.
        /// </summary>
        public static string ConnString { get; set; }

        /// <summary>
        /// Método que obtiene el string de connección y lo guarda en el campo de ConnString.
        /// </summary>
        /// <returns>El string de connección a la base de datos.</returns>
        public static string GetConnString() {

            // WARN: Temporal!!!
            var user = "root";
            var pass = "Jorgedavid12";

            var connString = $"server=localhost; database=mexty; Uid={user}; pwd={pass}; Database=gymcastillo";
            return connString;
        }

    }
}