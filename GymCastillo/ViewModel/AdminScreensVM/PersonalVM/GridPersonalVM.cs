using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.View.AdminScreensView.PersonalView;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.AdminScreensVM.PersonalVM {
    public class GridPersonalVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand NewPersonal { get; set; }
        public RelayCommand DeletePersonal { get; set; }
        public RelayCommand OverviewPersonal { get; set; }

        private Personal personal = new();

        public Personal Personal {
            get { return personal; }
            set
            {
                personal = value;
                OnPropertyChanged(nameof(Personal));
            }
        }

        private Personal selectedPersonal;

        public Personal SelectedPersonal {
            get { return selectedPersonal; }
            set
            {
                selectedPersonal = value;
                OnPropertyChanged(nameof(SelectedPersonal));
            }
        }

        public GridPersonalVM() {
            NewPersonal = new RelayCommand(NewWindowPersonal);
            DeletePersonal = new RelayCommand(BorrarPersonal);
            OverviewPersonal = new RelayCommand(OverviewWindo);
        }

        private void OverviewWindo() {
            OverviewPersonalWindow window = new(SelectedPersonal);
            window.ShowDialog();
            RefreshGrid();
        }

        private async void BorrarPersonal() {
            if (ShowPrettyMessages.QuestionYesNo("¿Desea borrar el personal seleccionado?", "Confirmación")) {
                await AdminUsuariosGeneral.Delete(SelectedPersonal);
                RefreshGrid();
            }
            else return;
        }

        private async void RefreshGrid() {
            var lista = await GetFromDb.GetPersonal();
            InitInfo.ObCoPersonal.Clear();
            foreach (var item in lista) {
                InitInfo.ObCoPersonal.Add(item);
            }
        }

        private void NewWindowPersonal() {
            NewPersonalWindow window = new();
            window.ShowDialog();
            RefreshGrid();
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
