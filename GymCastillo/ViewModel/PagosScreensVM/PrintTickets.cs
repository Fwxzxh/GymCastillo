using GymCastillo.Model.DataTypes.Movimientos;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.PagosScreensVM {
    public class PrintTickets {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        private Font consola = new("Arial", 10);

        private PrintDocument pd = new();

        private string concepto;

        private decimal total = 0.0m;
        public PrintTickets(string concepto, decimal total) {
            this.concepto = concepto;
            this.total = total;
            pd.PrinterSettings.PrinterName = "EPSON TM-T88V Receipt";
            pd.PrintPage += new PrintPageEventHandler(PrintTicket);
            pd.Print();
        }

        private void PrintTicket(object sender, PrintPageEventArgs ppeArgs) {
            System.Drawing.Image image = System.Drawing.Image.FromFile(@"C:\GymCastillo\Assets\CastilloF2.png");
            var newimage = ResizeImage(image, 500, 350);

            Point ulCorner = new Point(55, 0);

            Graphics g = ppeArgs.Graphics;
            var settings = ppeArgs.PageSettings;
            float yPos = 125;
            int count = 0;
            //Read margins from PrintPageEventArgs      
            float leftMargin = 0;
            int renglon = 18;
            var totalVenta = 0m;

            g.DrawImage(newimage, ulCorner);
            //g.DrawString("qwertyuiopasdfghjklñzxcvbnmqwertyuiopasdf", consola, Brushes.Black, leftMargin, yPos);
            //renglon += 15;
            g.DrawString("Concepto                                            Total", consola, Brushes.Black, leftMargin, yPos + renglon);
            renglon += 15;
            renglon += 15;
            renglon += 15;
            g.DrawString("----------------------------------------------------------------", consola, Brushes.Black, leftMargin, yPos + renglon);
            renglon += 15;
            g.DrawString(string.Format("{0,2}", concepto), consola, Brushes.Black, leftMargin, yPos + renglon);

            g.DrawString(string.Format("                     {0,40}", total), consola, Brushes.Black, leftMargin, yPos + renglon);
            renglon += 15;
            renglon += 15;
            renglon += 15;
            var newYpos = renglon + 15;
            g.DrawString("----------------------------------------------------------------", consola, Brushes.Black, leftMargin, newYpos);
            newYpos += 15;
            newYpos += 15;
            g.DrawString(string.Format("Total Venta {0,40}", total), consola, Brushes.Black, leftMargin, newYpos);
            newYpos += 15;
            newYpos += 15;
            g.DrawString($"Fecha: {DateTime.Now}", consola, Brushes.Black, leftMargin, newYpos);
            newYpos += 15;
            newYpos += 15;
            g.DrawString("                   Gracias por su compra!", consola, Brushes.Black, leftMargin, newYpos);

        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private static Bitmap ResizeImage(Image image, int width, int height) {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes()) {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
