using GymCastillo.Model.Interfaces;
using System.Windows;

namespace GymCastillo.View.ClientsView {
    /// <summary>
    /// Interaction logic for NewClientsWindow.xaml
    /// </summary>
    public partial class NewClientsWindow : Window, IClosable {
        public NewClientsWindow() {
            InitializeComponent();
        }
    }
}
