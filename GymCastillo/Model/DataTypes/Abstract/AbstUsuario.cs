using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GymCastillo.Model.Helpers;
using ImageMagick;

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
        /// El domicilio del cliente.
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
        /// Array de bytes con la información de la imagen.
        /// </summary>
        private byte[] foto = Array.Empty<byte>();

        /// <summary>
        /// Foto del usuario en formato para manipular.
        /// </summary>
        public MagickImage Foto {
            get => new(foto);
            set {
                // Creamos la geometría con el tamaño deseado.
                var size = new MagickGeometry(355, 355) {
                    IgnoreAspectRatio = true
                };

                // La ponemos en el tamaño adecuado
                value.Resize(size);
                var bytes = value.ToByteArray();

                // Verificamos que la imagen pueda caber en la base de datos y si no bajamos la calidad.
                if (bytes.Length >= 64000) {
                    // Quality base 75
                    value.Quality = 60;
                }

                // Si sigue demasiado grande mandamos un mensaje.
                if (bytes.Length >= 64000) {
                    ShowPrettyMessages.WarningOk(
                        "Esta imagen es demasiado grande para ser guardada en la base de datos, elija otra o comprímala.",
                        "Imagen Demasiado Grande");
                    return;
                }

                foto = value.ToByteArray();
            }
        }

        /// <summary>
        /// Foto raw en un array de bytes.
        /// </summary>
        public byte[] FotoRaw {
            get => foto;
            set => foto = value;
        }

        /// <summary>
        /// Foto en bitmapImage Para el Front
        /// </summary>
        public BitmapImage FotoBitmap {
            get {
                var image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                using var mem = new MemoryStream(foto);
                mem.Position = 0;
                image.StreamSource = mem;
                image.EndInit();
                image.Freeze();
                return image;
            }

        }

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