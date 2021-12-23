using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.ComponentModel;

namespace GymCastillo.ViewModel.PagosScreensVM {
    public class IngresosVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand PagoCliente { get; set; }
        public RelayCommand PagoVenta { get; set; }
        public RelayCommand PagoRenta { get; set; }
        public RelayCommand PagoOtros { get; set; }

        private Paquete paquete = new();

        public Paquete Paquete {
            get { return paquete; }
            set
            {
                paquete = value;
                OnPropertyChanged(nameof(Paquete));
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

        private Ingresos ingresos = new();

        public Ingresos Ingresos {
            get { return ingresos; }
            set
            {
                ingresos = value;
                OnPropertyChanged(nameof(Ingresos));
            }
        }

        private ClienteRenta clienteRenta = new();

        public ClienteRenta ClienteRenta {
            get { return clienteRenta; }
            set
            {
                clienteRenta = value;
                OnPropertyChanged(nameof(ClienteRenta));
            }
        }

        private Espacio espacio = new();

        public Espacio Espacio {
            get { return espacio; }
            set
            {
                espacio = value;
                OnPropertyChanged(nameof(Espacio));
            }
        }


        private int item = -1;

        public int Item {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged(nameof(Item));
                ClearData();
            }
        }


        public IngresosVM() {
            PagoCliente = new RelayCommand(ClientsPyment);
            PagoRenta = new RelayCommand(RentPayment);
            PagoVenta = new RelayCommand(SalesPayement);
            PagoOtros = new RelayCommand(OthersPayment);
        }

        private async void OthersPayment() {
            ingresos.Tipo = 4;
            await PagosHelper.NewIngreso(ingresos);
            RefreshGrid();
        }

        private void SalesPayement() {
            throw new NotImplementedException();
        }

        private void RentPayment() {
            throw new NotImplementedException();
        }

        private async void ClientsPyment() {
            ingresos.Tipo = 1;
            ingresos.Monto = paquete.Costo;
            ingresos.IdPaquete = paquete.IdPaquete;
            ingresos.IdCliente = cliente.Id;
            await PagosHelper.NewIngreso(ingresos);
            RefreshGrid();

        }

        private async void RefreshGrid() {
            var pagos = await GetFromDb.GetIngresos();
            InitInfo.ObCoIngresos.Clear();
            foreach (var item in pagos) {
                InitInfo.ObCoIngresos.Add(item);
            }
            ClearData();

        }

        private void ClearData() {
            Cliente = null;
            Paquete = null;
            Ingresos = null;
            Espacio = null;
            ClienteRenta = null;
            ClienteRenta = new();
            Espacio = new();
            Ingresos = new();
            Cliente = new();
            Paquete = new();
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
