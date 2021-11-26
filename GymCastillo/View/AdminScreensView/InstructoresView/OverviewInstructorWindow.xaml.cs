using System.Windows;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.AdminScreensVM.InstructoresVM;

namespace GymCastillo.View.AdminScreensView.InstructoresView {
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
