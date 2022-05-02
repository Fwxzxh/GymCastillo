using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.IntersectionTables;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.PersonalScreensVM.ClientsVM {
    public class ClientesHorarioVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand AddHorario { get; set; }
        public RelayCommand DeleteHorario { get; set; }

        private Cliente cliente;

        public Cliente Cliente {
            get { return cliente; }
            set
            {
                cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }

        private ObservableCollection<Horario> horarios;

        public ObservableCollection<Horario> Horarios {
            get { return horarios; }
            set
            {
                horarios = value;
                OnPropertyChanged(nameof(Horarios));
            }
        }

        private ObservableCollection<Horario> horarios1;

        public ObservableCollection<Horario> Horarios1 {
            get { return horarios1; }
            set
            {
                horarios1 = value;
                OnPropertyChanged(nameof(Horarios1));
            }
        }

        private Horario selectedHorario;

        public Horario SelectedHorario {
            get { return selectedHorario; }
            set
            {
                selectedHorario = value;
                OnPropertyChanged(nameof(SelectedHorario));
            }
        }
        public ClienteHorario clienteHorario = new();

        public int idPaquete { get; set; }

        public ClientesHorarioVM(Cliente cliente) {
            this.cliente = cliente;
            var paquetes = InitInfo.ObCoDePaquetes.First(x => x.IdPaquete == cliente.IdPaquete);
            Horarios = new ObservableCollection<Horario>();
            Horarios1 = new ObservableCollection<Horario>();
            // obtenemos la lista de IdClases a las que puede entrar el cliente
            var paquetesClases =
                InitInfo.ListPaquetesClases.Where(x => x.IdPaquete == cliente.IdPaquete)
                    .Select(x => x.IdClase);

            // Obtenemos los horarios
            var horarios =
                InitInfo.ObCoHorarios.Where(x => paquetesClases.Contains(x.IdClase));
            foreach (var item in horarios) {
                Horarios.Add(item);
            }

            RefreshGrid();

            AddHorario = new RelayCommand(AgregarHorario);
            DeleteHorario = new RelayCommand(BorrarHorarioAsync);
        }

        private void RefreshGrid() {
            if (Horarios1 != null) Horarios1.Clear();
            var idHorario = InitInfo.ObCoClienteHorario.Where(x => x.IdCliente == cliente.Id).Select(x => x.IdHorario);
            var horarios = InitInfo.ObCoHorarios.Where(x => idHorario.Contains(x.IdHorario));
            foreach (var item in horarios) {
                Horarios1.Add(item);
            }
        }

        private async void BorrarHorarioAsync() {

            clienteHorario.IdHorario = SelectedHorario.IdHorario;
            clienteHorario.IdCliente = Cliente.Id;

            await AdminOtrosTipos.Delete(clienteHorario);
            var list = await GetFromDb.GetClienteHorario(); 
            InitInfo.ObCoClienteHorario.Clear();
            foreach (var item in list) {
                InitInfo.ObCoClienteHorario.Add(item);
            }
            RefreshGrid();
        }

        private async void AgregarHorario() {
            //checamos que no demos de alta más clases de las permitidas en el paquete por semana
            if (InitInfo.ObCoClienteHorario.Where(x => x.IdCliente == cliente.Id).Count() > cliente.ClasesSemanaDisponibles) {
                ShowPrettyMessages.ErrorOk("Número máximo de clases dado de alta", "Error");
                return;
            }

            clienteHorario.IdHorario = SelectedHorario.IdHorario;
            clienteHorario.IdCliente = Cliente.Id;

            await AdminOtrosTipos.Alta(clienteHorario);
            var list = await GetFromDb.GetClienteHorario();
            InitInfo.ObCoClienteHorario.Clear();
            foreach (var item in list) {
                InitInfo.ObCoClienteHorario.Add(item);
            }
            RefreshGrid();
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
