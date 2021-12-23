using System;
using System.Collections.Generic;
using System.Linq;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Init;

namespace GymCastillo.Model.DataTypes.Otros {
    /// <summary>
    /// Clase que contiene los campos para registrar una nueva asistencia.
    /// </summary>
    public class Asistencia {

        /// <summary>
        /// El tipo de la asistencia, puede ser 1:Cliente, 2:Instructor.
        /// </summary>
        public int Tipo { get; set; }

        /// <summary>
        /// Objeto que contiene la información del cliente.
        /// </summary>
        public Cliente DatosCliente { get; set; }

        /// <summary>
        /// Objeto que contiene la información del Instructor.
        /// </summary>
        public Instructor DatosInstructor { get; set; }

        /// <summary>
        /// El Id del cliente o Instructor.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// indica si se le concede la entrada al usuario.
        /// </summary>
        public bool Entrada { get; set; }

        /// <summary>
        /// El sueldo a descontar en el caso de que se aplique la penalización.
        /// </summary>
        public decimal SueldoADescontar { get; set; }

        /// <summary>
        /// Lista con los Id de los horarios a los cuales se va a entrar.
        /// </summary>
        public List<int> ClasesAEntrar { get; set; } = new();

        /// <summary>
        /// El número de clases a entrar para saber cuantas descontar.
        /// </summary>
        public int NúmeroClasesAEntrar { get; set; }

        /// <summary>
        /// Una lista con los horarios de clases disponibles para cierta hora de entrada.
        /// </summary>
        public List<Horario> ListaHorarios { get; set; }

        /// <summary>
        /// Método que llena la lista de <see cref="ListaHorarios"/>, con los horarios para la hora actual.
        /// </summary>
        /// <param name="clases">Lista Con las clases a las que puede entrar el cliente.</param>
        public void GetHorarios(IEnumerable<int> clases) {
            // TODO: probar esto
            var horarios =
                InitInfo.ObCoHorarios.Where(
                    x => clases.Contains(x.IdClase) && x.HoraInicio > DateTime.Now)
                    .OrderBy(x => x.HoraInicio).AsParallel().ToList();

            ListaHorarios = horarios;
        }
    }
}