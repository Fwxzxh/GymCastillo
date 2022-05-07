using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.ViewModel.ClasesVM;
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

namespace GymCastillo.View.ClsesScreensView {
    /// <summary>
    /// Interaction logic for InscritosHorarioWindow.xaml
    /// </summary>
    public partial class InscritosHorarioWindow : Window {
        public InscritosHorarioWindow(Horario horario) {
            InitializeComponent();
            DataContext = new InscritosHorarioVM(horario);
        }
    }
}
