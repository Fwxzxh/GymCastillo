using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.AsistenciasVM;
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

namespace GymCastillo.View.AsistenciasView {
    /// <summary>
    /// Interaction logic for AsistenciaInstructorWindow.xaml
    /// </summary>
    public partial class AsistenciaInstructorWindow : Window , IClosable {
        public AsistenciaInstructorWindow(Asistencia asistencia) {
            InitializeComponent();
            DataContext = new AsistenciaInstructorVM(asistencia);
        }
    }
}
