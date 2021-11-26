using System.Windows;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.AdminScreensVM.ClientsRentaVM;

namespace GymCastillo.View.AdminScreensView.ClientsRentaView {
    /// <summary>
    /// Interaction logic for OverviewCRWindow.xaml
    /// </summary>
    public partial class OverviewCRWindow : Window, IClosable{
        public OverviewCRWindow(ClienteRenta cliente) {
            InitializeComponent();
            DataContext = new OverviewRentaVM(cliente);
        }
    }
}
