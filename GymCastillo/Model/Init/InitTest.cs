using System.Collections.ObjectModel;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;

namespace GymCastillo.Model.Init {
    /// <summary>
    /// fdaff
    /// </summary>
    public class InitTest {

        /// <summary>
        /// La ObservableCollection de todos los clientes.
        /// </summary>
        public ObservableCollection<Cliente> ObCoClientes { get; set; }

        /// <summary>
        /// La ObservableCollection de todos los instructores.
        /// </summary>
        public ObservableCollection<Instructor> ObCoInstructor { get; set; }

        /// <summary>
        /// La ObservableCollection de todos los usuarios.
        /// </summary>
        public ObservableCollection<Usuario> ObCoUsuario { get; set; }

        /// <summary>
        /// La ObservableCollection de todos los clientes de renta.
        /// </summary>
        public ObservableCollection<ClienteRenta> ObCoClienteRenta { get; set; }

        /// <summary>
        /// La ObservableCollection de todos los paquetes.
        /// </summary>
        public ObservableCollection<Paquete> ObCoPaquete { get; set; }

        /// <summary>
        /// La ObservableCollection de todos los tipos de clientes.
        /// </summary>
        public ObservableCollection<Tipo> ObCoTipoCliente { get; set; }

        /// <summary>
        /// La ObservableCollection de todos los tipos de instructores.
        /// </summary>
        public ObservableCollection<Tipo> ObCoTipoInstructor { get; set; }

        /// <summary>
        /// La ObservableCollection de todos los lockers Disponibles.
        /// </summary>
        public ObservableCollection<Locker> ObCoLockerDisponibles { get; set; }

        /// <summary>
        /// La ObservableCollection de todos los lockers.
        /// </summary>
        public ObservableCollection<Locker> ObCoLocker { get; set; }

        /// <summary>
        /// La ObservableCollection de todas las clases.
        /// </summary>
        public ObservableCollection<Clase> ObCoClases { get; set; }

        /// <summary>
        /// La ObservableCollection que contiene TODOS los horarios.
        /// </summary>
        public ObservableCollection<Horario> ObCoHorario { get; set; }

        /// <summary>
        /// La ObservableCollection que contiene todos los espacios.
        /// </summary>
        public ObservableCollection<Espacio> ObCoEspacio { get; set; }


        /// <summary>
        /// Constructor de InitTest,
        /// </summary>
        public InitTest() {
            ObCoClientes = new ObservableCollection<Cliente>(InitInfo.ListaClientes);
            ObCoInstructor = new ObservableCollection<Instructor>(InitInfo.ListaInstructor);
            ObCoUsuario = new ObservableCollection<Usuario>(InitInfo.ListaUsuarios);
            ObCoClienteRenta = new ObservableCollection<ClienteRenta>(InitInfo.ListaClientesRenta);

            ObCoPaquete = new ObservableCollection<Paquete>(InitInfo.ListaDePaquetes);
            ObCoTipoCliente = new ObservableCollection<Tipo>(InitInfo.ListaTipoCliente);
            ObCoTipoInstructor = new ObservableCollection<Tipo>(InitInfo.ListaTipoInstructor);
            ObCoLockerDisponibles = new ObservableCollection<Locker>(InitInfo.ListaLockersOpen);

            ObCoLocker = new ObservableCollection<Locker>(InitInfo.ListaLockers);
            ObCoClases = new ObservableCollection<Clase>(InitInfo.ListaClases);
            ObCoHorario = new ObservableCollection<Horario>(InitInfo.ListHorarios);
            ObCoEspacio = new ObservableCollection<Espacio>(InitInfo.ListEspacios);
        }
    }
}