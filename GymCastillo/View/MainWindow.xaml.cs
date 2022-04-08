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
using GymCastillo.Model.Init;
using GymCastillo.ViewModel.AdminScreensVM.ClasesVM;
using GymCastillo.ViewModel.AdminScreensVM.EspaciosVM;
using GymCastillo.ViewModel.AdminScreensVM.PaquetesVM;
using GymCastillo.ViewModel.AdminScreensVM.PersonalVM;
using GymCastillo.ViewModel.AdminScreensVM.UsersVM;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsVM;
using GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM;
using GymCastillo.ViewModel.VentasVM;

namespace GymCastillo {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RentasVM rentasVM = new();
            InventarioVM inventarioVM = new();
            rentasVM.Query = "";
            inventarioVM.Query = "";

            ClasesSettingsVM clasesSettings = new();
            MainSettingsVM espaciosSettings = new();
            PaquetesSettingsVM paquetesSettings = new();
            GridPersonalVM gridPersonal = new();
            GridUsuariosVM gridUsuarios = new();
            clasesSettings.Query = "";
            espaciosSettings.Query = "";
            paquetesSettings.Query = "";
            gridPersonal.Query = "";
            gridUsuarios.Query = "";

            //GridRentaVM gridRenta = new();
            //GridClientesVM gridClientes = new();
            //GridInstructoresVM gridInstructores = new();
            //gridRenta.Query = "";
            //gridClientes.Query = "";
            //gridInstructores.Query = "";
        }
    }
}