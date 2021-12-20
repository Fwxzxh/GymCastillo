using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.Init;
using log4net;

namespace GymCastillo.Model.Helpers {
    /// <summary>
    /// Clase que se encarga de manejar los registros de las asistencias.
    /// </summary>
    public class AsistenciasHelper {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        // Se actualiza:
        // Cliente: FechaUltimoAcceso, ClasesTotalesDisponibles (si entra), ClasesSemanaDisponibles (si entra).
        // Instructor: FechaUltimoAcceso, Sueldo a descontar (con la hora), DiasTrabajados.
        // Horarios: CupoActual.

        // Proceso:
        // 1. Obtenemos que tipo es y el Id.
        // 2. Verificamos si pueden entrar.
        //     a. Si es Cliente, verificamos su fecha de VencimientoPago.
        //     b. Si es instructor, Verificamos su hora de entrada.
        // 2-1. Si no pueden entrar.
        //     a. SI es Cliente, Mostramos que no pueden entrar al gym.
        //     b. Si es instructor, Mostramos el campo para preguntar si queremos ponerle una penalización x retraso.
        // 3. Si, si pueden entrar.
        //     a. SI es Cliente, Obtenemos las clases disponibles (En tiempo y Capacidad).
        //         1. Guardamos la asistencia actualizando los campos de Cliente y de Horarios.
        //     b. Si es instructor, Podemos Registrar la asistencia Actualizando los campos de Instructor.


        /// <summary>
        /// Método que checa si los datos iniciales de la asistencia (Tipo e Id) son válidos.
        /// </summary>
        /// <param name="asistencia">Un objeto con los datos iniciales de la asistencia.</param>
        /// <returns><c>true</c> Si los datos son correctos y el usuario existe.</returns>
        public bool CheckId(Asistencia asistencia) {
            if (asistencia.Tipo == 1) {
                // es cliente
                var clienteQuery = InitInfo.ObCoClientes.Where(x => x.Id == asistencia.Id)
                    .AsParallel().ToList();

                if (clienteQuery.Count == 0) {
                    // El cliente no se encontró
                    ShowPrettyMessages.WarningOk(
                        $"No se ha podido encontrar el cliente con Id {asistencia.Id.ToString()}.",
                        "Cliente No encontrado.");
                    return false;
                }

                // Existe
                return true;
            }
            // es instructor.

            // Obtenemos el instructor.
            var instructorQuery = InitInfo.ObCoInstructor.Where(x => x.Id == asistencia.Id)
                .AsParallel().ToList();

            if (instructorQuery.Count == 0) {
                // El instructor no se encontró
                ShowPrettyMessages.WarningOk(
                    $"No se ha podido encontrar el instructor con Id {asistencia.Id.ToString()}.",
                    "Cliente No encontrado.");
                return false;
            }

            // Existe
            return true;
        }

        /// <summary>
        /// Método que verifica si el usuario tiene permitido entrar.
        /// </summary>
        /// <param name="asistencia">El objeto con la información inicial de la asistencia.</param>
        /// <returns>
        /// Una tupla con un bool indicando si se le puede dar acceso y un nuevo objeto asistencia actualizado.
        /// </returns>
        public (bool, Asistencia) CheckEntrada(Asistencia asistencia) {
            if (asistencia.Tipo == 1) {
                // Cliente

                // Obtenemos el cliente.
                var cliente = InitInfo.ObCoClientes.Where(x => x.Id == asistencia.Id)
                    .AsParallel().ToList().First();

                asistencia.DatosCliente = cliente;

                // Validamos si puede entrar.
                if (DateTime.Today > cliente.FechaVencimientoPago) {
                    ShowPrettyMessages.WarningOk(
                        $"El cliente {cliente.Nombre} {cliente.ApellidoPaterno}, se le ha vencido su Pago el {cliente.FechaVencimientoPago.ToString(CultureInfo.InvariantCulture)}",
                        "Pago Vencido.");

                    return (false, asistencia);
                }
                // Si puede entrar.

                // Obtenemos la lista de las clases a las que puede entrar.
                var clases = InitInfo.ObCoClases.Where(
                    x => x.IdPaquete == asistencia.DatosCliente.IdPaquete).ToList();

                // Obtenemos los horarios de las clases a las que puede entrar.
                // TODO: probar esto.
                asistencia.GetHorarios(clases.Select(x => x.IdClase).ToList());

                // Puede entrar
                return (true, asistencia);
            }
            // Instructor

            // Obtenemos el instructor.
            var instructor = InitInfo.ObCoInstructor.Where(x => x.Id == asistencia.Id)
                .AsParallel().ToList().First();

            asistencia.DatosInstructor = instructor;

            // validamos si puede entrar.
            if (DateTime.Now > instructor.HoraEntrada) {
                ShowPrettyMessages.WarningOk(
                    $"El instructor {instructor.Nombre} {instructor.ApellidoPaterno} llego tarde, su hora de entrada es: {instructor.HoraEntrada.ToString()}",
                    "Retardo");
                return (false, asistencia);
            }

            // Puede Entrar
            return (true, asistencia);
        }

        /// <summary>
        /// Método que se encarga de la asistencia de un cliente.
        /// </summary>
        /// <param name="asistencia">Objeto que tiene la información de la asistencia.</param>
        private async Task AsistenciaCliente(Asistencia asistencia) {

            // Validamos si tienen clases disponibles.
            if (asistencia.NúmeroClasesAEntrar > asistencia.DatosCliente.ClasesSemanaDisponibles ||
                asistencia.NúmeroClasesAEntrar > asistencia.DatosCliente.ClasesTotalesDisponibles) {
                // No tienen clases suficientes para entrar.
                ShowPrettyMessages.WarningOk(
                    "El cliente no tiene suficientes clases disponibles para registrar su asistencia.",
                    "Insuficientes clases.");
                return;
            }

            // Guardamos la asistencia en cliente.
            var asistenciaTask =  asistencia.DatosCliente.NuevaAsistencia(asistencia.NúmeroClasesAEntrar);

            // Actualizamos el cupo en la clase.
            foreach (var horario in asistencia.ListaHorarios) {
                if (asistencia.ClasesAEntrar.Contains(horario.IdClase)) {

                    // si nuestra lista de id con las clases a entrar coincide con el horario.
                     var res = await Task.Run(() => horario.NuevaAsistencia());
                     if (res == 0) {
                         ShowPrettyMessages.WarningOk(
                             "No se ha actualizado la base de datos al intentar actualizar el cupo actual de la clase.",
                             "Si cambios.");
                     }
                }
            }

            var resAsistenciaTask = await asistenciaTask;

            if (resAsistenciaTask == 0) {
                ShowPrettyMessages.WarningOk(
                     "No se ha actualizado la base de datos a la hora de actualizar los datos de entrada del cliente.",
                     "Sin cambios.");
            }
        }

        /// <summary>
        /// Método que se encarga de la asistencia de un Instructor.
        /// </summary>
        private async Task AsistenciaInstructor(Asistencia asistencia) {

            // Registramos la asistencia.
            var taskAsistencia = asistencia.DatosInstructor.NuevaAsistencia(asistencia.SueldoADescontar);
            var res = await taskAsistencia;
            if (res == 0) {
                ShowPrettyMessages.WarningOk(
                     "No se ha actualizado la base de datos a la hora de actualizar los datos de entrada del Instructor.",
                     "Sin cambios.");
            }
        }
    }
}