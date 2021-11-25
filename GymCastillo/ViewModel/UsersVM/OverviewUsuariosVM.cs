using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.UsersVM {
    public class OverviewUsuariosVM : INotifyPropertyChanged {

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }


        private Usuario usuario;

        public Usuario Usuario {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        public OverviewUsuariosVM(Usuario usuario) {
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);

            this.usuario = usuario;
        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
