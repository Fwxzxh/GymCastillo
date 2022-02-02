using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using ImageMagick;
using log4net;
using Microsoft.Win32;
using System;
using System.ComponentModel;

namespace GymCastillo.ViewModel.AdminScreensVM.PersonalVM {
    public class NewPersonalVM : INotifyPropertyChanged {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand ImageCommand { get; set; }

        private Personal personal = new() {
            FechaNacimiento = DateTime.Now
        };

        public Personal Personal {
            get { return personal; }
            set
            {
                personal = value;
                OnPropertyChanged(nameof(Personal));
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

        public NewPersonalVM() {
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            SaveCommand = new RelayCommand(GuardarPersonal);
            ImageCommand = new RelayCommand(SelectPhoto);
        }

        private void SelectPhoto() {
            OpenFileDialog dialog = new() {
                Filter = "Image files|*.png;*.jpg;*.jpeg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };
            if (dialog.ShowDialog() == true) {
                PhotoPath = dialog.FileName;
                var image = new MagickImage(PhotoPath);
                personal.Foto = image;
            }
        }

        private async void GuardarPersonal() {
            await AdminUsuariosGeneral.Alta(Personal);
            Personal = null;
            Personal = new() {
                FechaNacimiento = DateTime.Now
            };
            Log.Debug("Alta de personal nuevo realizada");
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
