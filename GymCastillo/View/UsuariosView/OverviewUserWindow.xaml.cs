using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.UsersVM;
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

namespace GymCastillo.View.UsuariosView {
    /// <summary>
    /// Interaction logic for OverviewUserWindow.xaml
    /// </summary>
    public partial class OverviewUserWindow : Window, IClosable {
        public OverviewUserWindow(Usuario usuario) {
            InitializeComponent();
            DataContext = new OverviewUsuariosVM(usuario);
        }
    }
}
