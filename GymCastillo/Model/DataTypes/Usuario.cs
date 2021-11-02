using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que se encarga de guardar los campos y métodos de objeto tipo Usuario
    /// </summary>
    public class Usuario : AbstUsuario {

        /// <summary>
        /// El nombre de usuario de el usuario.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// La contraseña del usuario.
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// Método que Actualiza la instancia actual del objeto en la base de datos.
        /// </summary>
        /// <returns></returns>
        public override int Update() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Método que borra la instancia actual del objeto en la base de datos.
        /// </summary>
        /// <returns></returns>
        public override int Delete() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Método que da de alta la instancia actual del objeto en la base de datos.
        /// </summary>
        /// <returns></returns>
        public override int Alta() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Método que valida la instancia actual de la base de datos.
        /// </summary>
        /// <returns></returns>
        public override bool Validate() {
            throw new System.NotImplementedException();
        }
    }
}