using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.InstructoresVM;
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

namespace GymCastillo.View.InstructoresView {
    /// <summary>
    /// Interaction logic for OverviewInstructorWindow.xaml
    /// </summary>
    public partial class OverviewInstructorWindow : Window, IClosable {
        public OverviewInstructorWindow(Instructor instructor ) {
            InitializeComponent();
            DataContext = new OverviewInstructorVM(instructor);
        }
    }
}
