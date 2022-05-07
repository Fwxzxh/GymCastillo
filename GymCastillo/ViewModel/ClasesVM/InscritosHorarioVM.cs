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

namespace GymCastillo.ViewModel.ClasesVM {
    public class InscritosHorarioVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private Horario horario;

        public Horario Horario {
            get { return horario; }
            set
            {
                horario = value;
                OnPropertyChanged(nameof(Horario));
            }
        }

        private string nombreClase;

        public string NombreClase {
            get { return nombreClase; }
            set
            {
                nombreClase = value;
                OnPropertyChanged(nameof(NombreClase));
            }
        }

        private int diaClase;

        public int DiaClase {
            get { return diaClase; }
            set
            {
                diaClase = value;
                OnPropertyChanged(nameof(DiaClase));
            }
        }

        private ObservableCollection<Cliente> listaClientes;

        public ObservableCollection<Cliente> ListaClientes {
            get { return listaClientes; }
            set
            {
                listaClientes = value;
                OnPropertyChanged(nameof(ListaClientes));
            }
        }


        public InscritosHorarioVM(Horario horario) {
            this.horario = horario;
            NombreClase = InitInfo.ObCoClases.Where(x => x.IdClase == horario.IdClase).First().NombreClase;
            DiaClase = horario.Dia;
            ListaClientes = new ObservableCollection<Cliente>();
            var listTemp = ListaAlumnosHelper.GetClientesDeHorario(horario.IdHorario);
            foreach (var item in listTemp) {
                ListaClientes.Add(item);
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
