using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.InstructorsCommands;
using log4net;

namespace GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM {
    public class OverviewInstructorVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Tipo> TiposInstructor { get; set; }

        public UpdateInstructorCommand updateInstructor { get; set; }

        private Instructor instructor;

        public Instructor Instructor {
            get { return instructor; }
            set
            {
                instructor = value;
                OnPropertyChanged(nameof(Instructor));
            }
        }

        public OverviewInstructorVM(Instructor instructor) {
            try {
                this.instructor = instructor;
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                updateInstructor = new(this);
                TiposInstructor = new ObservableCollection<Tipo>(InitInfo.ObCoTipoInstructor);
            }
            catch (Exception e) {
                Log.Error(e.Message);
                ShowPrettyMessages.ErrorOk(e.Message, "Error");
            }

        }

        public void UpdateInstructor() {
            Task.Run(()=>AdminUsuariosGeneral.Update(Instructor));
            Log.Debug("Instructor actualizado.");
        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
