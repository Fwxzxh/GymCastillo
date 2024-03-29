﻿using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Bot;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using log4net;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.SettingsScreensVM {
    public class BotMessagesVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand SendMessage { get; set; }
        public RelayCommand SendPacketMessage { get; set; }
        public RelayCommand SendBroadcastMessage { get; set; }
        public RelayCommand SendAreaMessage { get; set; }
        public RelayCommand AddFile { get; set; }

        private Cliente cliente;

        public Cliente Cliente {
            get { return cliente; }
            set
            {
                cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }

        private Paquete paquete;

        public Paquete Paquete {
            get { return paquete; }
            set
            {
                paquete = value;
                OnPropertyChanged(nameof(Paquete));
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

        private Espacio espacio;

        public Espacio Espacio {
            get { return espacio; }
            set
            {
                espacio = value;
                OnPropertyChanged(nameof(Espacio));
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

        private string pMessage;

        public string PMessage {
            get { return pMessage; }
            set
            {
                pMessage = value;
                OnPropertyChanged(nameof(PMessage));
            }
        }

        private string bMessage;

        public string BMessage {
            get { return bMessage; }
            set
            {
                bMessage = value;
                OnPropertyChanged(nameof(BMessage));
            }
        }

        private string eMessage;

        public string EMessage {
            get { return eMessage; }
            set
            {
                eMessage = value;
                OnPropertyChanged(nameof(EMessage));
            }
        }

        private string fileName;

        public string FileName {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        private string safeFileName;

        public string SafeFileName {
            get { return safeFileName; }
            set
            {
                safeFileName = value;
                OnPropertyChanged(nameof(SafeFileName));
            }
        }

        public BotMessagesVM() {
            Clientes = new ObservableCollection<Cliente>();
            SendMessage = new RelayCommand(MandarMensaje);
            SendPacketMessage = new RelayCommand(PaqueteMensaje);
            SendBroadcastMessage = new RelayCommand(BroadcastMessage);
            SendAreaMessage = new RelayCommand(AreaMessageAsync);
            AddFile = new RelayCommand(AgregarArchivo);
            RefreshData();
        }

        private async void AreaMessageAsync() {
            if (Espacio != null) {
                await Bot.SendMessage(EMessage, espacio.IdEspacio);
                EMessage = "";
                Espacio = null;
                Espacio = new();
            }
            else {
                ShowPrettyMessages.ErrorOk("Por favor selecciona a un espacio", "Error");
            }
        }

        private void AgregarArchivo() {
            OpenFileDialog dialog = new() {
                Filter = "Image files|*.png;*.jpg;*.jpeg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (dialog.ShowDialog() == true) {
                FileName = dialog.FileName;
                SafeFileName = dialog.SafeFileName;
            }
        }

        private async void BroadcastMessage() {
            if (string.IsNullOrEmpty(FileName)) {
                await Bot.SendMassiveMessage(bMessage);
                BMessage = "";
                FileName = "";
                SafeFileName = "";
            }
            else {
                await Bot.SendMassiveMessageMultimedia(FileName, BMessage);
                BMessage = "";
                FileName = "";
                SafeFileName = "";
            }
        }

        private async void PaqueteMensaje() {
            if (Paquete != null) {
                await Bot.SendMassiveMessage(PMessage, Paquete.IdPaquete);
                PMessage = "";
                Paquete = null;
                Paquete = new();
            }
            else {
                ShowPrettyMessages.ErrorOk("Por favor selecciona un paquete", "Error");
            }

        }

        private async void MandarMensaje() {
            if (Cliente != null) {
                await Bot.SendMessage(message, cliente.Id);
                Message = "";
                Cliente = null;
                Cliente = new();
            }
            else {
                ShowPrettyMessages.ErrorOk("Por favor selecciona a un cliente", "Error");
            }

        }

        private async void RefreshData() {
            var lista = await GetFromDb.GetClientes();
            foreach (var cliente in lista.Where(c => c.ChatId != null || c.ChatId != "")) {
                clientes.Add(cliente);
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
