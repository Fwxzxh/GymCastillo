using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Ventas;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.VentasVM {
    public class InventarioVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<bool> SaveCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        private Inventario inventario = new();

        public Inventario Inventario {
            get { return inventario; }
            set
            {
                inventario = value;
                OnPropertyChanged(nameof(Inventario));
            }
        }

        public InventarioVM() {
            SaveCommand = new RelayCommand<bool>(NuevoProducto);
            DeleteCommand = new RelayCommand(BorrarProducto);
        }

        private async void BorrarProducto() {
            if (ShowPrettyMessages.QuestionYesNo("¿Desea eliminar el producto?", "Confirmación")) {
                await AdminOtrosTipos.Delete(Inventario);
                RefreshGrid();
            }
            else {
                RefreshGrid();
                return;
            }
        }

        private async void NuevoProducto(bool guardar) {
            Log.Debug("Boton para dar de alta un nuevo producto en inventario");
            if (guardar) {
                await AdminOtrosTipos.Alta(Inventario);
                var clases = await GetFromDb.GetClases();
                InitInfo.ObCoClases.Clear();
                foreach (var item in clases) {
                    InitInfo.ObCoClases.Add(item);
                }

            }
            else {
                await AdminOtrosTipos.Update(Inventario);
            }
            RefreshGrid();
        }

        private async void RefreshGrid() {
            Inventario = null;
            Inventario = new();
            var listainventario = await GetFromDb.GetInventario();
            InitInfo.ObCoInventario.Clear();
            foreach (var item in listainventario) {
                InitInfo.ObCoInventario.Add(item);
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
