using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM;
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

namespace GymCastillo.View.PersonalScreenView.InstructoresView {
    /// <summary>
    /// Interaction logic for InstructorClasesSummary.xaml
    /// </summary>
    public partial class InstructorClasesSummary : Window {
        public InstructorClasesSummary(Instructor instructor ) {
            InitializeComponent();
            DataContext = new InstructorClasesSummaryVM(instructor);
        }
    }
}
