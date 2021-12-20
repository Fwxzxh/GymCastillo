using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Movimientos;
using log4net;

namespace GymCastillo.Model.Helpers {
    /// <summary>
    /// Clase que se encarga de manejar los pagos
    /// </summary>
    public class PagosHelper {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        // 1. Me llega el objeto del front con los datos,
        // 2. Valido que los datos necesarios para el tipo de operación estén.
        // 3. hago el insert a donde va (Ingresos/Egresos)
        // 4. Actualizo los datos del usuario si es que se necesitan (FechaUltimoPago,MontoUltimoPago, FechaVencimientoPago).
        // 5. Genero el ticket WIP

        /// <summary>
        /// Método que da de alta un nuevo Ingreso.
        /// </summary>
        /// <param name="ingreso">Un objeto con la información del ingreso.</param>
        public async Task NewIngreso(Ingresos ingreso) {
            // TODO: implementar lo de los tickets.
            Log.Debug("Se ha iniciado el proceso de dar de alta un nuevo ingreso");

            ingreso.FechaRegistro = DateTime.Now;

            try {
                switch (ingreso.Tipo) {
                    case 1: // Cliente
                        // Tienen que estar: FechaRegistro, IdUsuario, Concepto, NumRecibo?, Monto, IdCliente, IdRenta.
                        ingreso.IdRenta = 0;
                        ingreso.IdVenta = 0;

                        await AdminOnlyAlta.Alta(ingreso);
                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo ingreso tipo Cliente");
                        break;
                    case 2: // Ventas
                        // Tienen que estar: FechaRegistro, IdUsuario, Concepto, NumRecibo?, Monto, IdVenta.
                        ingreso.IdRenta = 0;
                        ingreso.IdCliente = 0;
                        ingreso.IdLocker = 0;

                        await AdminOnlyAlta.Alta(ingreso);
                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo ingreso tipo Ventas");
                        break;
                    case 3: //Rentas
                        // Primero se tiene que hacer la renta!!!
                        // Tienen que estar: FechaRegistro, IdUsuario, Concepto, NumRecibo?, Monto, IdRenta
                        ingreso.IdCliente = 0;
                        ingreso.IdLocker = 0;
                        ingreso.IdVenta = 0;

                        await AdminOnlyAlta.Alta(ingreso);
                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo ingreso tipo Rentas");
                        break;
                    case 4: // Otros
                        // Tienen que estar: FechaRegistro, IdUsuario, Concepto, NumRecibo?, Monto,
                        ingreso.IdCliente = 0;
                        ingreso.IdLocker = 0;
                        ingreso.IdVenta = 0;
                        ingreso.IdRenta = 0;

                        await AdminOnlyAlta.Alta(ingreso);
                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo ingreso tipo Otros");
                        break;
                    default:
                        throw new KeyNotFoundException("No se ha encontrado el tipo");
                }
            }
            catch (KeyNotFoundException e) {
                Log.Error("Ha ocurrido un error al identificar el tipo de ingreso");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    "Ha ocurrido un error desconocido al identificar el tipo de ingreso, contacte a los administradores.",
                    "Error Desconocido.");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido al Hacer el proceso de categorizar y registrar el ingreso.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al Hacer el proceso de registrar el ingreso, Error: {e.Message}",
                    "Error desconocido.");
            }
        }

        /// <summary>
        /// Método que da de alta un nuevo egreso.
        /// </summary>
        /// <param name="egreso">Un objeto con la información del egreso.</param>
        public async Task NewEgreso(Egresos egreso) {
            Log.Debug("Se ha iniciado el proceso de dar de alta un nuevo egreso.");

            egreso.FechaRegistro = DateTime.Now;

            try {
                switch (egreso.Tipo) {
                    case 1: // NominaUsuarios:
                        // Tienen que estar FechaRegistro: IdUsuario, Concepto, NumRecibo?, Monto, IdUsuarioPagar.
                        egreso.Nomina = true;

                        await AdminOnlyAlta.Alta(egreso);
                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo egreso de tipo Nómina Usuarios.");
                        break;
                    case 2: // NominaInstructores:
                        // Tienen que estar FechaRegistro: IdUsuario, Concepto, NumRecibo?, Monto, IdInstructor.
                        egreso.Nomina = true;

                        await AdminOnlyAlta.Alta(egreso);
                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo egreso de tipo Nómina Instructores.");
                        break;
                    case 3: // Servicios
                        // Tienen que estar FechaRegistro: IdUsuario, Concepto, NumRecibo?, Monto, Servicios.
                        egreso.Servicios = true;

                        await AdminOnlyAlta.Alta(egreso);
                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo egreso de tipo Servicios.");
                        break;
                    case 4: // Otros
                        // Tienen que estar FechaRegistro: IdUsuario, Concepto, NumRecibo?, Monto, Otros.
                        egreso.Otros = true;

                        await AdminOnlyAlta.Alta(egreso);
                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo egreso de tipo Otros.");
                        break;
                    default:
                        // Error
                        throw new KeyNotFoundException("No se ha encontrado el tipo");
                }

            }
            catch (KeyNotFoundException e) {
                Log.Error("Ha ocurrido un error al identificar el tipo de egreso");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    "Ha ocurrido un error desconocido al identificar el tipo de egreso, contacte a los administradores.",
                    "Error Desconocido.");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido al Hacer el proceso de categorizar y registrar el egreso.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al Hacer el proceso de registrar el egreso, Error: {e.Message}",
                    "Error desconocido.");
            }
        }
    }
}