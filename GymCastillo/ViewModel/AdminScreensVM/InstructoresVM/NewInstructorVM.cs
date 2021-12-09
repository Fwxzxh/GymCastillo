using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.AdminScreensCommands.InstructorsCommands;
using log4net;

namespace GymCastillo.ViewModel.AdminScreensVM.InstructoresVM {
    public class NewInstructorVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public NewInstructorCommand instructorCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Tipo> TiposInstructor { get; set; }

        private Instructor instructor = new() {
            FechaNacimiento = DateTime.Now,
            FechaUltimoAcceso = DateTime.Now,
            FechaUltimoPago = DateTime.Now,
            HoraEntrada = DateTime.Now,
            HoraSalida = DateTime.Now
        };
        public Instructor Instructor {
            get { return instructor; }
            set
            {
                instructor = value;
                OnPropertyChanged(nameof(Instructor));
            }
        }

        public NewInstructorVM() {
            try {
                Log.Debug("Nuevo instructor ventana inicializada");
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                TiposInstructor = new ObservableCollection<Tipo>(InitInfo.ObCoTipoInstructor);
                instructorCommand = new(this);

            }
            catch (Exception e) {
                Log.Error(e.Message);
                ShowPrettyMessages.ErrorOk(e.Message, "Error");
            }

        }

        public async void CrearInstructor() {
            Log.Debug("Nuevo instructor creado");
            await AdminUsuariosGeneral.Alta(Instructor);
            Instructor = new() {
                FechaNacimiento = DateTime.Now,
                FechaUltimoAcceso = DateTime.Now,
                FechaUltimoPago = DateTime.Now,
                HoraEntrada = DateTime.Now,
                HoraSalida = DateTime.Now
            };
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
