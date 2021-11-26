﻿using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.Commands.UsersCommands;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.UsersVM {
    public class OverviewUsuariosVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public UpdateUserCommand updateUser { get; set; }


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
            try {
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                updateUser = new(this);
                this.usuario = usuario;
            }
            catch (Exception e) {

                Log.Error(e.Message);
            }
        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }

        public async void UpdateUser() {
            await AdminUsuariosGeneral.Update(Usuario);
            Log.Debug("Usuario actualizado");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
