using System.Text.RegularExpressions;
using System.Windows;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.AdminScreensVM.UsersVM;

namespace GymCastillo.View.AdminScreensView.UsuariosView {
    /// <summary>
    /// Interaction logic for OverviewUserWindow.xaml
    /// </summary>
    public partial class OverviewUserWindow : Window, IClosable {
        public OverviewUserWindow(Usuario usuario) {
            InitializeComponent();
            DataContext = new OverviewUsuariosVM(usuario);
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
