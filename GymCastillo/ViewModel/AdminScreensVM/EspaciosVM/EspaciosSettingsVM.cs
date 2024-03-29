﻿using System;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Init;
using log4net;

namespace GymCastillo.ViewModel.AdminScreensVM.EspaciosVM {
    public class MainSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        private Espacio espacio = new();

        public Espacio Espacio {
            get { return espacio; }
            set { espacio = value;
                OnPropertyChanged(nameof(Espacio));
            }
        }

        private string query = "";

        public string Query {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged(nameof(Query));
                FilterData(value);
            }
        }

        private static void FilterData(string value) {
            if (value != null) {
                CollectionViewSource.GetDefaultView(InitInfo.ObCoEspacios).Filter = item => (item as Espacio).NombreEspacio.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
            }
            else CollectionViewSource.GetDefaultView(InitInfo.ObCoEspacios);
        }

        public MainSettingsVM() {
            try {
                Log.Debug("Iniciada ventana de settings");
                SaveCommand = new RelayCommand(SaveEspacio);
                CancelCommand = new RelayCommand(Cancelar);
            }
            catch (Exception e) {
                Log.Error($"Error ventana de settings {e.Message}");
            }
            
        }

        private void Cancelar() {
            Espacio = new();
        }

        private async void SaveEspacio() {
            Log.Debug("Nuevo Espacio Guardado");
            await AdminOtrosTipos.Alta(Espacio);
            Refresh();
        }

        private async void Refresh() {
            Espacio = new();
            InitInfo.ObCoEspacios.Clear();
            var espacios = await GetFromDb.GetEspacios();
            foreach (var item in espacios) {
                InitInfo.ObCoEspacios.Add(item);
            }
        }
    }
}
