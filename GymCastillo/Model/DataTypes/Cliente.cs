using System;
using GymCastillo.Model.DataTypes.Abstract;

namespace GymCastillo.Model.DataTypes {
    /// <summary>
    /// Clase que contiene los campos y métodos del objeto Cliente
    /// </summary>
    public class Cliente : AbstClientInstructor{

        /// <summary>
        /// Si el cliente tiene alguna condición especial.
        /// </summary>
        public bool CondicionEspecial { get; set; }

        /// <summary>
        /// Id del tipo de cliente.
        /// </summary>
        public int IdTipoCliente { get; set; }

        /// <summary>
        /// Nombre el tipo de cliente.
        /// </summary>
        public string NombreTipoCliente { get; set; }

        /// <summary>
        /// La deuda actual del cliente.
        /// </summary>
        public decimal DeudaCliente { get; set; }

        /// <summary>
        /// Las últimas asistencias del cliente.
        /// </summary>
        // TODO: ver como manejar esto en la bd.
        public string Asistencias { get; set; }

        /// <summary>
        /// La fecha del último pago del cliente.
        /// </summary>
        public DateTime FechaUltimoPago { get; set; }

        /// <summary>
        /// Monto del último pago del cliente.
        /// </summary>
        public decimal MontoUltimoPago { get; set; }

        /// <summary>
        /// Indica si el cliente esta activo.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Método que Actualiza la instancia actual del cliente en la Base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas en la bd.</returns>
        public override int Update() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que borra (desactiva) la instancia actual del cliente en la Base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas en la bd.</returns>
        public override int Delete() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de alta la instancia actual del cliente en la base de datos.
        /// </summary>
        /// <returns>El número de columnas afectadas de la bd.</returns>
        public override int Alta() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que valida el objeto actual del tipo cliente.
        /// </summary>
        /// <returns> <c>True </c> Si la instancia actual del objeto es válido o no.</returns>
        public override bool Validate() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que obtiene el horario de la instancia del cliente y lo da en un string.
        /// </summary>
        /// <returns>El horario en un string.</returns>
        public override string GetHorarioStr() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de alta una clase al cliente que corresponde a la instancia actual.
        /// </summary>
        /// <param name="clase"></param>
        public override void AltaClase(Clase clase) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que da de baja una lcase al cliente que corresponde a la insancia actual.
        /// </summary>
        /// <param name="clase"></param>
        public override void BajaClase(Clase clase) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que guarda una nueva asistencia al cliente de la instancia actual.
        /// </summary>
        /// <param name="fecha">La fecha de la asistencia</param>
        // TODO: ver que onda con las asistencias para ver si son al entrar al gym o por clase.
        public override void NuevaAsistencia(DateTime fecha) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que cobra la cantidad correspondiente a el tipo de usuario
        /// o una cantidad dada al cliente de la instancia actual.
        /// </summary>
        public void Cobrar() {
            throw new NotImplementedException();
        }
    }
}