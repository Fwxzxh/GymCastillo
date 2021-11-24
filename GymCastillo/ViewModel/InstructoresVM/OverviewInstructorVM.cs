using GalaSoft.MvvmLight.CommandWpf;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.Commands.InstructorsCommands;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.InstructoresVM {
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
                TiposInstructor = new ObservableCollection<Tipo>(InitInfo.ListaTipoInstructor);
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
