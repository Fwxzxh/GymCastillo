using System.Text.RegularExpressions;
using System.Windows;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM;

namespace GymCastillo.View.PersonalScreenView.InstructoresView {
    /// <summary>
    /// Interaction logic for OverviewInstructorWindow.xaml
    /// </summary>
    public partial class OverviewInstructorWindow : Window, IClosable {
        public OverviewInstructorWindow(Instructor instructor ) {
            InitializeComponent();
            DataContext = new OverviewInstructorVM(instructor);
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
