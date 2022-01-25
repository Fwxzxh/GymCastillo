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
        public RelayCommand CancelCommand { get; set; }

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
            CancelCommand = new RelayCommand(Cancelar);
        }

        private void Cancelar() {
            Inventario = null;
            Inventario = new();
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
                var lista = await GetFromDb.GetInventario();
                foreach (var item in InitInfo.ObCoInventario) {
                    if (item.NombreProducto == inventario.NombreProducto) {
                        var producto = InitInfo.ObCoInventario.First(x => x.NombreProducto == inventario.NombreProducto);
                        inventario = producto;
                        await producto.UpdateExistencias(1, true);
                        break;
                    }
                    else if (!string.IsNullOrEmpty(inventario.Descripción) &&
                        inventario.Costo != 0 && inventario.Existencias != 0) {
                        await AdminOtrosTipos.Alta(Inventario);
                        break;
                    }
                    //else {
                    //    ShowPrettyMessages.WarningOk("El producto no existe", "Aviso");
                    //    break;
                    //}
                }

                InitInfo.ObCoInventario.Clear();
                foreach (var item in lista) {
                    InitInfo.ObCoInventario.Add(item);
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
