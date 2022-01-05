using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.PagosScreensVM {
    public class EgresosVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand PagoUsuario { get; set; }
        public RelayCommand PagoInstructor { get; set; }
        public RelayCommand PagoPersonal { get; set; }
        public RelayCommand PagoServicios { get; set; }
        public RelayCommand PagoOtros { get; set; }

        // private PagosHelper pagos = new();

        private Usuario usuario = new();

        public Usuario Usuario {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        private Instructor instructor = new();

        public Instructor Instructor {
            get { return instructor; }
            set
            {
                instructor = value;
                OnPropertyChanged(nameof(Instructor));
                if (instructor != null) {
                    MontoFinal = instructor.Sueldo - instructor.SueldoADescontar;
                }
            }
        }

        private Personal personal = new();

        public Personal Personal {
            get { return personal; }
            set
            {
                personal = value;
                OnPropertyChanged(nameof(Personal));
            }
        }

        private Egresos egresos = new();

        public Egresos Egresos {
            get { return egresos; }
            set
            {
                egresos = value;
                OnPropertyChanged(nameof(Egresos));
            }
        }

        private int item = -1;

        public int Item {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged(nameof(Item));
                ClearData();
            }
        }
        private decimal montoFinal;

        public decimal MontoFinal {
            get { return montoFinal; }
            set
            {
                montoFinal = value;
                OnPropertyChanged(nameof(MontoFinal));
            }
        }

        public EgresosVM() {
            PagoUsuario = new RelayCommand(UserPayment);
            PagoInstructor = new RelayCommand(InstructorPayment);
            PagoPersonal = new RelayCommand(PersonalPayment);
            PagoServicios = new RelayCommand(ServicesPayment);
            PagoOtros = new RelayCommand(OthersPayment);
            RefreshGrid();
        }

        private async void OthersPayment() {
            egresos.Tipo = 5;
            await PagosHelper.NewEgreso(egresos);
            RefreshGrid();
        }

        private async void ServicesPayment() {
            egresos.Tipo = 4;
            await PagosHelper.NewEgreso(egresos);
            RefreshGrid();
        }

        private async void PersonalPayment() {
            egresos.Tipo = 3;
            egresos.IdPersonal = personal.Id;
            await PagosHelper.NewEgreso(egresos);
            RefreshGrid();
        }

        private async void InstructorPayment() {
            egresos.Tipo = 2;
            egresos.IdInstructor = instructor.Id;
            egresos.Monto = MontoFinal;
            await PagosHelper.NewEgreso(egresos);
            RefreshGrid();
        }

        private async void UserPayment() {
            egresos.Tipo = 1;
            egresos.IdUsuarioPagar = usuario.Id;
            await PagosHelper.NewEgreso(egresos);
            RefreshGrid();
        }

        private async void RefreshGrid() {
            var pagos = await GetFromDb.GetEgresos();
            InitInfo.ObCoEgresos.Clear();
            foreach (var item in pagos.OrderByDescending(c => c.FechaRegistro)) {
                InitInfo.ObCoEgresos.Add(item);
            }
            ClearData();
        }

        private void ClearData() {
            Usuario = null;
            Usuario = new();
            Instructor = null;
            Instructor = new();
            Personal = null;
            Personal = new();
            Egresos = null;
            Egresos = new();
            MontoFinal = 0;
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
