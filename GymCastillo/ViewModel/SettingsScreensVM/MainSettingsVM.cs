using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Notificaciones;
using GymCastillo.Model.Reportes;
using iText.Layout;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using iText.Layout.Element;
using iText.IO.Image;
using System.IO;

namespace GymCastillo.ViewModel.SettingsScreensVM {
    public class MainSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public event PropertyChangedEventHandler PropertyChanged;
        private string path = @"C:\GymCastillo\Reportes\ReportesDiarios";

        public RelayCommand ManualCommand { get; set; }
        public RelayCommand SaveKey { get; set; }
        public RelayCommand SavePrecios { get; set; }
        public RelayCommand MakeReporte { get; set; }

        private string apiKey;

        public string ApiKey {
            get { return apiKey; }
            set
            {
                apiKey = value;
                OnPropertyChanged(nameof(ApiKey));
            }
        }

        private decimal visitaGym;

        public decimal VisitaGym {
            get { return visitaGym; }
            set
            {
                visitaGym = value;
                OnPropertyChanged(nameof(VisitaGym));
            }
        }

        private decimal visitaBox;

        public decimal VisitaBox {
            get { return visitaBox; }
            set
            {
                visitaBox = value;
                OnPropertyChanged(nameof(VisitaBox));
            }
        }

        private decimal visitaAlberca;

        public decimal VisitaAlberca {
            get { return visitaAlberca; }
            set
            {
                visitaAlberca = value;
                OnPropertyChanged(nameof(VisitaAlberca));
            }
        }


        public MainSettingsVM() {
            ApiKey = GetInitData.GetApiKey();
            ManualCommand = new(Actualizar);
            SaveKey = new RelayCommand(GuardarKey);
            SavePrecios = new RelayCommand(ActualizarPrecios);
            MakeReporte = new RelayCommand(ReporteDiario);
            VisitaGym = GetInitData.VisitaGym;
            VisitaBox = GetInitData.VisitaBox;
            VisitaAlberca = GetInitData.VisitaAlberca;
            Directory.CreateDirectory(path);
        }

        private async void ReporteDiario() {
            var ingrsosHoy = await GetReportes.GetIngresosToday();
            var egresosHoy = await GetReportes.GetEgresosToday();
            Document document;
            var fontSize = 15;
            string[] columnas = { "Concepto", "Monto Total", "Monto Recibido" };
            float[] tamaños = { 1, 1, 1 };

            try {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(@$"{path}\Reporte-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}.pdf", FileMode.Create, FileAccess.Write)));
                document = new Document(pdfDocument, PageSize.A4);
                document.SetMargins(20, 20, 20, 20);
                ImageData imageData = ImageDataFactory.Create(@"C:\GymCastillo\Assets\logo.jpg");
                Image image = new Image(imageData).ScaleAbsolute(200, 100);
                image.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(image);
                var titulo = $"Ingresos del dia {DateTime.Now.Date.ToShortDateString()}";
                var montoTotalRecibido = 0.0m;
                var montoTotalTipo = 0.0m;

                document.Add(new Paragraph(titulo).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetBold());
                Table table = new Table(UnitValue.CreatePercentArray(tamaños));

                document.Add(new Paragraph("Clientes").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in ingrsosHoy.Where(l => l.IdCliente != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.MontoRecibido;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total recibido: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalTipo = 0;
                document.Add(new Paragraph("Ventas").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in ingrsosHoy.Where(l => l.IdVenta != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.MontoRecibido;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total recibido: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalTipo = 0;
                document.Add(new Paragraph("Rentas").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in ingrsosHoy.Where(l => l.IdRenta != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.MontoRecibido;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total recibido: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalTipo = 0;

                document.Add(new Paragraph("Otros").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in ingrsosHoy.Where(l => l.Otros != false)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.MontoRecibido;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total recibido: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalTipo = 0;


                var titulo1 = $"Egresos del dia {DateTime.Now.Date.ToShortDateString()}";

                document.Add(new Paragraph(titulo1).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetBold());

                document.Add(new Paragraph("Usuarios").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in egresosHoy.Where(l => l.IdUsuarioPagar != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalTipo = 0;

                document.Add(new Paragraph("Instructores").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in egresosHoy.Where(l => l.IdInstructor != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalTipo = 0;

                document.Add(new Paragraph("Personal").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in egresosHoy.Where(l => l.IdPersonal != 0)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalTipo = 0;

                document.Add(new Paragraph("Servicios").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in egresosHoy.Where(l => l.Servicios == true)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                montoTotalTipo = 0;

                document.Add(new Paragraph("Otros").SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in egresosHoy.Where(l => l.Otros == true)) {
                    table.AddCell(new Cell().Add(new Paragraph(item.Concepto).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(string.Format("{0:C}", item.Monto)).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    montoTotalTipo += item.Monto;
                }
                montoTotalTipo = 0;
                document.Add(table);
                document.Add(new Paragraph((string.Format("Monto total: {0:C}", montoTotalTipo))).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(fontSize).SetBold());
                table = new Table(UnitValue.CreatePercentArray(tamaños));
                var ingresos = 0m;
                var egresos = 0m;
                foreach (var item in ingrsosHoy) {
                    montoTotalRecibido += item.MontoRecibido;
                }

                document.Add(new Paragraph((string.Format("Monto total ingresos del día {1}: {0:C}", montoTotalRecibido, DateTime.Now.Date.ToShortDateString()))).SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());
                ingresos = montoTotalRecibido;
                montoTotalRecibido = 0;

                foreach (var item in egresosHoy) {
                    montoTotalRecibido += item.Monto;
                }
                egresos = montoTotalRecibido;
                document.Add(new Paragraph((string.Format("Monto total egresos del día {1}: {0:C}", montoTotalRecibido, DateTime.Now.Date.ToShortDateString()))).SetTextAlignment(TextAlignment.LEFT).SetFontSize(fontSize).SetBold());

                var resultado = ingresos - egresos;
                document.Add(new Paragraph((string.Format("Monto total de ingresos menos egresos del día {1}: {0:C}", resultado, DateTime.Now.Date.ToShortDateString()))).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize).SetBold());
                document.Close();
                ShowPrettyMessages.InfoOk($"Documento creado en la ruta {path}", "Reporte Diario Generado");

            }
            catch (Exception e) {
                Log.Debug("Error al crear documento pdf");
                Log.Error(e.Message);
            }
        }

        private void ActualizarPrecios() {
            GetInitData.VisitaGym = VisitaGym;
            GetInitData.VisitaBox = VisitaBox;
            GetInitData.VisitaAlberca = VisitaAlberca;
            GetInitData.SavePreciosVisitas();
        }

        private void GuardarKey() {
            if (!string.IsNullOrWhiteSpace(ApiKey)) {
                GetInitData.WriteApikey(ApiKey);
            }
            else {
                ShowPrettyMessages.ErrorOk("La key no debe de estar vacía", "Error");
            }
        }

        private async void Actualizar() {
            await Notificaciones.ResetFieldsAndUpdate();
            Notificaciones.FechaUltimoReset = DateTime.Now;
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
