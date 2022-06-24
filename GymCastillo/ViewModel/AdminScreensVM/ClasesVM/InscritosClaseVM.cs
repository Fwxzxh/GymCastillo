using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using iText.Layout;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using iText.Layout.Element;
using iText.IO.Image;

namespace GymCastillo.ViewModel.AdminScreensVM.ClasesVM {
    public class InscritosClaseVM {

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand ListaInscritos { get; set; }

        private string path = @"C:\GymCastillo\Reportes\ListasAsistencia";


        private Clase clase;

        public Clase Clase {
            get { return clase; }
            set
            {
                clase = value;
                OnPropertyChanged(nameof(Clase));
            }
        }

        private ObservableCollection<Cliente> listaClientes;

        public ObservableCollection<Cliente> ListaClientes {
            get { return listaClientes; }
            set
            {
                listaClientes = value;
                OnPropertyChanged(nameof(ListaClientes));
            }
        }


        public InscritosClaseVM(Clase clase) {
            Directory.CreateDirectory(path);
            this.clase = clase;
            ListaClientes = new ObservableCollection<Cliente>();
            var lista = ListaAlumnosHelper.GetClientesDeClase(clase.IdClase).Where(x => x.Activo == true);
            foreach (var item in lista) {
                ListaClientes.Add(item);
            }
            ListaInscritos = new RelayCommand(PdfInscritos);
        }

        private void PdfInscritos() {
            var listaClientes = ListaAlumnosHelper.GetClientesDeClase(clase.IdClase).Where(x => x.Activo == true).OrderBy(n => n.ApellidoPaterno);
            if (ListaClientes.Count() == 0) {
                ShowPrettyMessages.WarningOk("No hay alumnos en esta clase", "Advertencia");
                return;
            }
            Document document;
            var fontSize = 12;
            string[] columnas = { "Nombre", "Último Pago", "Vencimiento de Pago" };
            float[] tamaños = { 1, 1, 1 };

            try {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(@$"{path}\{Clase.NombreClase.ToUpper()}-{DateTime.Now.Day}-{DateTime.Now.Month}.pdf", FileMode.Create, FileAccess.Write)));
                document = new Document(pdfDocument, PageSize.A4);
                document.SetMargins(20, 20, 20, 20);
                ImageData imageData = ImageDataFactory.Create(@"C:\GymCastillo\Assets\logo.jpg");
                Image image = new Image(imageData).ScaleAbsolute(200, 100);
                image.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(image);
                var titulo = $"Lista Inscritos en {Clase.NombreClase.ToUpper()}";
                document.Add(new Paragraph(titulo).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                Table table = new Table(UnitValue.CreatePercentArray(tamaños));
                table.SetWidth(UnitValue.CreatePercentValue(100));
                foreach (string columa in columnas) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(columa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize)));
                }
                foreach (var item in listaClientes) {
                    table.AddCell(new Cell().Add(new Paragraph($"{item.ApellidoPaterno} {item.ApellidoMaterno} {item.Nombre}").SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(item.FechaUltimoPago.ToShortDateString())).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER));
                    table.AddCell(new Cell().Add(new Paragraph(item.FechaVencimientoPago.ToShortDateString())).SetFontSize(fontSize).SetTextAlignment(TextAlignment.CENTER));
                }
                document.Add(table);
                document.Close();
                ShowPrettyMessages.InfoOk($"Documento creado en la ruta {path}", "Reporte Generado");
            }
            catch (Exception e) {
                ShowPrettyMessages.ErrorOk("Error al crear documento", "Error");
                ShowPrettyMessages.ErrorOk(e.Message.ToString(), "Error");
                Log.Debug("Error al crear documento pdf");
                Log.Error(e.Message);
            }

        }
    }
}
