using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GymCastillo.Model.DataTypes.Abstract {
    /// <summary>
    /// Clase abstracta que contiene los campos y métodos base para Cliente, Instructor, Cliente Renta y Usuario
    /// </summary>
    public abstract class AbstUsuario {

        /// <summary>
        /// Id en la base de datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre sin apellidos.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido Paterno.
        /// </summary>
        public string ApellidoPaterno { get; set; }

        /// <summary>
        /// Apellido Materno.
        /// </summary>
        public string ApellidoMaterno { get; set; }

        /// <summary>
        /// Domicilio actual del usuario
        /// </summary>
        public string Domicilio { get; set; }

        /// <summary>
        /// Fecha de nacimiento.
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Número de teléfono.
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Nombre completo del contacto de emergencia.
        /// </summary>
        public string NombreContacto { get; set; }

        /// <summary>
        /// Telefono del contacto de emergencia
        /// </summary>
        public string TelefonoContacto { get; set; }

        /// <summary>
        /// Foto del usuario
        /// </summary>
        //TODO: Ver como manejar las fotos.
        public BitmapImage Foto { get; set; }


        /// <summary>
        /// Método que Actualiza el objeto en la base de datos.
        /// </summary>
        /// <returns>El número de col afectadas.</returns>
        public abstract Task<int> Update();

        /// <summary>
        /// Método que Borra el objeto en la base de datos.
        /// </summary>
        /// <returns>El número de col afectadas.</returns>
        public abstract Task<int> Delete();

        /// <summary>
        /// Método que da de alta el objeto en la base de datos.
        /// </summary>
        /// <returns>El número de col afectadas.</returns>
        public abstract Task<int> Alta();
    }
}