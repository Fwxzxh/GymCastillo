using GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsVM;
using GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM;
using System.Windows.Controls;

namespace GymCastillo.View.PersonalScreenView {
    /// <summary>
    /// Interaction logic for UsersViewUserControl.xaml
    /// </summary>
    public partial class UsersViewUserControl : UserControl {
        public UsersViewUserControl() {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            //GridRentaVM gridRenta = new();
            //GridClientesVM gridClientes = new();
            //GridInstructoresVM gridInstructores = new();
            //gridRenta.Query = "";
            //gridClientes.Query = "";
            //gridInstructores.Query = "";

        }
    }
}
