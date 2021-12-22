using System.Text.RegularExpressions;
using System.Windows;
using GymCastillo.Model.Interfaces;

namespace GymCastillo.View.PersonalScreenView.ClientsView {
    /// <summary>
    /// Interaction logic for NewClientsWindow.xaml
    /// </summary>
    public partial class NewClientsWindow : Window, IClosable {
        public NewClientsWindow() {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
