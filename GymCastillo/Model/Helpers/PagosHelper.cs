using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Init;
using log4net;

namespace GymCastillo.Model.Helpers {
    /// <summary>
    /// Clase que se encarga de manejar los pagos
    /// </summary>
    public static class PagosHelper {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        // 1. Me llega el objeto del front con los datos,
        // 2. Valido que los datos necesarios para el tipo de operación estén.
        // 3. hago el insert a donde va (Ingresos/Egresos)
        // 4. Actualizo los datos del usuario si es que se necesitan (FechaUltimoPago,MontoUltimoPago, FechaVencimientoPago).
        // 5. Genero el ticket WIP

        /// <summary>
        /// Método que da de alta un nuevo Ingreso.
        /// </summary>
        /// <param name="ingreso">Un objeto con la información del ingreso.</param>
        /// <param name="silent"><c>true</c> para no mostrar el mensaje de operación exitosa</param>
        /// <param name="meses">Numero de meses que se va a pagar por la mensualidad, default 1</param>
        public static async Task NewIngreso(Ingresos ingreso, bool silent = false, int meses=1) {

            Log.Debug("Se ha iniciado el proceso de dar de alta un nuevo ingreso");

            ingreso.FechaRegistro = DateTime.Now;
            ingreso.NumeroRecibo = GetInitData.GetMonthMovNumerator().ToString();
            GetInitData.SetNextMonthMovNumerator();

            try {
                switch (ingreso.Tipo) {
                    case 1: // Cliente
                        // Tienen que estar: FechaRegistro, IdUsuario, Concepto, NumRecibo?, Monto, IdCliente, IdLocker.
                        ingreso.IdRenta = 0;
                        ingreso.IdVenta = 0;
                        ingreso.IdClienteRenta = 0;

                        // Obtenemos el cliente
                        var cliente = InitInfo.ObCoClientes.First(x => x.Id == ingreso.IdCliente);

                        // Actualizamos los datos del cliente.
                        cliente.MontoUltimoPago = ingreso.Monto;
                        cliente.FechaUltimoPago = ingreso.FechaRegistro;
                        cliente.Activo = true;

                        // Registramos el proceso.
                        await IngresoCliente(ingreso, cliente, meses);

                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo ingreso tipo Cliente");
                        break;

                    case 2: // Ventas
                        // Tienen que estar: FechaRegistro, IdUsuario, Concepto, NumRecibo?, Monto, IdVenta.
                        ingreso.IdRenta = 0;
                        ingreso.IdCliente = 0;
                        ingreso.IdLocker = 0;
                        ingreso.IdClienteRenta = 0;

                        // Registramos el Pago
                        await AdminOnlyAlta.Alta(ingreso);

                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo ingreso tipo Ventas");
                        break;

                    case 3: //Rentas
                        // Primero se tiene que hacer la renta!!!
                        // Tienen que estar: FechaRegistro, IdUsuario, Concepto, NumRecibo?, Monto, IdRenta
                        ingreso.IdCliente = 0;
                        ingreso.IdLocker = 0;
                        ingreso.IdVenta = 0;
                        ingreso.IdClienteRenta = 0;

                        // Registramos el Pago
                        await AdminOnlyAlta.Alta(ingreso, silent);

                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo ingreso tipo Rentas");
                        break;

                    case 4: // Otros
                        // Tienen que estar: FechaRegistro, IdUsuario, Concepto, NumRecibo?, Monto,
                        ingreso.IdCliente = 0;
                        ingreso.IdLocker = 0;
                        ingreso.IdVenta = 0;
                        ingreso.IdRenta = 0;
                        ingreso.IdClienteRenta = 0;
                        ingreso.Otros = true;

                        // Registramos el Pago
                        await AdminOnlyAlta.Alta(ingreso);

                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo ingreso tipo Otros");
                        break;

                    case 5: // Pago deuda cliente renta
                        // Tienen que estar: FechaRegistro, IdUsuario, Concepto, NumRecibo?, Monto, IdClienteRenta
                        ingreso.IdCliente = 0;
                        ingreso.IdLocker = 0;
                        ingreso.IdVenta = 0;
                        ingreso.IdRenta = 0;

                        // Registramos el Pago
                        var resPagoDeuda = await AdminOnlyAlta.Alta(ingreso);

                        if (resPagoDeuda) {
                            var clienteRenta = InitInfo.ObCoClientesRenta.First(x => x.Id == ingreso.IdClienteRenta);

                            clienteRenta.MontoUltimoPago = ingreso.Monto;
                            clienteRenta.FechaUltimoPago = DateTime.Now;
                            clienteRenta.DeudaCliente -= ingreso.MontoRecibido;

                            await clienteRenta.Pago();
                        }

                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo ingreso tipo Otros");
                        break;

                    default:
                        throw new KeyNotFoundException("No se ha encontrado el tipo");
                }
            }
            catch (ValidationException ) {
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
        /// Método que hace el proceso de dar de alta un ingreso a un cliente.
        /// </summary>
        /// <param name="ingreso">Un objeto con la información del ingreso.</param>
        /// <param name="cliente">Un objeto con la información del cliente</param>
        /// <param name="meses">Número de meses que se va a comprar el paquete, default 1</param>
        private static async Task IngresoCliente(Ingresos ingreso, Cliente cliente, int meses=1) {

            var paqueteAntiguo = cliente.IdPaquete;

            // Obtenemos el paquete y los datos de este y actualizamos. (si es que eligió uno)
            if (ingreso.IdPaquete != 0) {
                // Obtenemos el paquete.
                var paquete = InitInfo.ObCoDePaquetes.First(x => x.IdPaquete == ingreso.IdPaquete);

                // actualizamos.
                cliente.IdPaquete = ingreso.IdPaquete;

                // Si no tiene fecha de vencimiento (es un cliente nuevo)
                if (cliente.FechaVencimientoPago == DateTime.MinValue ||
                    cliente.FechaVencimientoPago == DateTime.MaxValue) {
                    // Sumamos un més a la fecha actual de pago
                    cliente.FechaVencimientoPago = ingreso.FechaRegistro.Date.AddMonths(meses);
                }
                else {
                    //calculo fecha de vencimiento

                    // hay tolerancia, si pagas 5 dias antes o 3 días después del corte
                    // conservamos el mismo dia del més de pago pasado, si no agregamos 30.
                    var hoy = DateTime.Today.DayOfYear;
                    var diaCorteCliente = cliente.FechaVencimientoPago.DayOfYear;

                    if (diaCorteCliente - 5 <= hoy || diaCorteCliente + 3 >= hoy) {
                        // Conservamos el dia del més del pago pasado
                        cliente.FechaVencimientoPago = cliente.FechaVencimientoPago.Date.AddMonths(meses);
                    }
                    else {
                        // Esta fuera del colchón, la fecha se calcula a partir del dia del pago.
                        cliente.FechaVencimientoPago = DateTime.Today.Date.AddMonths(meses);
                    }
                }

                // agregamos los demás campos.
                cliente.ClasesTotalesDisponibles = paquete.NumClasesTotales;
                cliente.ClasesSemanaDisponibles = paquete.NumClasesSemanales;
                cliente.DuraciónPaquete += 30;
            }

            // calculamos la deuda.
            if (ingreso.MontoRecibido < ingreso.Monto) {
                // hay deuda
                var newDeuda = ingreso.Monto - ingreso.MontoRecibido;
                cliente.DeudaCliente += newDeuda;
                ShowPrettyMessages.InfoOk(
                    $"Se va a abonar una deuda de: $ {newDeuda.ToString(CultureInfo.InvariantCulture)}",
                    "Deuda A abonar");
            }

            // Si dan más dinero descontamos de la deuda.
            if (ingreso.MontoRecibido > ingreso.Monto) {
                // No pueden haber deudas negativas.
                var resto = ingreso.MontoRecibido - ingreso.Monto;
                
                if (cliente.DeudaCliente - resto < 0) {
                    ShowPrettyMessages.WarningOk(
                        "Los clientes no pueden tener crédito, revisa los montos de los pagos, \n " +
                        $"Si la deuda esta en 0 no pueden pagar más de lo que deben ya que no tienen deuda.",
                        "Monto de pago invalido");
                        return;
                }

                cliente.DeudaCliente -= resto;
                cliente.MontoUltimoPago = resto;

                ShowPrettyMessages.InfoOk(
                    $"El cliente quedará con una deuda de: $ {cliente.DeudaCliente.ToString(CultureInfo.InvariantCulture)}",
                    "Actualización de deuda");
            }

            cliente.IdLocker = ingreso.IdLocker;

            // Registramos el Pago
            var res = await AdminOnlyAlta.Alta(ingreso, true);

            if (res) {
                // actualizamos los campos del cliente
                await AdminUsuariosGeneral.Pago(cliente);

                // Mandamos el ticket al bot
                if (cliente.ChatId != "") {
                    var msg = $"Se ha registrado exitosamente tu pago de $ {ingreso.MontoRecibido.ToString(CultureInfo.InvariantCulture)} \n" +
                              $"En la compra de: {ingreso.Concepto} \n" +
                              $"El {ingreso.FechaRegistro.ToString("g")} \n" +
                              $"¡Gracias por su preferencia!";
                    await Bot.Bot.SendMessage(msg, cliente.Id);
                }
            }
            else {
                throw new Exception("No se ha completado la alta del ingreso de manera correcta.");
            }

            if (ingreso.IdPaquete != paqueteAntiguo) { // Si se compra un paquete diferente al que se tenia

                // Obtenemos los horario
                var listaHorarios = 
                    InitInfo.ObCoClienteHorario.Where(x => x.IdCliente == cliente.Id).ToList();

                // Desactivamos
                foreach (var horario in listaHorarios) {
                    await horario.Delete();
                }
                
                // Actualizamos la lista de clientesHorario
                var nuevosClientesHorarios = await GetFromDb.GetClienteHorario();
                InitInfo.ObCoClienteHorario.Clear();
                foreach (var clientesHorario in nuevosClientesHorarios) {
                    InitInfo.ObCoClienteHorario.Add(clientesHorario);
                }
                
                ShowPrettyMessages.InfoOk(
                    "Se ha cambiado el paquete a el usuario, Debe actualizar los horarios de clase " +
                    "asignados a este cliente, de lo contrario podría no poder entrar las clases de su paquete " +
                    "o poder entrar a clases fuera de su paquete.",
                    "Cambio de paquete a usuario.");
            }
        }

        /// <summary>
        /// Método que da de alta un nuevo egreso.
        /// </summary>
        /// <param name="egreso">Un objeto con la información del egreso.</param>
        public static async Task NewEgreso(Egresos egreso) {
            Log.Debug("Se ha iniciado el proceso de dar de alta un nuevo egreso.");

            egreso.FechaRegistro = DateTime.Now;
            egreso.NumeroRecibo = GetInitData.GetMonthMovNumerator().ToString();
            GetInitData.SetNextMonthMovNumerator();

            try {
                switch (egreso.Tipo) {
                    case 1: // NominaUsuarios:
                        // Tienen que estar FechaRegistro: IdUsuario, Concepto, NumRecibo?, Monto, IdUsuarioPagar.
                        egreso.Nomina = true;

                        // Obtenemos el usuario
                        var usuario = InitInfo.ObCoUsuarios.First(x => x.Id == egreso.IdUsuarioPagar);

                        // Actualizamos
                        usuario.MontoUltimoPago = egreso.Monto;
                        usuario.FechaUltimoPago = egreso.FechaRegistro;

                        // Registramos el Pago
                        var res = await AdminOnlyAlta.Alta(egreso, true);

                        if (res) {
                            // Registramos
                            await AdminUsuariosGeneral.Pago(usuario);
                        }

                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo egreso de tipo Nómina Usuarios.");
                        break;

                    case 2: // NominaInstructores:
                        // Tienen que estar FechaRegistro: IdUsuario, Concepto, NumRecibo?, Monto, IdInstructor.
                        egreso.Nomina = true;

                        // Obtenemos al instructor
                        var instructor = InitInfo.ObCoInstructor.First(x => x.Id == egreso.IdInstructor);

                        // Actualizamos los campos.
                        instructor.MontoUltimoPago = egreso.Monto;
                        instructor.FechaUltimoPago = egreso.FechaRegistro;

                        instructor.DiasTrabajados = 0;
                        instructor.SueldoADescontar = 0;

                        // Registramos el Pago
                        var resInstructores = await AdminOnlyAlta.Alta(egreso, true);

                        if (resInstructores) {
                            // Registramos el pago
                            await AdminUsuariosGeneral.Pago(instructor);
                        }
                        else {
                            throw new Exception("No se registro el egreso de manera correcta.");
                        }

                        Log.Debug(
                            "Se ha terminado el proceso de dar de alta un nuevo egreso de tipo Nómina Instructores.");
                        break;

                    case 3: // NominaPersonal:
                        // Tienen que estar FechaRegistro: IdUsuario, Concepto, NumRecibo?, Monto, IdPersonal.
                        egreso.Nomina = true;

                        // Obtenemos al personal
                        var personal = InitInfo.ObCoPersonal.First(x => x.Id == egreso.IdPersonal);

                        // Actualizamos
                        personal.MontoUltimoPago = egreso.Monto;
                        personal.FechaUltimoPago = egreso.FechaRegistro;

                        // Registramos el Pago
                        var resAlta = await AdminOnlyAlta.Alta(egreso, true);

                        if (resAlta) {
                            // Registramos
                            await AdminUsuariosGeneral.Pago(personal);
                        }
                        else {
                            throw new Exception("no se registro el egreso de manera correcta.");
                        }

                        Log.Debug(
                            "Se ha terminado el proceso de dar de alta un nuevo egreso de tipo Nómina Instructores.");
                        break;

                    case 4: // Servicios
                        // Tienen que estar FechaRegistro: IdUsuario, Concepto, NumRecibo?, Monto, Servicios.
                        egreso.Servicios = true;

                        // Registramos el Pago
                        await AdminOnlyAlta.Alta(egreso);

                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo egreso de tipo Servicios.");
                        break;

                    case 5: // Otros
                        // Tienen que estar FechaRegistro: IdUsuario, Concepto, NumRecibo?, Monto, Otros.
                        egreso.Otros = true;

                        // Registramos el Pago
                        await AdminOnlyAlta.Alta(egreso);

                        Log.Debug("Se ha terminado el proceso de dar de alta un nuevo egreso de tipo Otros.");
                        break;

                    default:
                        // Error
                        throw new KeyNotFoundException("No se ha encontrado el tipo");
                }
            }
            catch (ValidationException ) {

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