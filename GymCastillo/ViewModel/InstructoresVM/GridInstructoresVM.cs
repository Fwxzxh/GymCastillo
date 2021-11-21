using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.View.InstructoresView;
using GymCastillo.ViewModel.Commands.InstructorsCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.InstructoresVM {
    public class GridInstructoresVM : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        public NewWindowInstructorCommand newWindow { get; set; }

        //private List<Instructor> instructores = Model.Database.GetFromDb.GetInstructores();

        //public List<Instructor> Instructores {
        //    get { return instructores; }
        //    set { instructores = value; }
        //}


        public GridInstructoresVM() {
            newWindow = new(this);

        }

        public void OpenWindow() {
            NewInstructorWindow  window = new NewInstructorWindow();
            window.ShowDialog();
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
