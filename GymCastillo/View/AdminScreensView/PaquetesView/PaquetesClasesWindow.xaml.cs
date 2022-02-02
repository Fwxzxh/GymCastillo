using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.ViewModel.AdminScreensVM.PaquetesVM;
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
using System.Windows.Shapes;

namespace GymCastillo.View.AdminScreensView.PaquetesView {
    /// <summary>
    /// Interaction logic for PaquetesClasesWindow.xaml
    /// </summary>
    public partial class PaquetesClasesWindow : Window {
        public PaquetesClasesWindow(Paquete paquete) {
            InitializeComponent();
            DataContext = new ClasesPaqueteVM(paquete);
        }
    }
}
