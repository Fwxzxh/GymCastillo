using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using log4net;

namespace GymCastillo.Model.Init {
    /// <summary>
    /// Clase que se encarga de hacer todas las queries necesarias para el inicio del programa.
    /// </summary>
    public class InitInfo {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// La ObservableCollection  de todos los clientes.
        /// </summary>
        public static ObservableCollection<Cliente> ObCoClientes { get; set; }

        /// <summary>
        /// La ObservableCollection  de todos los instructores.
        /// </summary>
        public static ObservableCollection<Instructor> ObCoInstructor { get; set; }

        /// <summary>
        /// La ObservableCollection  de todos los usuarios.
        /// </summary>
        public static ObservableCollection<Usuario> ObCoUsuarios { get; set; }

        /// <summary>
        /// La ObservableCollection  de todos los clientes de renta.
        /// </summary>
        public static ObservableCollection<ClienteRenta> ObCoClientesRenta { get; set; }

        /// <summary>
        /// La ObservableCollection de todos en el personal.
        /// </summary>
        public static ObservableCollection<Personal> ObCoPersonal { get; set; }

        /// <summary>
        /// La ObservableCollection  de todos los paquetes.
        /// </summary>
        public static ObservableCollection<Paquete> ObCoDePaquetes { get; set; }

        /// <summary>
        /// La ObservableCollection  de todos los tipos de clientes.
        /// </summary>
        public static ObservableCollection<Tipo> ObCoTipoCliente { get; set; }

        /// <summary>
        /// La ObservableCollection  de todos los tipos de instructores.
        /// </summary>
        public static ObservableCollection<Tipo> ObCoTipoInstructor { get; set; }

        /// <summary>
        /// La ObservableCollection  de todos los lockers Disponibles.
        /// </summary>
        public static ObservableCollection<Locker> ObCoLockersOpen { get; set; }

        /// <summary>
        /// La ObservableCollection  de todos los lockers.
        /// </summary>
        public static ObservableCollection<Locker> ObCoLockers { get; set; }

        /// <summary>
        /// La ObservableCollection  de todas las clases.
        /// </summary>
        public static ObservableCollection<Clase> ObCoClases { get; set; }

        /// <summary>
        /// La ObservableCollection  que contiene TODOS los horarios.
        /// </summary>
        public static ObservableCollection<Horario> ObCoHorarios { get; set; }

        /// <summary>
        /// La ObservableCollection  que contiene todos los espacios.
        /// </summary>
        public static ObservableCollection<Espacio> ObCoEspacios { get; set; }

        /// <summary>
        /// La ObservableCollection  que contiene todos los ingresos.
        /// </summary>
        public static ObservableCollection<Ingresos> ObCoIngresos { get; set; }

        /// <summary>
        /// La ObservableCollection que contiene todos los egresos.
        /// </summary>
        public static ObservableCollection<Egresos> ObCoEgresos { get; set; }


        /// <summary>
        /// Indica si las queries han terminado de ejecutarse.
        /// </summary>
        public readonly bool DoneTasks;

        /// <summary>
        /// Inicializa las queries en paralelo y retorna un true cuando todas terminen.
        /// </summary>
        public InitInfo() {
            if (DoneTasks) return;
            var done = Task.Run(GetAllInfo);
            DoneTasks = done.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Método que lanza las queries de manera asíncrona y obtiene los resultados.
        /// </summary>
        public static async Task<bool> GetAllInfo() {
            Log.Info("Se ha empezado el proceso de obtener la información de la base de datos.");

            try { // Lanzamos las tareas.
                var allClientes = GetFromDb.GetClientes();
                var allInstructores = GetFromDb.GetInstructores();
                var allUsuarios = GetFromDb.GetUsuarios();
                var allClientesRenta = GetFromDb.GetClientesRenta();

                var allPaquetes = GetFromDb.GetPaquetes();
                var allTipoClientes = GetFromDb.GetTipoCliente();
                var allTipoInstructores = GetFromDb.GetTipoInstructor();
                // TODO: igual y ya no ocupamos las queries de onlyOpen.
                var allLockersOpen = GetFromDb.GetLockers(true);

                var allLockers = GetFromDb.GetLockers();
                var allClases = GetFromDb.GetClases();
                var allHorarios = GetFromDb.GetHorarios();
                var allEspacios = GetFromDb.GetEspacios();

                var allIngresos = GetFromDb.GetIngresos();
                var allEgresos = GetFromDb.GetEgresos();
                var allPersonal = GetFromDb.GetPersonal();

                // Nos aseguramos que todas las tareas hayan terminado.
                await Task.WhenAll(
                    allClientes, allInstructores, allUsuarios, allClientesRenta,
                    allPaquetes, allTipoClientes, allTipoInstructores, allLockersOpen,
                    allLockersOpen, allClases, allHorarios, allEspacios,
                    allIngresos, allEgresos, allPersonal).ConfigureAwait(false);
                Log.Info("Se ha obtenido toda la información de la base de datos.");

                // Esperamos los resultados...
                ObCoClientes = await allClientes;
                ObCoInstructor = await allInstructores;
                ObCoUsuarios = await allUsuarios;
                ObCoClientesRenta = await allClientesRenta;

                ObCoDePaquetes = await allPaquetes;
                ObCoTipoCliente = await allTipoClientes;
                ObCoTipoInstructor = await allTipoInstructores;
                ObCoLockersOpen = await allLockersOpen;

                ObCoLockers = await allLockers;
                ObCoClases = await allClases;
                ObCoHorarios = await allHorarios;
                ObCoEspacios = await allEspacios;

                ObCoIngresos = await allIngresos;
                ObCoEgresos = await allEgresos;
                ObCoPersonal = await allPersonal;

                return true;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener toda la información inicial de la base de datos.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    "Ha ocurrido un error al obtener la información inicial de la base de datos, si este error persiste, contacte a los administradores",
                    "Error fatal.");
                return false;
            }
        }
    }
}