using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Interfaces;
using ImageMagick;
using log4net;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.AdminScreensVM.PersonalVM {
    public class OverviewPersonalVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }
        public RelayCommand SaveCommand { get; set; }

        public RelayCommand ImageCommand { get; set; }

        private Personal personal;

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

        public OverviewPersonalVM(Personal personal) {
            this.personal = personal;
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            ImageCommand = new RelayCommand(SelectPhoto);
            SaveCommand = new RelayCommand(GuardarPersonal);
        }

        private async void GuardarPersonal() {
            await AdminUsuariosGeneral.Update(Personal);
            Log.Debug("Personal Actualizado");
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
