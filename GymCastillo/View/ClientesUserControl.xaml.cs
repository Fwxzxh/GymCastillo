using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ClientesUserControl.xaml
    /// </summary>
    public partial class ClientesUserControl : UserControl {
        public ClientesUserControl() {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e) {
            DialogHost.Show("hola", sender);
        }
    }
}
