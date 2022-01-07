﻿using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Bot;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.SettingsScreensVM {
    public class BotSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public event PropertyChangedEventHandler PropertyChanged;

        public Bot Bot;
        public RelayCommand StartBot { get; set; }
        public RelayCommand StopBot { get; set; }
        public RelayCommand SendMessage { get; set; }
        public RelayCommand GenPassword { get; set; }
        public RelayCommand SaveKey { get; set; }


        private string password;

        public string Password {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }


        private string message;

        public string Message {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private Cliente cliente;

        public Cliente Cliente {
            get { return cliente; }
            set
            {
                cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }


        private ObservableCollection<Cliente> clientes;

        public ObservableCollection<Cliente> Clientes {
            get { return clientes; }
            set
            {
                clientes = value;
                OnPropertyChanged(nameof(Clientes));
            }
        }

        private string apiKey;

        public string ApiKey {
            get { return apiKey; }
            set
            {
                apiKey = value;
                OnPropertyChanged(nameof(ApiKey));
            }
        }

        public BotSettingsVM() {
            Clientes = new ObservableCollection<Cliente>();
            StartBot = new RelayCommand(IniciarBot);
            StopBot = new RelayCommand(DetenerBot);
            SendMessage = new RelayCommand(MandarMensaje);
            GenPassword = new RelayCommand(GenerarPassword);
            SaveKey = new RelayCommand(GuardarKey);

            RefreshData();
        }

        private void GenerarPassword() {
            Password = Bot.GenPassword();
        }

        private async void MandarMensaje() {
            await Bot.SendMessage(message, cliente.Id);
            Message = "";
        }

        private void DetenerBot() {
            Bot = new("5031509807:AAEqBUEnXaARUzeFAWjd-Tk_FQt220LyEfM");
            Bot.StopBot();
        }

        private void IniciarBot() {
            Bot = new("5031509807:AAEqBUEnXaARUzeFAWjd-Tk_FQt220LyEfM");
        }
        private void GuardarKey() {
            if (!string.IsNullOrWhiteSpace(ApiKey)) {
                GetInitData.WriteApikey(ApiKey);
            }
            else {
                ShowPrettyMessages.ErrorOk("La key no debe de estar vacía", "Error");
            }
        }

        private async void RefreshData() {
            var lista = await GetFromDb.GetClientes();
            //foreach (var cliente in lista.Where(c => c.ChatId != null || c.ChatId != "")) {
            //    clientes.Add(cliente);
            //}
            foreach (var cliente in lista) {
                Clientes.Add(cliente);
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
