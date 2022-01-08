using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes.Ventas;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GymCastillo.Model.Database;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using System.Linq;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace GymCastillo.ViewModel.VentasVM {
    public class VentasVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand AddVenta { get; set; }
        public RelayCommand RemoveVenta { get; set; }
        public RelayCommand MakeVenta { get; set; }
        public RelayCommand CancelVenta { get; set; }

        private Font consola = new("Arial", 10);

        private PrintDocument pd = new();

        private Venta venta = new();

        public Venta Venta {
            get { return venta; }
            set
            {
                venta = value;
                OnPropertyChanged(nameof(Venta));
            }
        }

        private Inventario selected;

        public Inventario Selected {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        private int index;

        public int Index {
            get { return index; }
            set
            {
                index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        private string concepto;

        public string Concepto {
            get { return concepto; }

            set {
                concepto = value;
                OnPropertyChanged(nameof(concepto));
            }
        }

        private decimal costo;

        public decimal Costo {
            get { return costo; }
            set
            {
                costo = value;
                OnPropertyChanged(nameof(Costo));
            }
        }

        private decimal recibido = 0;

        public decimal Recibido {
            get { return recibido; }
            set
            {
                recibido = value;
                OnPropertyChanged(nameof(Recibido));
                ActualizarTotal();
            }
        }

        private decimal total = 0;

        public decimal Total {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        private bool gym;

        public bool Gym {
            get { return gym; }
            set
            {
                gym = value;
                OnPropertyChanged(nameof(Gym));
                RefreshGrid();
            }
        }

        private ObservableCollection<Inventario> listaVenta;

        public ObservableCollection<Inventario> ListaVenta {
            get { return listaVenta; }
            set
            {
                listaVenta = value;
                OnPropertyChanged(nameof(ListaVenta));
            }
        }

        public VentasVM() {
            ListaVenta = new ObservableCollection<Inventario>();
            AddVenta = new RelayCommand(NuevoProducto);
            RemoveVenta = new RelayCommand(RemoverProducto);
            MakeVenta = new RelayCommand(HacerVenta);
            CancelVenta = new RelayCommand(Cancelar);
            pd.PrinterSettings.PrinterName = "EPSON TM-T88V Receipt";
            Refresh();
        }

        private void PrintTicket(object sender, PrintPageEventArgs ppeArgs) {
            Image image = Image.FromFile(@"C:\GymCastillo\Assets\CastilloF2.png");
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
            g.DrawString("Producto                                            Total", consola, Brushes.Black, leftMargin, yPos + renglon);
            renglon += 15;
            g.DrawString("----------------------------------------------------------------", consola, Brushes.Black, leftMargin, yPos + renglon);
            float topMargin = 145 + renglon;
            foreach (var item in ListaVenta) {
                var total = item.Costo.ToString();
                var name = "";
                totalVenta += item.Costo;

                name = item.NombreProducto;

                yPos = topMargin + (count * consola.GetHeight(g));
                g.DrawString(string.Format("{0,2}", name), consola, Brushes.Black, leftMargin, yPos);
                g.DrawString(string.Format("                     {0,40}", total), consola, Brushes.Black, leftMargin, yPos);
                count++;
            }
            var newYpos = yPos + 15;
            g.DrawString("----------------------------------------------------------------", consola, Brushes.Black, leftMargin, newYpos);
            newYpos += 15;
            newYpos += 15;
            g.DrawString(string.Format("Total Venta ${0,40}", totalVenta), consola, Brushes.Black, leftMargin, newYpos);
            newYpos += 15;
            newYpos += 15;
            g.DrawString("                   Gracias por su compra!", consola, Brushes.Black, leftMargin, newYpos);
        }

        private void Cancelar() {
            Gym = false;
            ClearFields();
        }

        private async void HacerVenta() {

            if (venta.Costo == 0) {
                ShowPrettyMessages.ErrorOk(
                    "No se puede hacer una venta con el costo en 0",
                    "Error en venta.");
                return;
            }

            Venta.FechaVenta = DateTime.Now;
            Venta.Concepto = Concepto;
            Venta.Costo = Costo;
            Venta.VisitaGym = Gym;

            foreach (var item in ListaVenta) {
                Venta.IdsProductos += $"{item.IdProducto},";
            }
            // await AdminOnlyAlta.Alta(Venta);
            await VentasHelper.NuevaVenta(Venta, Recibido);
            pd.PrintPage += new PrintPageEventHandler(PrintTicket);
            pd.Print();
            ClearFields();

            if (!gym) { // Solo si no es una entrada a un gym.
                // Actualizamos el inventario por las existencias.
                var updatedInventario = await GetFromDb.GetInventario();
                InitInfo.ObCoInventario.Clear();
                foreach (var item in updatedInventario) {
                    InitInfo.ObCoInventario.Add(item);
                }
            }


            Refresh();
        }

        private void ClearFields() {
            Costo = 0;
            Recibido = 0;
            Total = 0;
            Concepto = "";
            Venta = null;
            venta = new();
            ListaVenta.Clear();
        }

        private void RemoverProducto() {
            ListaVenta.RemoveAt(Index);
            RefreshCosto();
        }

        private void NuevoProducto() {
            ListaVenta.Add(Selected);
            RefreshCosto();
        }

        private void RefreshGrid() {
            if (ListaVenta != null) {
                ListaVenta.Clear();
            }

            ClearFields();
        }

        private async void Refresh() {
            // Actualizamos los ingresos
            var updatedIngresos = await GetFromDb.GetIngresos();
            InitInfo.ObCoIngresos.Clear();
            foreach (var item in updatedIngresos.OrderByDescending(c => c.FechaRegistro)) {
                InitInfo.ObCoIngresos.Add(item);
            }
        }

        private void ActualizarTotal() {
            if (Recibido != 0) {
                Total = Math.Max(Recibido - Costo, 0);
            }
        }

        private void RefreshCosto() {
            Costo = 0;
            var costo = 0.0m;
            foreach (var item in ListaVenta) {
                costo += item.Costo;
            }
            Costo += costo;
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

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
