using System;
using System.IO;
using GymCastillo.Model.DataTypes.Personal;
using ImageMagick;
using log4net;

namespace GymCastillo.Model.Helpers {

    /// <summary>
    /// Clase que se encarga de crear y guardar la credencial digital de los clientes.
    /// </summary>
    public static class DigitalCard {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de dibujar sobre la plantilla de la credencial
        /// </summary>
        public static void DrawCard(Cliente cliente) {
            Log.Warn("Ha iniciado el proceso de generar la credencial de un cliente.");

            try {

                // TODO: ver que onda con esto
                const string plantillaPath = @"C:/GymCastillo/Assets/IdentifiacionP1.png";

                var saveRoute = $"C:/GymCastillo/Clientes/{cliente.Id.ToString()}-{cliente.ApellidoPaterno}/";
                var saveFile = $"Card-{cliente.Id.ToString()}.png";
                var saveDir = saveRoute + saveFile;

                // Creamos la carpeta del cliente
                Directory.CreateDirectory(saveRoute);

                // Obtenemos los datos principales
                var apellidos = $"{cliente.ApellidoPaterno} {cliente.ApellidoMaterno}";
                var nombres = cliente.Nombre;
                var id = cliente.Id.ToString();
                var teléfono = cliente.Telefono;


                // Verificar si tiene foto de perfil guardada.
                var profileImage = cliente.Foto == null
                    ? new MagickImage(@"C:/GymCastillo/Assets/GenericProfile.png")
                    : new MagickImage(@"C:/GymCastillo/Assets/something.png");

                // Definimos los settings de la fuente.
                var idSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.Yellow,
                    TextGravity = Gravity.Center,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 40, // height of text box
                    Width = 40 // width of text box
                };

                var nombreSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.White,
                    TextGravity = Gravity.Center,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 50, // height of text box
                    Width = 650 // width of text box
                };

                var telefonoSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.White,
                    TextGravity = Gravity.Center,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 30, // height of text box
                    Width = 650 // width of text box
                };

                using var plantilla = new MagickImage(plantillaPath);
                using var idLabel = new MagickImage($"caption:{id}", idSettings);
                using var apellidosLabel = new MagickImage($"caption:{apellidos}", nombreSettings);
                using var nombresLabel = new MagickImage($"caption:{nombres}", nombreSettings);
                using var telefonoLabel = new MagickImage($"caption:{teléfono}", telefonoSettings);

                // Ponemos la imagen de perfil debajo de la
                plantilla.Composite(profileImage, 146, 147, CompositeOperator.DstOver);

                // Aplicamos el Id del usuario
                plantilla.Composite(idLabel, 10, 10, CompositeOperator.Over);

                // Aplicamos el label de los apellidos
                plantilla.Composite(apellidosLabel, 0, 515, CompositeOperator.Over);

                // Aplicamos el label de los nombres
                plantilla.Composite(nombresLabel, 0, 565, CompositeOperator.Over);

                // Aplicamos el label de los nombres
                plantilla.Composite(telefonoLabel, 0, 620, CompositeOperator.Over);

                // TODO: agregar el código de barras cuando este.

                // Guardamos
                plantilla.Write(saveDir);
                Log.Debug("Se ha creado la nueva credencial con éxito.");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al generar la credencial de un usuario.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al generar la credencial, Error: {e.Message}",
                    "Error desconocido al generar la credencial");
            }
        }
    }
}