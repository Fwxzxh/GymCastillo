using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.ComponentModel;
using System.Windows;
using GymCastillo.Model.DataTypes.IntersectionTables;
using GymCastillo.Model.Helpers;

namespace GymCastillo.ViewModel.VentasVM {
    public class RentasVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public RelayCommand<bool> SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand DeleteRenta { get; set; }

        private Rentas renta = new() { FechaRenta = DateTime.Now };

        public Rentas Renta {
            get { return renta; }
            set
            {
                renta = value;
                OnPropertyChanged(nameof(Renta));
            }
        }

        private Rentas selected;

        public Rentas Selected {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }


        private ClienteRenta clienteRenta;

        public ClienteRenta ClienteRenta {
            get { return clienteRenta; }
            set
            {
                clienteRenta = value;
                OnPropertyChanged(nameof(ClienteRenta));
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


        public event PropertyChangedEventHandler PropertyChanged;

        public RentasVM() {
            SaveCommand = new RelayCommand<bool>(GuardarRenta);
            CancelCommand = new RelayCommand(CancelarRenta);
            DeleteRenta = new RelayCommand(BorrarRenta);
        }

        private void BorrarRenta() {
            MessageBox.Show("Implementar borrar renta");
        }

        private void CancelarRenta() {
            ClienteRenta = null;
            Renta = null;
            Espacio = null;
            ClienteRenta = new();
            Espacio = new();
            Renta = new() { FechaRenta = DateTime.Now };
        }

        private async void GuardarRenta(bool guardar) {
            renta.IdClienteRenta = clienteRenta.Id;
            renta.IdEspacio = espacio.IdEspacio;

            // await AdminOnlyAlta.Alta(Renta);
            await RentaHelper.NuevaRenta(renta, clienteRenta);
            RefreshGrid();

            // Actualizamos los ingresos
            var updatedIngresos = await GetFromDb.GetIngresos();
            InitInfo.ObCoIngresos.Clear();
            foreach (var item in updatedIngresos) {
                InitInfo.ObCoIngresos.Add(item);
            }
        }

        private async void RefreshGrid() {
            var list = await GetFromDb.GetRentas();
            InitInfo.ObCoRentas.Clear();
            foreach (var item in list) {
                InitInfo.ObCoRentas.Add(item);
            }

        }
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
