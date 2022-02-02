using GymCastillo.Model.Helpers;
using GymCastillo.ViewModel.AsistenciasVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GymCastillo.View {
    /// <summary>
    /// Interaction logic for HomeViewUserControl.xaml
    /// </summary>
    public partial class HomeViewUserControl : UserControl {
        private HomeAsistenciasVM vm;
        private string barcode;
        public HomeViewUserControl() {
            InitializeComponent();
            vm = new HomeAsistenciasVM();

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void id_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                try {
                    if (id.Text.StartsWith("1")) {
                        vm.Asistencia.Id = int.Parse(id.Text.Substring(1));
                        vm.AsistenciaClienteView();
                        id.Text = "0";
                    }
                }
                catch (Exception ) {
                    ShowPrettyMessages.WarningOk("Input incorrecto, ingresa el ID y luego selecciona si es Cliente o Instructor", "Error");
                }

            }
        }

        private void UserControl_TextInput(object sender, TextCompositionEventArgs e) {
            barcode += e.Text;
            if (barcode.Length == 13) {
                //Asisntencias(barcode);
            }
        }

        private void Asisntencias(string barcode) {
            barcode.Trim('\r');
            if (id.Text.StartsWith("1")) {
                vm.Asistencia.Id = int.Parse(barcode.Substring(1));
                vm.AsistenciaClienteView();
                id.Text = "";
            }
        }
    }
}
