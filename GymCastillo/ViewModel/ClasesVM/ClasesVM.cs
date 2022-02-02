using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Settings;
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

        private ObservableCollection<Horario> horarios;

        public ObservableCollection<Horario> Horarios {
            get { return horarios; }
            set
            {
                horarios = value;
                OnPropertyChanged(nameof(Horarios));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public ClasesVM() {
            GetHorarios();
        }

        private async void GetHorarios() {
            var lista = await GetFromDb.GetHorarios();
            //foreach (var item in lista.Where(c => c.Dia)) {

            //}
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
