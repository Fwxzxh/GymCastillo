using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GymCastillo.ViewModel.AsistenciasVM {
    public class AsistenciaClientesVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }
        public RelayCommand<Horario> AddCommand { get; set; }

        public RelayCommand<IClosable> AsistenciaCommand { get; set; }

        private ObservableCollection<Horario> selectedHorario;

        private Cliente cliente = new();

        public Cliente Cliente {
            get { return cliente; }
            set
            {
                cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }

        private Asistencia asistencia;

        public Asistencia Asistencia {
            get { return asistencia; }
            set
            {
                asistencia = value;
                OnPropertyChanged(nameof(Asistencia));
            }
        }

        private List<Horario> listaHorarios;

        public List<Horario> ListaHorarios {
            get { return listaHorarios; }
            set
            {
                listaHorarios = value;
                OnPropertyChanged(nameof(ListaHorarios));
            }
        }

        private Horario horario;

        public Horario Horario {
            get { return horario; }
            set
            {
                horario = value;
                OnPropertyChanged(nameof(Horario));
            }
        }

        public AsistenciaClientesVM(Asistencia asistencia) {
            Log.Debug("Ventana Asistencia Clientes iniciada");
            this.asistencia = asistencia;
            Cliente = asistencia.DatosCliente;
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            ListaHorarios = asistencia.ListaHorarios;
            selectedHorario = new ObservableCollection<Horario>();
            AddCommand = new RelayCommand<Horario>(AddHorarios);
            AsistenciaCommand = new RelayCommand<IClosable>(this.AltaAsistencia);
        }

        private async void AltaAsistencia(IClosable closable) {
            asistencia.ClasesAEntrar = selectedHorario.Select(x => x.IdHorario).ToList();
            asistencia.NúmeroClasesAEntrar = selectedHorario.Count();
            await AsistenciasHelper.AsistenciaCliente(asistencia);
            RefreshData();
            closable.Close();
        }

        private async void RefreshData() {
            var clientes = await GetFromDb.GetClientes();
            var horarios = await GetFromDb.GetHorarios();
            InitInfo.ObCoClientes.Clear();
            InitInfo.ObCoHorarios.Clear();
            foreach (var item in clientes) {
                InitInfo.ObCoClientes.Add(item);
            }
            foreach (var item in horarios) {
                InitInfo.ObCoHorarios.Add(item);
            }
        }

        private void AddHorarios(Horario horario) {
            if (!selectedHorario.Contains(Horario)) {
                selectedHorario.Add(Horario);
            }
            else if (selectedHorario.Contains(Horario)) {
                selectedHorario.Remove(Horario);
            }
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
