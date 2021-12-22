using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.InstructorsCommands;
using ImageMagick;
using log4net;
using Microsoft.Win32;

namespace GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM {
    public class NewInstructorVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public NewInstructorCommand instructorCommand { get; set; }
        public RelayCommand ImageCommand { get; set; }


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

        private string photoPath;

        public string PhotoPath {
            get { return photoPath; }
            set
            {
                photoPath = value;
                OnPropertyChanged(nameof(PhotoPath));
            }
        }

        public NewInstructorVM() {
            try {
                Log.Debug("Nuevo instructor ventana inicializada");
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                TiposInstructor = new ObservableCollection<Tipo>(InitInfo.ObCoTipoInstructor);
                instructorCommand = new(this);
                ImageCommand = new RelayCommand(SelectPhoto);
            }
            catch (Exception e) {
                Log.Error(e.Message);
                ShowPrettyMessages.ErrorOk(e.Message, "Error");
            }

        }

        private void SelectPhoto() {
            OpenFileDialog dialog = new() {
                Filter = "Image files|*.png;*.jpg;*.jpeg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };
            if (dialog.ShowDialog() == true) {
                PhotoPath = dialog.FileName;
                var image = new MagickImage(PhotoPath);
                instructor.Foto = image;
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
