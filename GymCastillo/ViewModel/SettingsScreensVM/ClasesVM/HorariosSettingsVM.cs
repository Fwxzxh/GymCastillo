using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Settings;
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

namespace GymCastillo.ViewModel.SettingsScreensVM.ClasesVM {
    public class HorariosSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Clase clase;

        public Clase Clase {
            get { return clase; }
            set { clase = value;
                OnPropertyChanged(nameof(Clase));
            }
        }

        private int dia;

        public int Dia {
            get { return dia; }
            set { dia = value;
                OnPropertyChanged(nameof(Dia));
            }
        }

        private Horario horario = new();

        public Horario Horarios {
            get { return horario; }
            set { horario = value;
                OnPropertyChanged(nameof(Horarios));
            }
        }

        public RelayCommand<IClosable> CloseCommand { get; private set; }

        public RelayCommand AgregarCommand { get; set; }

        public ObservableCollection<Horario> ListaHorarios { get; set; }

        public HorariosSettingsVM(Clase clase) {
            this.clase = clase;
            ListaHorarios = new ObservableCollection<Horario>();
            HorariosClase(clase.IdClase);
            CloseCommand = new RelayCommand<IClosable>(this.CloseWindow);
            AgregarCommand = new RelayCommand(AgregarHorario);
        }

        private void AgregarHorario() {
            //FrontHorario front = new();
            //front.AddHorario(Horarios, Dia);
            MessageBox.Show($"{Horarios.HoraInicio} {Horarios.HoraFin} {Dia}");
        }

        private async void HorariosClase(int id) {
            var horarios = await GetFromDb.GetHorarios();
            foreach (var item in horarios.Where(h => h.IdClase == id).OrderBy(dia => dia.Dia)) {
                ListaHorarios.Add(item);
            }
        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }
    }
}
