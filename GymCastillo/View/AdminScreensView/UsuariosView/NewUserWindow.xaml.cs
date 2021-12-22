using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using GymCastillo.Model.Interfaces;

namespace GymCastillo.View.AdminScreensView.UsuariosView {
    /// <summary>
    /// Interaction logic for NewUserWindow.xaml
    /// </summary>
    public partial class NewUserWindow : Window, IClosable {
        public NewUserWindow() {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
