using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes.Personal;
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
        }

        private void BorrarPersonal() {
            throw new NotImplementedException();
        }

        private void NewWindowPersonal() {
            NewPersonalWindow window = new();
            window.ShowDialog();
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
