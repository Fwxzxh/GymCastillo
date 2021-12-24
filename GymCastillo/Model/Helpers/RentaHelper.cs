using System;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Init;
using log4net;

namespace GymCastillo.Model.Helpers {
    /// <summary>
    /// Clase que expone la función de Hacer una Renta
    /// </summary>
    public static class RentaHelper {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método Para dar de alta una Renta registrando antes la renta y después el ingreso
        /// </summary>
        /// <param name="renta">El objeto con la información de la renta.</param>
        /// <param name="clienteRenta">El objeto con la información del cliente</param>
        /// <param name="montoRecibido">La cantidad recibida por la renta (para calcular la deuda)</param>
        public static async Task NuevaRenta(Rentas renta, ClienteRenta clienteRenta, decimal montoRecibido) {
            // Debemos de Hacer la renta y luego registrar el ingreso
            Log.Debug("Se ha iniciado el proceso de registrar una renta.");

            try {
                // Registramos la renta
                var res = await AdminOnlyAlta.Alta(renta);

                if (res) { // Si se completo la alta exitosamente proseguimos

                    // Llenamos el ingreso con los datos necesarios.
                    var ingreso = new Ingresos {
                        Tipo = 3,
                        Concepto = "",
                        NumeroRecibo = "",
                        Monto = renta.Costo,
                        MontoRecibido = montoRecibido
                    };

                    // Obtenemos el IdRenta de la renta dada de alta.
                    if (InitInfo.ObCoRentas.Count > 0) {
                        var idRentaMax = InitInfo.ObCoRentas.Max(x => x.IdRenta);
                        ingreso.IdRenta = idRentaMax + 1;
                    }
                    else {
                        ingreso.IdRenta = 1;
                    }

                    // Registramos el ingreso
                    await PagosHelper.NewIngreso(ingreso);

                    // Actualizamos la información del cliente
                    clienteRenta.FechaUltimoPago = DateTime.Now;
                    clienteRenta.MontoUltimoPago = montoRecibido;
                    clienteRenta.DeudaCliente += renta.Costo - montoRecibido;

                    // Actualizamos los campos del cliente
                    await AdminUsuariosGeneral.Pago(clienteRenta);

                    Log.Debug("Se ha completado el proceso de registro de renta de manera exitosa.");
                }
                else {
                    ShowPrettyMessages.ErrorOk(
                        "Ha ocurrido un error al registrar la renta, Contacte a los administradores",
                        "Error al registrar la renta");
                }
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido al Hacer el proceso de registrar la renta.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al Hacer el proceso de registrar la renta de espacio, Error: {e.Message}",
                    "Error desconocido.");
            }
        }
    }
}