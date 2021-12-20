using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.AdminScreensVM.PersonalVM {
    public class NewPersonalVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }
        public RelayCommand SaveCommand { get; set; }

        private Personal personal;

        public Personal Personal {
            get { return personal; }
            set
            {
                personal = value;
                OnPropertyChanged(nameof(Personal));
            }
        }

        public NewPersonalVM() {
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            SaveCommand = new RelayCommand(GuardarPersonal);

        }

        private async void GuardarPersonal() {
            await AdminUsuariosGeneral.Alta(Personal);
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
