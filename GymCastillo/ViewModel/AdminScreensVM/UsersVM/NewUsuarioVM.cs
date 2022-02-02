using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.UsersCommands;
using ImageMagick;
using log4net;
using Microsoft.Win32;

namespace GymCastillo.ViewModel.AdminScreensVM.UsersVM {
    public class NewUsuarioVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public NewUserCommand newUser { get; set; }
        public RelayCommand ImageCommand { get; set; }

        private Usuario usuario = new() { FechaNacimiento = DateTime.Now };

        public Usuario Usuario {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged(nameof(Usuario));
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


        public NewUsuarioVM() {
            try {
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                ImageCommand = new RelayCommand(SelectPhoto);
                newUser = new(this);

            }
            catch (Exception) {

                throw;
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
                usuario.Foto = image;
            }
        }

        public async void NewUser() {
            await AdminUsuariosGeneral.Alta(Usuario);
            Log.Debug("Nuevo usuario creado");
            Usuario = new() { FechaNacimiento = DateTime.Now };
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
