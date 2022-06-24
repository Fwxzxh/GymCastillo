using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using iText.Layout.Element;
using iText.IO.Image;
using System.IO;
using iText.Layout;
using log4net;

namespace GymCastillo.ViewModel.ClasesVM {
    public class InscritosHorarioVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand ListaInscritos { get; set; }

        private string path = @"C:\GymCastillo\Reportes\ListasAsistenciaPorHorarios";
        private Horario horario;

        public Horario Horario {
            get { return horario; }
            set
            {
                horario = value;
                OnPropertyChanged(nameof(Horario));
            }
        }

        private string nombreClase;

        public string NombreClase {
            get { return nombreClase; }
            set
            {
                nombreClase = value;
                OnPropertyChanged(nameof(NombreClase));
            }
        }

        private int diaClase;

        public int DiaClase {
            get { return diaClase; }
            set
            {
                diaClase = value;
                OnPropertyChanged(nameof(DiaClase));
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


        public InscritosHorarioVM(Horario horario) {
            Directory.CreateDirectory(path);
            this.horario = horario;
            NombreClase = InitInfo.ObCoClases.Where(x => x.IdClase == horario.IdClase).First().NombreClase;
            DiaClase = horario.Dia;
            ListaClientes = new ObservableCollection<Cliente>();
            var listTemp = ListaAlumnosHelper.GetClientesDeHorario(horario.IdHorario);
            foreach (var item in listTemp) {
                ListaClientes.Add(item);
            }
            ListaInscritos = new RelayCommand(MakePDF);
        }

        private void MakePDF() {
            var listaClientes = ListaAlumnosHelper.GetClientesDeHorario(horario.IdHorario).Where(x => x.Activo == true).OrderBy(n => n.ApellidoPaterno);
            if (listaClientes.Count() == 0) {
                ShowPrettyMessages.WarningOk("No hay alumnos en esta clase", "Advertencia");
                return;
            }
            Document document;
            var fontSize = 12;
            string[] columnas = { "Nombre", "Último Pago", "Vencimiento de Pago" };
            float[] tamaños = { 1, 1, 1 };
            var HoraInicio = string.Format("{0:t}", horario.HoraInicio);
            var HoraFin = string.Format("{0:t}", horario.HoraFin);
            var nombreDia = "";
            switch (DiaClase) {
                case 1:
                    nombreDia = "Lunes";
                    break;
                case 2:
                    nombreDia = "Martes";
                    break;
                case 3:
                    nombreDia = "Miercoles";
                    break;
                case 4:
                    nombreDia = "Jueves";
                    break;
                case 5:
                    nombreDia = "Viernes";
                    break;
                case 6:
                    nombreDia = "Sabado";
                    break;
                default:
                    nombreDia = "";
                    break;
            }
            try {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(@$"{path}\{NombreClase.ToUpper()}-{nombreDia}-{horario.HoraInicio.Hour}hrs-{horario.HoraFin.Hour}hrs.pdf", FileMode.Create, FileAccess.Write)));
                document = new Document(pdfDocument, PageSize.A4);
                document.SetMargins(20, 20, 20, 20);
                ImageData imageData = ImageDataFactory.Create(@"C:\GymCastillo\Assets\logo.jpg");
                Image image = new Image(imageData).ScaleAbsolute(200, 100);
                image.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(image);
                var titulo = $"{NombreClase.ToUpper()} de {HoraInicio} a {HoraFin}";
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

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
