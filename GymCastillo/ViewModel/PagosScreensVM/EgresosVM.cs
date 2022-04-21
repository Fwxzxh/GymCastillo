using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Reportes;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using log4net;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace GymCastillo.ViewModel.PagosScreensVM {
    public class EgresosVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        private string path = @"C:\GymCastillo\Reportes\Egresos";

        public RelayCommand PagoUsuario { get; set; }
        public RelayCommand PagoInstructor { get; set; }
        public RelayCommand PagoPersonal { get; set; }
        public RelayCommand PagoServicios { get; set; }
        public RelayCommand PagoOtros { get; set; }
        public RelayCommand MakeReporte { get; set; }

        private PrintTickets tickets;

        private Usuario usuario = new();

        public Usuario Usuario {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        private Instructor instructor = new();

        public Instructor Instructor {
            get { return instructor; }
            set
            {
                instructor = value;
                OnPropertyChanged(nameof(Instructor));
                if (instructor != null) {
                    MontoFinal = instructor.Sueldo - instructor.SueldoADescontar;
                }
            }
        }

        private Personal personal = new();

        public Personal Personal {
            get { return personal; }
            set
            {
                personal = value;
                OnPropertyChanged(nameof(Personal));
            }
        }

        private Egresos egresos = new();

        public Egresos Egresos {
            get { return egresos; }
            set
            {
                egresos = value;
                OnPropertyChanged(nameof(Egresos));
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
        private decimal montoFinal;

        public decimal MontoFinal {
            get { return montoFinal; }
            set
            {
                montoFinal = value;
                OnPropertyChanged(nameof(MontoFinal));
            }
        }

        public EgresosVM() {
            PagoUsuario = new RelayCommand(UserPayment);
            PagoInstructor = new RelayCommand(InstructorPayment);
            PagoPersonal = new RelayCommand(PersonalPayment);
            PagoServicios = new RelayCommand(ServicesPayment);
            PagoOtros = new RelayCommand(OthersPayment);
            MakeReporte = new RelayCommand(ReporteSemanal);
            Directory.CreateDirectory(path);
            RefreshGrid();
        }

        private async void ReporteSemanal() {
            var lista = await GetReportes.GetReporteEgresos();
            Document document;
            var fontSize = 15;
            string[] columnas = { "Concepto", "Monto Total" };
            float[] tamaños = { 1, 1};
            try {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(@$"{path}\Egresos-{DateTime.Now.Day}-{DateTime.Now.Month}.pdf", FileMode.Create, FileAccess.Write)));
                document = new Document(pdfDocument, PageSize.A4);
                document.SetMargins(20, 20, 20, 20);
                ImageData imageData = ImageDataFactory.Create(@"C:\GymCastillo\Assets\logo.jpg");
                Image image = new Image(imageData).ScaleAbsolute(200, 100);
                image.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(image);
                var titulo = "Egresos Últimos 7 días";
                var montoTotalRecibido = 0.0m;
                var montoTotalTipo = 0.0m;

                document.Add(new Paragraph(titulo).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                Table table = new Table(UnitValue.CreatePercentArray(tamaños));

                document.Add(new Paragraph("Usuarios").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in lista.Where(l => l.IdUsuarioPagar != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalRecibido += montoTotalTipo;
                montoTotalTipo = 0;

                document.Add(new Paragraph("Instructores").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in lista.Where(l => l.IdInstructor != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalRecibido += montoTotalTipo;
                montoTotalTipo = 0;

                document.Add(new Paragraph("Personal").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in lista.Where(l => l.IdPersonal != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalRecibido += montoTotalTipo;
                montoTotalTipo = 0;

                document.Add(new Paragraph("Servicios").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in lista.Where(l => l.Servicios == true)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalRecibido += montoTotalTipo;
                montoTotalTipo = 0;

                document.Add(new Paragraph("Otros").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in lista.Where(l => l.Otros == true)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalRecibido += montoTotalTipo;
                montoTotalTipo = 0;
                document.Add(new Paragraph((string.Format("Monto total últimos 7 días: {0:C}", montoTotalRecibido))).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetBold());
                document.Close();
                ShowPrettyMessages.InfoOk($"Documento creado en la ruta {path}", "Reporte Generado");
            }
            catch (Exception e) {
                Log.Debug("Error al crear documento pdf");
                Log.Error(e.Message);
            }
        }

        private async void OthersPayment() {
            egresos.Tipo = 5;
            tickets = new($"Pago otros", egresos.Monto, GetInitData.GetMonthMovNumerator());
            tickets = new($"Pago otros", egresos.Monto, GetInitData.GetMonthMovNumerator());

            await PagosHelper.NewEgreso(egresos);
            RefreshGrid();
        }

        private async void ServicesPayment() {
            egresos.Tipo = 4;
            tickets = new($"Pago servicios {egresos.Concepto}", egresos.Monto, GetInitData.GetMonthMovNumerator());
            tickets = new($"Pago servicios {egresos.Concepto}", egresos.Monto, GetInitData.GetMonthMovNumerator());
            await PagosHelper.NewEgreso(egresos);
            RefreshGrid();
        }

        private async void PersonalPayment() {
            egresos.Tipo = 3;
            egresos.IdPersonal = personal.Id;
            egresos.Monto = personal.Sueldo;
            tickets = new($"Nómina personal {personal.Nombre}", egresos.Monto, GetInitData.GetMonthMovNumerator());
            tickets = new($"Nómina personal {personal.Nombre}", egresos.Monto, GetInitData.GetMonthMovNumerator());
            await PagosHelper.NewEgreso(egresos);
            RefreshGrid();
        }

        private async void InstructorPayment() {
            egresos.Tipo = 2;
            egresos.IdInstructor = instructor.Id;
            egresos.Monto = MontoFinal;
            tickets = new($"Nómina instructor {instructor.Nombre}", egresos.Monto, GetInitData.GetMonthMovNumerator());
            tickets = new($"Nómina instructor {instructor.Nombre}", egresos.Monto, GetInitData.GetMonthMovNumerator());
            await PagosHelper.NewEgreso(egresos);
            RefreshGrid();
        }

        private async void UserPayment() {
            egresos.Tipo = 1;
            egresos.IdUsuarioPagar = usuario.Id;
            egresos.Monto = usuario.Sueldo;
            tickets = new($"Nómina usuario {usuario.Nombre}", egresos.Monto, GetInitData.GetMonthMovNumerator());
            tickets = new($"Nómina usuario {usuario.Nombre}", egresos.Monto, GetInitData.GetMonthMovNumerator());
            await PagosHelper.NewEgreso(egresos);
            RefreshGrid();
        }

        private async void RefreshGrid() {
            var pagos = await GetFromDb.GetEgresos();
            InitInfo.ObCoEgresos.Clear();
            foreach (var item in pagos.OrderByDescending(c => c.FechaRegistro)) {
                InitInfo.ObCoEgresos.Add(item);
            }
            ClearData();
        }

        private void ClearData() {
            Usuario = null;
            Usuario = new();
            Instructor = null;
            Instructor = new();
            Personal = null;
            Personal = new();
            Egresos = null;
            Egresos = new();
            MontoFinal = 0;
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
