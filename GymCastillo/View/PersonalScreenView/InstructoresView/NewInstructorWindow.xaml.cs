using System.Text.RegularExpressions;
using System.Windows;
using GymCastillo.Model.Interfaces;

namespace GymCastillo.View.PersonalScreenView.InstructoresView {
    /// <summary>
    /// Interaction logic for NewInstructorWindow.xaml
    /// </summary>
    public partial class NewInstructorWindow : Window, IClosable {
        public NewInstructorWindow() {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
