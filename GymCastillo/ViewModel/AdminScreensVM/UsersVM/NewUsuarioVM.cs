﻿using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.UsersCommands;
using log4net;

namespace GymCastillo.ViewModel.AdminScreensVM.UsersVM {
    public class NewUsuarioVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public NewUserCommand newUser { get; set; }


        private Usuario usuario = new() { FechaNacimiento = DateTime.Now };

        public Usuario Usuario {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        public NewUsuarioVM() {
            try {
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                newUser = new(this);

            }
            catch (Exception) {

                throw;
            }
        }

        public async void NewUser() {
            await AdminUsuariosGeneral.Alta(Usuario);
            Log.Debug("Nuevo usuario creado");
            Usuario = new() { FechaNacimiento = DateTime.Now};
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
