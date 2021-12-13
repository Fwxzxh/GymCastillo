using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.ViewModel.SettingsScreensVM.ClasesVM;
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

namespace GymCastillo.View.AdminScreensView.ClasesView {
    /// <summary>
    /// Interaction logic for HorariosWindow.xaml
    /// </summary>
    public partial class HorariosWindow : Window {
        public HorariosWindow(Clase clase) {
            InitializeComponent();
            DataContext = new HorariosSettingsVM(clase);
        }
    }
}
