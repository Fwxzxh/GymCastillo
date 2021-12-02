using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using log4net;

namespace GymCastillo.Model.Init {
    /// <summary>
    /// Clase que se encarga de hacer todas las queries necesarias para el inicio del programa.
    /// </summary>
    public static class InitInfo {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// La lista de todos los clientes.
        /// </summary>
        public static List<Cliente> ListaClientes { get; set; }

        /// <summary>
        /// La lista de todos los instructores.
        /// </summary>
        public static List<Instructor> ListaInstructor { get; set; }

        /// <summary>
        /// La lista de todos los usuarios.
        /// </summary>
        public static List<Usuario> ListaUsuarios { get; set; }

        /// <summary>
        /// La lista de todos los clientes de renta.
        /// </summary>
        public static List<ClienteRenta> ListaClientesRenta { get; set; }

        /// <summary>
        /// La lista de todos los paquetes.
        /// </summary>
        public static List<Paquete> ListaDePaquetes { get; set; }

        /// <summary>
        /// La lista de todos los tipos de clientes.
        /// </summary>
        public static List<Tipo> ListaTipoCliente { get; set; }

        /// <summary>
        /// La lista de todos los tipos de instructores.
        /// </summary>
        public static List<Tipo> ListaTipoInstructor { get; set; }

        /// <summary>
        /// La lista de todos los lockers Disponibles.
        /// </summary>
        public static List<Locker> ListaLockersOpen { get; set; }

        /// <summary>
        /// La lista de todos los lockers.
        /// </summary>
        public static List<Locker> ListaLockers { get; set; }

        /// <summary>
        /// La lista de todas las clases.
        /// </summary>
        public static List<Clase> ListaClases { get; set; }

        /// <summary>
        /// La lista que contiene TODOS los horarios.
        /// </summary>
        public static List<Horario> ListHorarios { get; set; }

        /// <summary>
        /// La lista que contiene todos los espacios.
        /// </summary>
        public static List<Espacio> ListEspacios { get; set; }

        /// <summary>
        /// Método que lanza las queries de manera asíncrona y obtiene los resultados.
        /// </summary>
        public static async Task<bool> GetAllInfo() {
            Log.Info("Se ha empezado el proceso de obtener la información de la base de datos.");

            try {
                // Lanzamos las tareas.
                var allClientes = GetFromDb.GetClientes();
                var allInstructores = GetFromDb.GetInstructores();
                var allUsuarios = GetFromDb.GetUsuarios();
                var allClientesRenta = GetFromDb.GetClientesRenta();

                var allPaquetes = GetFromDb.GetPaquetes();
                var allTipoClientes = GetFromDb.GetTipoCliente();
                var allTipoInstructores = GetFromDb.GetTipoInstructor();
                var allLockersOpen = GetFromDb.GetLockers(true);

                var allLockers = GetFromDb.GetLockers();
                var allClases = GetFromDb.GetClases();
                var allHorarios = GetFromDb.GetHorarios();
                var allEspacios = GetFromDb.GetEspacios();

                // Esperamos los resultados...
                ListaClientes = await allClientes;
                ListaInstructor = await allInstructores;
                ListaUsuarios = await allUsuarios;
                ListaClientesRenta = await allClientesRenta;

                ListaDePaquetes = await allPaquetes;
                ListaTipoCliente = await allTipoClientes;
                ListaTipoInstructor = await allTipoInstructores;
                ListaLockersOpen = await allLockersOpen;

                ListaLockers = await allLockers;
                ListaClases = await allClases;
                ListHorarios = await allHorarios;
                ListEspacios = await allEspacios;

                // Nos aseguramos que todas las tareas hayan terminado.
                await Task.WhenAll(
                    allClientes, allInstructores, allUsuarios, allClientesRenta,
                    allPaquetes, allTipoClientes, allTipoInstructores, allLockersOpen,
                    allLockersOpen, allClases, allHorarios, allEspacios).ConfigureAwait(false);
                Log.Info("Se ha obtenido toda la información de la base de datos.");

                return true;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al obtener toda la información inicial de la base de datos.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    "Ha ocurrido un error al obtener la información inicial de la base de datos, si este error persiste, contacte a los administradores",
                    "Error fatal.");
                throw;
            }
        }
    }
}