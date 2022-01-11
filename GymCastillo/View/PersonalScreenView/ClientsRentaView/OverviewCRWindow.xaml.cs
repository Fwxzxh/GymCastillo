using System.Text.RegularExpressions;
using System.Windows;
using GymCastillo.Model.DataTypes.IntersectionTables;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM;

namespace GymCastillo.View.PersonalScreenView.ClientsRentaView {
    /// <summary>
    /// Interaction logic for OverviewCRWindow.xaml
    /// </summary>
    public partial class OverviewCRWindow : Window, IClosable{
        public OverviewCRWindow(ClienteRenta cliente) {
            InitializeComponent();
            DataContext = new OverviewRentaVM(cliente);
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
