using System;
using System.Collections.Generic;
using System.Linq;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Init;
using log4net;

namespace GymCastillo.Model.Notificaciones {
    /// <summary>
    /// Clase que contiene los métodos para obtener las notificaciones y los cuales son necesarios para que el programa
    /// Este al tanto de las fechas y los plazos.
    /// </summary>
    public static class Notificaciones {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de obtener los usuarios a los cuales se les expira su paquete en los próximos 15 dias
        /// </summary>
        /// <param name="dias">El número de dias a verificar por la fecha de vencimiento de pago, por defecto 15</param>
        /// <returns>Una lista con clientes</returns>
        public static IEnumerable<Cliente> GetNextExpireUsers(int dias=15) {
            Log.Debug("Se ha iniciado el proceso de ver que clientes están próximos a que se les expire su paquete");

            // Obtenemos los usuarios a los cuales se les vence su paquete en los próximos 15 dias.
            var usuarios =
                InitInfo.ObCoClientes.Where(x => x.FechaVencimientoPago < DateTime.Today + TimeSpan.FromDays(dias));

            return usuarios;
        }
    }


}