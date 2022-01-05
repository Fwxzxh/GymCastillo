using System;
using System.Drawing;
using System.IO;
using BarcodeLib;
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

                const string plantillaPath = @"C:/GymCastillo/Assets/Plantilla.png";
                const string genericProfilePath = @"C:/GymCastillo/Assets/GenericProfile.png";

                // Verificamos que exista la plantilla.
                if (!File.Exists(plantillaPath)) {
                    throw new FileNotFoundException(
                        $"No se ha encontrado el archivo con la plantilla de la credencial, verifique su existencia en la ruta {plantillaPath}");
                }

                // Verificamos que exista la plantilla
                if (!File.Exists(genericProfilePath)) {
                    throw new FileNotFoundException(
                        $"No se ha encontrado el archivo con la imagen de perfil por defecto, verifique su existencia en la ruta {genericProfilePath}");
                }

                var saveRoute = cliente.ClienteDir;
                var saveFile = $"Card-{cliente.Id.ToString()}.png";
                var saveDir = $"{saveRoute}{saveFile}";

                // Creamos la carpeta del cliente
                Directory.CreateDirectory(saveRoute);

                // Obtenemos los datos principales
                var apellidos = $"{cliente.ApellidoPaterno} {cliente.ApellidoMaterno}";
                var nombres = cliente.Nombre;
                var id = cliente.Id.ToString();
                var fechaRegistro = cliente.FechaRegistro.Date;
                var code = $"1{id}".PadLeft(12, '0');

                // verificamos si tiene foto de perfil guardada.
                var profileImage = cliente.FotoRaw.Length == 0
                    ? new MagickImage(genericProfilePath)
                    : cliente.Foto;

                // Definimos los settings de la fuente.
                var idSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.Yellow,
                    TextGravity = Gravity.Center,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 40, // height of text box
                    Width = 40 // width of text box
                };

                // Definimos los settings de los nombres.
                var nombreSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.White,
                    TextGravity = Gravity.Center,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 50, // height of text box
                    Width = 650 // width of text box
                };

                // Definimos lo settings de la fecha de registro
                var fechaRegistroSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.White,
                    TextGravity = Gravity.Center,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 30, // height of text box
                    Width = 650 // width of text box
                };

                // Creamos el código de barras.
                var b = new Barcode();

                b.Encode(
                    TYPE.CODE128,
                    code,
                    Color.Black,
                    Color.White,
                    320, 160);

                using var plantilla = new MagickImage(plantillaPath);
                using var idLabel = new MagickImage($"caption:{id}", idSettings);
                using var apellidosLabel = new MagickImage($"caption:{apellidos}", nombreSettings);
                using var nombresLabel = new MagickImage($"caption:{nombres}", nombreSettings);
                using var fechaRegistroLabel =
                    // ReSharper disable once HeapView.BoxingAllocation
                    new MagickImage($"caption:{fechaRegistro:dd/MM/yyyy}", fechaRegistroSettings);
                using var barcodeImg = new MagickImage(b.Encoded_Image_Bytes);

                // Ponemos la imagen de perfil debajo de la plantilla
                plantilla.Composite(profileImage, 146, 140, CompositeOperator.DstOver);

                // Aplicamos el Id del usuario
                plantilla.Composite(idLabel, 10, 10, CompositeOperator.Over);

                // Aplicamos el label de los apellidos
                plantilla.Composite(apellidosLabel, 0, 515, CompositeOperator.Over);

                // Aplicamos el label de los nombres
                plantilla.Composite(nombresLabel, 0, 565, CompositeOperator.Over);

                // Aplicamos el label de la fecha de registro
                plantilla.Composite(fechaRegistroLabel, 0, 620, CompositeOperator.Over);

                // Agregamos el código de barras
                plantilla.Composite(barcodeImg, 160, 800, CompositeOperator.Over);

                // Guardamos
                plantilla.Write(saveDir);
                Log.Debug("Se ha creado la nueva credencial con éxito.");

                ShowPrettyMessages.NiceMessageOk(
                    $"Se ha generado la nueva credencial con éxito en la ruta {saveDir}.",
                    "Credencial Generada");
            }
            catch (FileNotFoundException e) {
                Log.Error("Ha ocurrido un error al generar la credencial de un usuario.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"{e.Message}",
                    "Archivo no encontrado");
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