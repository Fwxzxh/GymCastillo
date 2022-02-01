using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GymCastillo.View.PersonalScreenView.ClientsView {
    /// <summary>
    /// Interaction logic for HorariosClienteWindow.xaml
    /// </summary>
    public partial class HorariosClienteWindow : Window {
        public HorariosClienteWindow(Cliente cliente) {
            InitializeComponent();
            this.DataContext = new ClientesHorarioVM(cliente);

        }
    }
}
