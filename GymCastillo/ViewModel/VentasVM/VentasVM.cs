﻿using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes.Ventas;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GymCastillo.Model.Database;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;

namespace GymCastillo.ViewModel.VentasVM {
    public class VentasVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand AddVenta { get; set; }
        public RelayCommand RemoveVenta { get; set; }
        public RelayCommand MakeVenta { get; set; }
        public RelayCommand CancelVenta { get; set; }

        private Venta venta = new();

        public Venta Venta {
            get { return venta; }
            set
            {
                venta = value;
                OnPropertyChanged(nameof(Venta));
            }
        }

        private Inventario selected;

        public Inventario Selected {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        private int index;

        public int Index {
            get { return index; }
            set
            {
                index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        private string concepto;

        public string Concepto {
            get { return concepto; }

            set {
                concepto = value;
                OnPropertyChanged(nameof(concepto));
            }
        }

        private decimal costo;

        public decimal Costo {
            get { return costo; }
            set
            {
                costo = value;
                OnPropertyChanged(nameof(Costo));
            }
        }

        private decimal recibido = 0;

        public decimal Recibido {
            get { return recibido; }
            set
            {
                recibido = value;
                OnPropertyChanged(nameof(Recibido));
                ActualizarTotal();
            }
        }

        private decimal total = 0;

        public decimal Total {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        private bool gym;

        public bool Gym {
            get { return gym; }
            set
            {
                gym = value;
                OnPropertyChanged(nameof(Gym));
                RefreshGrid();
            }
        }

        private ObservableCollection<Inventario> listaVenta;

        public ObservableCollection<Inventario> ListaVenta {
            get { return listaVenta; }
            set
            {
                listaVenta = value;
                OnPropertyChanged(nameof(ListaVenta));
            }
        }

        public VentasVM() {
            ListaVenta = new ObservableCollection<Inventario>();
            AddVenta = new RelayCommand(NuevoProducto);
            RemoveVenta = new RelayCommand(RemoverProducto);
            MakeVenta = new RelayCommand(HacerVenta);
            CancelVenta = new RelayCommand(Cancelar);
        }

        private void Cancelar() {
            Gym = false;
            ClearFields();
        }

        private async void HacerVenta() {
            Venta.FechaVenta = DateTime.Now;
            Venta.Concepto = Concepto;
            Venta.Costo = Costo;
            Venta.VisitaGym = Gym;

            foreach (var item in ListaVenta) {
                Venta.IdsProductos += $"{item.IdProducto.ToString()},";
            }
            // await AdminOnlyAlta.Alta(Venta);
            await VentasHelper.NuevaVenta(Venta, Recibido);
            ClearFields();

            if (!gym) { // Solo si no es una entrada a un gym.
                // Actualizamos el inventario por las existencias.
                var updatedInventario = await GetFromDb.GetInventario();
                InitInfo.ObCoInventario.Clear();
                foreach (var item in updatedInventario) {
                    InitInfo.ObCoInventario.Add(item);
                }
            }

            // Actualizamos los ingresos
            var updatedIngresos = await GetFromDb.GetIngresos();
            InitInfo.ObCoIngresos.Clear();
            foreach (var item in updatedIngresos) {
                InitInfo.ObCoIngresos.Add(item);
            }
        }

        private void ClearFields() {
            Costo = 0;
            Recibido = 0;
            Total = 0;
            Concepto = "";
            Venta = null;
            venta = new();
            ListaVenta.Clear();
        }

        private void RemoverProducto() {
            ListaVenta.RemoveAt(Index);
            RefreshCosto();
        }

        private void NuevoProducto() {
            ListaVenta.Add(Selected);
            RefreshCosto();
        }

        private void RefreshGrid() {
            if (ListaVenta != null) {
                ListaVenta.Clear();
            }
            ClearFields();
        }

        private void ActualizarTotal() {
            if (Recibido != 0) {
                Total = Math.Max(Recibido - Costo, 0);
            }
        }

        private void RefreshCosto() {
            Costo = 0;
            var costo = 0.0m;
            foreach (var item in ListaVenta) {
                costo += item.Costo;
            }
            Costo += costo;
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
