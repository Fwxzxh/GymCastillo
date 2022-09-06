using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.View.ClsesScreensView;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.ClasesVM {
    public class ClasesVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand InscritosCommand { get; set; }

        private Horario horario = new();

        public Horario Horario {
            get { return horario; }
            set
            {
                horario = value;
                OnPropertyChanged(nameof(Horario));
            }
        }

        public ClasesVM() {
            InscritosCommand = new RelayCommand(VerInscritos);
        }

        private void VerInscritos() {
            InscritosHorarioWindow inscritosHorario = new(Horario);
            inscritosHorario.ShowDialog();
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
