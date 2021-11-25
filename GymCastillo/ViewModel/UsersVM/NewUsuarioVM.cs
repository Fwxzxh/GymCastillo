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
    public class NewUsuarioVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }


        private Usuario usuario;

        public Usuario Usuario {
            get { return usuario; }
            set { usuario = value; }
        }

        public NewUsuarioVM() {
            try {
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);

            }
            catch (Exception) {

                throw;
            }
        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }

    }
}
