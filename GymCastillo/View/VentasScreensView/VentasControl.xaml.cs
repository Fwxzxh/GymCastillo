using GymCastillo.ViewModel.VentasVM;
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

namespace GymCastillo.View.VentasScreensView {
    /// <summary>
    /// Interaction logic for VentasControl.xaml
    /// </summary>
    public partial class VentasControl : UserControl {
        public VentasControl() {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            //RentasVM rentasVM = new();
            //InventarioVM inventarioVM = new();
            //rentasVM.Query = "";
            //inventarioVM.Query = "";
        }
    }
}
