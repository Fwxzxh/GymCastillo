using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.ComponentModel;
using System.Linq;
using GymCastillo.Model.DataTypes.IntersectionTables;
using GymCastillo.Model.Reportes;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using iText.Layout.Element;
using iText.IO.Image;
using System.Collections.Generic;

namespace GymCastillo.ViewModel.PagosScreensVM {
    public class IngresosVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        private string path = @"C:\GymCastillo\Reportes\Ingresos";
        public RelayCommand PagoCliente { get; set; }
        public RelayCommand PagoOtros { get; set; }
        public RelayCommand PagoRenta { get; set; }
        public RelayCommand MakeReporte { get; set; }

        private PrintTickets tickets;

        private Paquete paquete = new();

        public static (DateTime, int) DoublePagoProtection = (DateTime.Now - TimeSpan.FromMinutes(2), 0);

        public Paquete Paquete {
            get { return paquete; }
            set
            {
                paquete = value;
                OnPropertyChanged(nameof(Paquete));
                ActualizarTotal();
            }
        }

        private Cliente cliente = new();

        public Cliente Cliente {
            get { return cliente; }
            set
            {
                cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }

        private Ingresos ingresos = new();

        public Ingresos Ingresos {
            get { return ingresos; }
            set
            {
                ingresos = value;
                OnPropertyChanged(nameof(Ingresos));
            }
        }

        private ClienteRenta clienteRenta = new();

        public ClienteRenta ClienteRenta {
            get { return clienteRenta; }
            set
            {
                clienteRenta = value;
                OnPropertyChanged(nameof(ClienteRenta));
            }
        }

        private Espacio espacio = new();

        public Espacio Espacio {
            get { return espacio; }
            set
            {
                espacio = value;
                OnPropertyChanged(nameof(Espacio));
            }
        }


        private int item = -1;

        public int Item {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged(nameof(Item));
                ClearData();
            }
        }

        private decimal inscripcion;

        public decimal Inscripcion {
            get { return inscripcion; }
            set
            {
                inscripcion = value;
                OnPropertyChanged(nameof(Inscripcion));
                ActualizarTotal();
            }
        }

        private decimal total;

        public decimal Total {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        private int noMeses;

        public int NoMeses {
            get { return noMeses; }
            set
            {
                noMeses = value;
                OnPropertyChanged(nameof(NoMeses));
                ActualizarTotal();
            }
        }

        private string concepto;

        public string Concepto {
            get { return concepto; }
            set
            {
                concepto = value;
                OnPropertyChanged(nameof(Concepto));
            }
        }


        public IngresosVM() {
            PagoCliente = new RelayCommand(ClientsPayment);
            PagoOtros = new RelayCommand(OthersPayment);
            PagoRenta = new RelayCommand(RentPayment);
            MakeReporte = new RelayCommand(ReporteSemanal);
            Directory.CreateDirectory(path);
            RefreshGrid();
        }

        private void ActualizarTotal() {
            if (Paquete == null) return;
            Concepto = $"Pago mensualidad {Paquete.NombrePaquete}";
            if (Paquete.IdPaquete == 0) Concepto = "";
            Total = (paquete.Costo * (NoMeses + 1)) + inscripcion;
        }

        private async void ReporteSemanal() {
            var lista = await GetReportes.GetReporteIngresos();
            Document document;
            var fontSize = 15;
            string[] columnas = { "Concepto", "Monto Total", "Monto Recibido" };
            float[] tamaños = { 1, 1, 1 };

            try {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(@$"{path}\Ingresos-{DateTime.Now.Day}-{DateTime.Now.Month}.pdf", FileMode.Create, FileAccess.Write)));
                document = new Document(pdfDocument, PageSize.A4);
                document.SetMargins(20, 20, 20, 20);
                ImageData imageData = ImageDataFactory.Create(@"C:\GymCastillo\Assets\logo.jpg");
                Image image = new Image(imageData).ScaleAbsolute(200, 100);
                image.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(image);
                var titulo = "Ingresos Últimos 7 días";
                var montoTotalRecibido = 0.0m;
                var montoTotalTipo = 0.0m;

                document.Add(new Paragraph(titulo).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                Table table = new Table(UnitValue.CreatePercentArray(tamaños));

                document.Add(new Paragraph("Clientes").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in lista.Where(l => l.IdCliente != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.MontoRecibido)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.MontoRecibido;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total recibido: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalRecibido += montoTotalTipo;
                montoTotalTipo = 0;

                document.Add(new Paragraph("Ventas").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());
                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in lista.Where(l => l.IdVenta != 0).OrderBy(x => x.Concepto)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total recibido: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalRecibido += montoTotalTipo;
                montoTotalTipo = 0;

                document.Add(new Paragraph("Rentas").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());
                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in lista.Where(l => l.IdRenta != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.MontoRecibido)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.MontoRecibido;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total recibido: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalRecibido += montoTotalTipo;
                montoTotalTipo = 0;

                document.Add(new Paragraph("Otros").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in lista.Where(l => l.Otros != false)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total recibido: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalRecibido += montoTotalTipo;
                montoTotalTipo = 0;
                //foreach (var item in lista) {
                //    montoTotalRecibido += item.Monto;
                //}
                document.Add(new Paragraph((string.Format("Monto total últimos 7 días: {0:C}", montoTotalRecibido))).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetBold());
                document.Close();
                ShowPrettyMessages.InfoOk($"Documento creado en la ruta {path}", "Reporte Generado");


            }
            catch (Exception e) {
                Log.Debug("Error al crear documento pdf");
                Log.Error(e.Message);
            }
        }

        private async void RentPayment() {
            ingresos.Tipo = 5;
            ingresos.Monto = ClienteRenta.DeudaCliente - Ingresos.MontoRecibido;
            ingresos.IdClienteRenta = ClienteRenta.Id;
            tickets = new($"Pago Renta", ingresos.Monto, noRecibo: GetInitData.GetMonthMovNumerator());
            tickets = new($"Pago Renta", ingresos.Monto, noRecibo: GetInitData.GetMonthMovNumerator());
            await PagosHelper.NewIngreso(ingresos);
            RefreshGrid();
            InitInfo.ObCoClientesRenta.Clear();
            var newClientesRenta = await GetFromDb.GetClientesRenta();
            foreach (var clientR in newClientesRenta) {
                InitInfo.ObCoClientesRenta.Add(clientR);
            }
        }

        private async void OthersPayment() {
            ingresos.Tipo = 4;

            tickets = new PrintTickets($"Pago Otros", ingresos.Monto, noRecibo: GetInitData.GetMonthMovNumerator());
            tickets = new PrintTickets($"Pago Otros", ingresos.Monto, noRecibo: GetInitData.GetMonthMovNumerator());
            await PagosHelper.NewIngreso(ingresos);
            RefreshGrid();
        }

        private async void ClientsPayment() {
            var horaNuevaRegistro = DateTime.Now;
            
            if (Cliente == null) return;
            ingresos.Tipo = 1;
            ingresos.Monto = Total;
            ingresos.IdPaquete = paquete.IdPaquete;
            ingresos.IdCliente = cliente.Id;
            ingresos.Concepto = Concepto;
            var time = horaNuevaRegistro - DoublePagoProtection.Item1;

            if (time.Seconds <= 30 && DoublePagoProtection.Item2 == Cliente.Id) {
                var res = ShowPrettyMessages.QuestionYesNo(
                    "Estas a punto de registrar dos pagos seguidos ¿deseas continuar?",
                    "Protección de pagos duplicados");
                
                if (!res) {
                    return;
                }
            }
            
            await PagosHelper.NewIngreso(ingresos, meses: NoMeses + 1);
            //tickets = new PrintTickets($"Pago {paquete.NombrePaquete}", ingresos.Monto, GetInitData.GetMonthMovNumerator(), ingresos.MontoRecibido, idCliente: cliente.Id);
            //tickets = new PrintTickets($"Pago {paquete.NombrePaquete}", ingresos.Monto, GetInitData.GetMonthMovNumerator(), ingresos.MontoRecibido, idCliente: cliente.Id);
            DoublePagoProtection.Item1 = horaNuevaRegistro;
            DoublePagoProtection.Item2 = Cliente.Id;
            
            ClearData();

            //tickets = new($"Pago {paquete.NombrePaquete}", ingresos.Monto);
            RefreshGrid();

        }

        private async void RefreshGrid() {
            try {
                var pagos = await GetFromDb.GetIngresos();
                var clientes = await GetFromDb.GetClientes();
                InitInfo.ObCoClientes.Clear();
                InitInfo.ObCoIngresos.Clear();
                foreach (var item in pagos.OrderByDescending(c => c.FechaRegistro)) {
                    InitInfo.ObCoIngresos.Add(item);
                }
                foreach (var item in clientes) {
                    InitInfo.ObCoClientes.Add(item);
                }
                ClearData();

            }
            catch (Exception e) {

                ShowPrettyMessages.ErrorOk(e.Message, "Error");
            }

        }

        private void ClearData() {
            Cliente = null;
            Paquete = null;
            Ingresos = null;
            Espacio = null;
            ClienteRenta = null;
            ClienteRenta = new();
            Espacio = new();
            Ingresos = new();
            Cliente = new();
            Paquete = new();
            Total = 0;
            Inscripcion = 0;
            NoMeses = 0;
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
