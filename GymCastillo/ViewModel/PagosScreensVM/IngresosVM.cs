using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.PagosScreensVM {
    public class IngresosVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand PagoCliente { get; set; }

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
            set { ingresos = value;
                OnPropertyChanged(nameof(Ingresos));
            }
        }


        public IngresosVM() {
            PagoCliente = new RelayCommand(ClientsPyment);
        }

        private async void ClientsPyment() {
            ingresos.Tipo = 1;
            ingresos.Monto = paquete.Costo;
            ingresos.IdPaquete = paquete.IdPaquete;
            ingresos.IdCliente = cliente.Id;
            ingresos.FechaRegistro = DateTime.Now;
            PagosHelper pagos = new();
            await pagos.NewIngreso(ingresos);
            RefreshGrid();

        }

        private async void RefreshGrid() {
            var pagos = await GetFromDb.GetIngresos();
            InitInfo.ObCoIngresos.Clear();
            foreach (var item in pagos) {
                InitInfo.ObCoIngresos.Add(item);
            }

        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
