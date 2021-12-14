using System;
using ImageMagick;

namespace GymCastillo.Model.Helpers {
    /// <summary>
    /// Clase que se encarga de crear y guardar la credencial digital de los clientes.
    /// </summary>
    public static class DigitalCard {

        /// <summary>
        /// Método que se encarga de dibujar sobre la plantilla de la credencial
        /// </summary>
        public static void DrawCard() {
            try {
                const string plantillaPath = @"C:/GymCastillo/Assets/IdentifiacionP1.png";
                var saveDir = $"C:/GymCastillo/CardTestDone.png";

                // Obtenemos los datos principales
                // TODO: hacer implementación para obtenerlos de verdad
                var test = "hellooo helooo";
                var apellidos = $"Alegría Escobedo";
                var nombres = $"Juan Pablo";
                var id = $"12";
                var teléfono = $"4421897740";

                // Verificar si tiene foto de perfil guardada.
                // if (Cliente.Foto == null) {
                //     // No tiene foto dada de alta.
                // TODO: ver como guardar este pedo en la carpeta de la solución.
                    var genericProfile = new MagickImage(@"C:/GymCastillo/Assets/GenericProfile.png");


                    // }
                // else {
                //
                //}

                // Definimos los settings de la fuente.
                var readSettings = new MagickReadSettings {
                    Font = "Calibri",
                    FillColor = MagickColors.Yellow,
                    TextGravity = Gravity.Center,
                    BackgroundColor = MagickColors.Transparent,
                    Height = 40, // height of text box
                    Width = 80 // width of text box
                };

                using (var plantilla = new MagickImage(plantillaPath)) {
                    using var idLabel = new MagickImage($"caption:{id}", readSettings);
                    using var apellidosLabel = new MagickImage($"caption:{apellidos}", readSettings);
                    using var nombresLabel = new MagickImage($"caption:{nombres}", readSettings);
                    using var telefonoLabel = new MagickImage($"caption:{teléfono}", readSettings);

                        // Ponemos la imagen de perfil debajo de la
                        plantilla.Composite(genericProfile, 146, 147, CompositeOperator.DstOver);

                        // Aplicamos el Id del usuario
                        plantilla.Composite(idLabel, 10, 10, CompositeOperator.Over);

                        plantilla.Write(saveDir);
                }
            }
            catch (Exception e) {
                ShowPrettyMessages.ErrorOk($"{e.Message}", "Error");
                Console.WriteLine(e);
                throw;
            }
        }
    }
}