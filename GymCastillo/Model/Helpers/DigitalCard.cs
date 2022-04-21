using System;
using System.Drawing;
using System.IO;
using System.Linq;
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
        public static void DrawCard(Cliente cliente, bool silent=false) {
            Log.Warn("Ha iniciado el proceso de generar la credencial de un cliente.");

            try {

                const string plantillaFrontPath = @"C:/GymCastillo/Assets/PlantillaFront.png";
                const string plantillaBackPath = @"C:/GymCastillo/Assets/PlantillaBack.png";
                const string genericProfilePath = @"C:/GymCastillo/Assets/GenericProfile.png";

                // Verificamos que exista la plantilla.
                if (!File.Exists(plantillaFrontPath) && !File.Exists(plantillaBackPath)) {
                    throw new FileNotFoundException(
                        $"No se ha encontrado el archivo con la plantilla de la credencial, " +
                        $"verifique su existencia en la ruta {cliente.ClienteDir}");
                }

                // Verificamos que exista la imagen de perfil genérico
                if (!File.Exists(genericProfilePath)) {
                    throw new FileNotFoundException(
                        $"No se ha encontrado el archivo con la imagen de perfil por defecto, " +
                        $"verifique su existencia en la ruta {genericProfilePath}");
                }

                var saveRoute = cliente.ClienteDir;
                var saveFileFront = $"CardFront-{cliente.Id.ToString()}.png";
                var saveFileBack = $"CardBack-{cliente.Id.ToString()}.png";

                var saveDirFront = $"{saveRoute}{saveFileFront}";
                var saveDirBack = $"{saveRoute}{saveFileBack}";

                // Creamos la carpeta del cliente
                Directory.CreateDirectory(saveRoute);

                // Obtenemos los datos principales
                var apellidos = $"{cliente.ApellidoPaterno} {cliente.ApellidoMaterno}";
                var nombres = cliente.Nombre;
                var nombrePila = $"{cliente.Nombre.Split(" ").First()} {cliente.ApellidoPaterno}";
                var id = cliente.Id.ToString();
                var fechaRegistro = cliente.FechaRegistro.Date;
                var code = $"1{id}".PadLeft(12, '0');

                // verificamos si tiene foto de perfil guardada.
                var profileImage = cliente.FotoRaw.Length == 0
                    ? new MagickImage(genericProfilePath)
                    : cliente.Foto;

                // Definimos los settings de la fuente.
                // Settings del Id
                var idSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.White,
                    TextGravity = Gravity.Center,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 40, // height of text box
                    Width = 40 // width of text box
                };

                // Definimos los settings de los nombres.
                var nombreSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.Black,
                    TextGravity = Gravity.West,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 50, // height of text box
                    Width = 650 // width of text box
                };

                // Definimos lo settings de la fecha de registro
                var fechaRegistroSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.Black,
                    TextGravity = Gravity.West,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 50, // height of text box
                    Width = 650 // width of text box
                };

                // Definimos lo settings de la fecha de registro
                var nombrePilaSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.White,
                    TextGravity = Gravity.West,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 50, // height of text box
                    Width = 650 // width of text box
                };

                // Creamos el código de barras.
                var b = new Barcode();

                b.Encode(
                    TYPE.CODE128,
                    code,
                    Color.Black,
                    Color.White,
                    350, 190);

                using var plantillaFront = new MagickImage(plantillaFrontPath);
                using var plantillaBack = new MagickImage(plantillaBackPath);

                using var nombrePilaLabel = new MagickImage($"caption:{nombrePila}", nombrePilaSettings);
                using var idLabel = new MagickImage($"caption:{id}", idSettings);
                using var apellidosLabel = new MagickImage($"caption:{apellidos}", nombreSettings);
                using var nombresLabel = new MagickImage($"caption:{nombres}", nombreSettings);
                using var fechaRegistroLabel =
                    // ReSharper disable once HeapView.BoxingAllocation
                    new MagickImage($"caption:{fechaRegistro:dd/MM/yyyy}", fechaRegistroSettings);

                using var barcodeImg = new MagickImage(b.Encoded_Image_Bytes);

                // Ponemos el nombre de pila
                plantillaFront.Composite(nombrePilaLabel, 50, 420, CompositeOperator.Over);

                // Ponemos la imagen de perfil
                plantillaBack.Composite(profileImage, 90, 190, CompositeOperator.Over);

                // Aplicamos el Id del usuario
                plantillaFront.Composite(idLabel, 10, 10, CompositeOperator.Over);

                // // Aplicamos el label de los nombres
                plantillaBack.Composite(nombresLabel, 540, 210, CompositeOperator.Over);

                // Aplicamos el label de los apellidos
                plantillaBack.Composite(apellidosLabel, 540, 346, CompositeOperator.Over);

                // Aplicamos el label de la fecha de registro
                plantillaBack.Composite(fechaRegistroLabel, 540, 480, CompositeOperator.Over);

                // Agregamos el código de barras
                plantillaFront.Composite(barcodeImg, 885, 160, CompositeOperator.Over);

                // Guardamos
                plantillaFront.Write(saveDirFront);
                plantillaBack.Write(saveDirBack);

                Log.Debug("Se ha creado la nueva credencial con éxito.");

                if (!silent) {
                    ShowPrettyMessages.NiceMessageOk(
                        $"Se ha generado la nueva credencial con éxito en la carpeta: {cliente.ClienteDir}.",
                        "Credencial Generada");
                }
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