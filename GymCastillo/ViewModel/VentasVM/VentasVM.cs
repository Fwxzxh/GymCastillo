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
        public RelayCommand AddProduct { get; set; }
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

            set
            {
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

        private bool visita;

        public bool Visita {
            get { return visita; }
            set
            {
                visita = value;
                OnPropertyChanged(nameof(Visita));
                //RefreshGrid();
            }
        }


        private bool gym = false;

        public bool Gym {
            get { return gym; }
            set
            {
                gym = value;
                OnPropertyChanged(nameof(Gym));
                UpdateSale();
            }
        }


        private bool box = false;

        public bool Box {
            get { return box; }
            set
            {
                box = value;
                OnPropertyChanged(nameof(Box));
                UpdateSale();
            }
        }

        private bool alberca = false;

        public bool Alberca {
            get { return alberca; }
            set
            {
                alberca = value;
                OnPropertyChanged(nameof(Alberca));
                UpdateSale();

            }
        }

        private string nombreProducto;

        public string NombreProducto {
            get { return nombreProducto; }
            set
            {
                nombreProducto = value;
                OnPropertyChanged(nameof(NombreProducto));
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

        private int noVisitas = 1;

        public int NoVisitas {
            get { return noVisitas; }
            set
            {
                noVisitas = value;
                OnPropertyChanged(nameof(NoVisitas));
                UpdateSale();
            }
        }


        public VentasVM() {
            ListaVenta = new ObservableCollection<Inventario>();
            AddVenta = new RelayCommand(NuevoProducto);
            RemoveVenta = new RelayCommand(RemoverProducto);
            MakeVenta = new RelayCommand(HacerVenta);
            CancelVenta = new RelayCommand(Cancelar);
            AddProduct = new RelayCommand(AgregarProductoFromName);
            pd.PrinterSettings.PrinterName = "EPSON TM-T88V Receipt";
            Refresh();
        }

        private void AgregarProductoFromName() {
            if (string.IsNullOrEmpty(NombreProducto)) return;
            try {
                var producto = InitInfo.ObCoInventario.First(x => x.NombreProducto == NombreProducto);
                if (producto != null) {
                    //ShowPrettyMessages.InfoOk(producto.NombreProducto, "hola");
                    ListaVenta.Add(producto);
                    RefreshCosto();
                    NombreProducto = "";
                }
            }
            catch (Exception) {
                ShowPrettyMessages.WarningOk("El producto no existe", "Error");
                NombreProducto = "";
            }
        }

        private void PrintTicket(object sender, PrintPageEventArgs ppeArgs) {
            Image image = Image.FromFile(@"C:\GymCastillo\Assets\logo.jpg");
            var newimage = ResizeImage(image, 150, 100);

            Point ulCorner = new Point(30, -10);

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
            if (ListaVenta.Count() == 0) {
                if (gym) {
                    var total = venta.Costo;
                    var name = "";
                    totalVenta += venta.Costo;

                    name = venta.Concepto;

                    yPos = topMargin + (count * consola.GetHeight(g));
                    g.DrawString(string.Format("{0,2}", name), consola, Brushes.Black, leftMargin, yPos);
                    g.DrawString(string.Format("                     {0,40}", total), consola, Brushes.Black, leftMargin, yPos);
                    count++;
                }
                else if (alberca) {
                    var total = venta.Costo;
                    var name = "";
                    totalVenta += venta.Costo;

                    name = venta.Concepto;

                    yPos = topMargin + (count * consola.GetHeight(g));
                    g.DrawString(string.Format("{0,2}", name), consola, Brushes.Black, leftMargin, yPos);
                    g.DrawString(string.Format("                     {0,40}", total), consola, Brushes.Black, leftMargin, yPos);
                    count++;
                }
                else if (box) {
                    var total = venta.Costo;
                    var name = "";
                    totalVenta += venta.Costo;

                    name = venta.Concepto;

                    yPos = topMargin + (count * consola.GetHeight(g));
                    g.DrawString(string.Format("{0,2}", name), consola, Brushes.Black, leftMargin, yPos);
                    g.DrawString(string.Format("                     {0,40}", total), consola, Brushes.Black, leftMargin, yPos);
                    count++;
                }

            }
            else {
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

            }
            var newYpos = yPos + 15;
            g.DrawString("----------------------------------------------------------------", consola, Brushes.Black, leftMargin, newYpos);
            newYpos += 15;
            g.DrawString(string.Format("Total Venta {0,40}", totalVenta), consola, Brushes.Black, leftMargin, newYpos);
            newYpos += 15;
            newYpos += 15;
            g.DrawString($"Fecha: {DateTime.Now}", consola, Brushes.Black, leftMargin, newYpos);
            newYpos += 15;
            g.DrawString($"No. Recibo: {GetInitData.GetMonthMovNumerator()}", consola, Brushes.Black, leftMargin, newYpos);
            newYpos += 15;
            newYpos += 15;
            g.DrawString("                   Gracias por su compra!", consola, Brushes.Black, leftMargin, newYpos);
        }

        private void Cancelar() {
            Visita = false;
            ClearFields();
        }

        private void UpdateSale() {
            if (gym) {
                if (ListaVenta.Count > 0) {
                    Costo = 0;
                    foreach (var item in ListaVenta) {
                        Costo += item.Costo;
                    }
                    Costo += (GetInitData.VisitaGym * NoVisitas);
                    Concepto = $"Visita gym x{NoVisitas} más venta de productos.";
                }
                else {
                    Costo = (GetInitData.VisitaGym * NoVisitas);
                    Concepto = $"Visita Gym x{NoVisitas}";
                }
                Venta.Concepto = Concepto;
                Venta.Costo = Costo;
            }
            else if (box) {
                if (ListaVenta.Count > 0) {
                    Costo = 0;
                    foreach (var item in ListaVenta) {
                        Costo += item.Costo;
                    }
                    Costo += (GetInitData.VisitaBox * NoVisitas);
                    Concepto = $"Visita Box x{NoVisitas} más venta de productos.";
                }
                else {
                    Costo = (GetInitData.VisitaBox * NoVisitas);
                    Concepto = $"Visita Box x{NoVisitas}";
                }
                Venta.Concepto = Concepto;
                Venta.Costo = Costo;
            }
            else if (alberca) {
                if (ListaVenta.Count > 0) {
                    Costo = 0;
                    foreach (var item in ListaVenta) {
                        Costo += item.Costo;
                    }
                    Costo += (GetInitData.VisitaAlberca * NoVisitas);
                    Concepto = $"Visita Alberca x{NoVisitas} más venta de productos.";
                }
                else {
                    Costo = (GetInitData.VisitaAlberca * NoVisitas);
                    Concepto = $"Visita Alberca x{NoVisitas}";
                }
                Venta.Concepto = Concepto;
                Venta.Costo = Costo;
            }
            else {
                Costo = 0;
                foreach (var item in ListaVenta) {
                    Costo += item.Costo;
                }
            }
        }

        private async void HacerVenta() {
            Venta.FechaVenta = DateTime.Now;
            Venta.Concepto = Concepto;
            Venta.Costo = Costo;
            Venta.VisitaGym = Visita;

            if (venta.Costo == 0 && ListaVenta.Count == 0) {
                ShowPrettyMessages.ErrorOk(
                    "No puedes dar de alta una venta vacía.",
                    "Venta vacía");
                return;
            }

            if (ListaVenta.Count > 0) {
                foreach (var item in ListaVenta) {
                    Venta.IdsProductos += $"{item.IdProducto},";
                }
            }

            // await AdminOnlyAlta.Alta(Venta);
            try {
                pd.PrintPage += new PrintPageEventHandler(PrintTicket);
                pd.Print();
                pd.Print();
            }
            catch (Exception e) {
                ShowPrettyMessages.ErrorOk("No se encontró una impresora de tickets.", "Error");
            }
            finally {

                await VentasHelper.NuevaVenta(Venta, Recibido);
            }

            if (!visita) { // Solo si no es una entrada a un gym.
                // Actualizamos el inventario por las existencias.
                var updatedInventario = await GetFromDb.GetInventario();
                InitInfo.ObCoInventario.Clear();
                foreach (var item in updatedInventario) {
                    InitInfo.ObCoInventario.Add(item);
                }
            }

            Refresh();
            ClearFields();
        }

        private void ClearFields() {
            Costo = 0.0m;
            Recibido = 0.0m;
            Total = 0.0m;
            Concepto = "";
            Venta = null;
            Venta = new();
            Gym = false;
            Box = false;
            Alberca = false;
            NombreProducto = "";
            NoVisitas = 1;
            Visita = false;
            ListaVenta.Clear();
        }

        private void RemoverProducto() {
            ListaVenta.RemoveAt(Index);
            //RefreshCosto();
            UpdateSale();
        }

        private void NuevoProducto() {
            ListaVenta.Add(Selected);
            //RefreshCosto();
            UpdateSale();
        }

        private void RefreshGrid() {
            if (ListaVenta != null || ListaVenta.Count != 0) {
                ListaVenta.Clear();
            }
            Costo = 0;
            Recibido = 0;
            Total = 0;
            Concepto = "";
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
            foreach (var item in ListaVenta) {
                if (Costo != 0) {
                    Costo += item.Costo;
                }
            }
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
