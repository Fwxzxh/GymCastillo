using System.Collections.Generic;
using System.Linq;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Init;

namespace GymCastillo.Model.Helpers {
    /// <summary>
    /// Clase que se encarga de obtener la lista de alumnos de cierta clase u horario.
    /// </summary>
    public static class ListaAlumnosHelper {
        // private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de obtener la lista de clientes de cierta clase.
        /// </summary>
        /// <param name="idClase">El id de la clase de la que queremos saber los clientes.</param>
        /// <returns>La lista de alumnos</returns>
        public static List<Cliente> GetClientesDeClase(int idClase) {
            // Obtenemos los horarios del cliente.
            var listaHorarios = 
                InitInfo.ObCoHorarios.Where(x => x.IdClase == idClase)
                    .Select(x => x.IdHorario);
            
            // Con los horarios obtenemos la lista de clientes
            var listaIdClientes = 
                InitInfo.ObCoClienteHorario.Where(x => listaHorarios.Contains(x.IdHorario))
                    .Select(x => x.IdCliente);

            // obtenemos los clientes
            var clientes = 
                InitInfo.ObCoClientes.Where(x => listaIdClientes.Contains(x.Id)).ToList();

            return clientes;
        }


        /// <summary>
        /// Método que se encarga de obtener la lista de clientes de cierto horario.
        /// </summary>
        /// <param name="idHorario">El id del horario del que queremos obtener los clientes.</param>
        /// <returns>Una lista de clientes.</returns>
        public static List<Cliente> GetClientesDeHorario(int idHorario) {
            // Con los horarios obtenemos la lista de clientes
            var listaIdClientes = 
                InitInfo.ObCoClienteHorario.Where(x => x.IdHorario == idHorario)
                    .Select(x => x.IdCliente);

            // obtenemos los clientes
            var clientes = 
                InitInfo.ObCoClientes.Where(x => listaIdClientes.Contains(x.Id)).ToList();

            return clientes;
        }
    }
}