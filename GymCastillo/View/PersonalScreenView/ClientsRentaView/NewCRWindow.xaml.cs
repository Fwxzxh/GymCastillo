using System.Text.RegularExpressions;
using System.Windows;
using GymCastillo.Model.Interfaces;

namespace GymCastillo.View.PersonalScreenView.ClientsRentaView {
    /// <summary>
    /// Interaction logic for NewCRWindow.xaml
    /// </summary>
    public partial class NewCRWindow : Window, IClosable {
        public NewCRWindow() {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
