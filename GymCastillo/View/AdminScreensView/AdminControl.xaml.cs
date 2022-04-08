using GymCastillo.ViewModel.AdminScreensVM.ClasesVM;
using GymCastillo.ViewModel.AdminScreensVM.EspaciosVM;
using GymCastillo.ViewModel.AdminScreensVM.PaquetesVM;
using GymCastillo.ViewModel.AdminScreensVM.PersonalVM;
using GymCastillo.ViewModel.AdminScreensVM.UsersVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GymCastillo.View.AdminScreensView {
    /// <summary>
    /// Interaction logic for AdminControl.xaml
    /// </summary>
    public partial class AdminControl : UserControl {
        public AdminControl() {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            //ClasesSettingsVM clasesSettings = new();
            //MainSettingsVM espaciosSettings = new();
            //PaquetesSettingsVM paquetesSettings = new();
            //GridPersonalVM gridPersonal = new();
            //GridUsuariosVM gridUsuarios = new();
            //clasesSettings.Query = "";
            //espaciosSettings.Query = "";
            //paquetesSettings.Query = "";
            //gridPersonal.Query = "";
            //gridUsuarios.Query = "";
        }
    }
}
