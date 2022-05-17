using System;
using System.Linq;
using System.Threading.Tasks;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Ventas;
using GymCastillo.Model.Init;
using log4net;

namespace GymCastillo.Model.Helpers {
    /// <summary>
    /// Clase que expone la función de Hacer una venta.
    /// </summary>
    public static class VentasHelper {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public static async Task NuevaVenta(Venta venta, decimal montoRecibido) {
            Log.Debug("Se ha iniciado el proceso de registrar una Venta");
            // Puede ser venta de inventario o venta de entrada al gym.
            // Verificamos que tipo de venta es.

            // El costo debe de ser coherente.
            if (montoRecibido < venta.Costo) {
                ShowPrettyMessages.ErrorOk(
                    "El monto recibido de la venta no puede ser menor al costo de la venta.",
                    "Transacción invalida.");
                return;
            }

            //venta.IdsProductos = venta.VisitaGym ? "" : venta.IdsProductos;
            //if (venta.IdsProductos != "" && venta.VisitaGym == false) {

            if (!string.IsNullOrWhiteSpace(venta.IdsProductos)) {
                var listaProductos = venta.IdsProductos.Split(",");

                var allProductos = InitInfo.ObCoInventario;

                foreach (var producto in listaProductos) {
                    var cantidad = listaProductos.Count(x => x == producto);
                    var tenemosExistencias = allProductos
                        .Where(x => x.IdProducto.ToString() == producto)// Obtengo el producto
                        .All(x => x.Existencias - cantidad >= 0);
                    if (!tenemosExistencias) {
                        ShowPrettyMessages.ErrorOk(
                            $"No tenemos suficientes existencias del producto con Id: {producto}",
                            "Inventario insuficiente.");
                        return;
                    }
                }
            }

            try {
                // Lanzamos la venta
                var res = await AdminOnlyAlta.Alta(venta, true);

                if (res) { // Si se completo exitosamente la venta
                    Log.Debug("Se completo exitosamente la venta");

                    // Actualizamos el inventario
                    if (!string.IsNullOrWhiteSpace(venta.IdsProductos)) {
                        var listaIdCarrito = venta.IdsProductos.Split(",");
                        var listaCarrito = InitInfo.ObCoInventario
                            .Where(x => listaIdCarrito.Contains(x.IdProducto.ToString()));

                        foreach (var producto in listaCarrito) {
                            await producto.UpdateExistencias(
                                listaIdCarrito.Count(x => x == producto.IdProducto.ToString()));
                        }
                    }

                    //Actualizamos la lista de ventas
                    var ventasUpdated = await GetFromDb.GetVentas();
                    InitInfo.ObCoVentas.Clear();
                    foreach (var item in ventasUpdated) {
                        InitInfo.ObCoVentas.Add(item);
                    }

                    // Creamos el ingreso
                    var ingreso = new Ingresos() {
                        Tipo = 2,
                        Concepto = venta.Concepto,
                        NumeroRecibo = "",
                        Monto = venta.Costo,
                        MontoRecibido = montoRecibido
                    };

                    // Obtenemos el IdVenta de la renta dada de alta.
                    if (InitInfo.ObCoVentas.Count > 0) {
                        var idVentaMax = InitInfo.ObCoVentas.Max(x => x.IdVenta);
                        ingreso.IdVenta = idVentaMax    ;
                    }
                    else {
                        ingreso.IdVenta = 1;
                    }

                    // Registramos el ingreso
                    await PagosHelper.NewIngreso(ingreso);

                    Log.Debug("Se ha completado el proceso de registro de renta de manera exitosa.");
                }
                else {
                    ShowPrettyMessages.ErrorOk(
                        "Ha ocurrido un error al registrar la venta, Contacte a los administradores",
                        "Error al registrar la renta");
                }
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido al Hacer el proceso de registrar la Venta.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al Hacer el proceso de registrar la venta. Error: {e.Message}",
                    "Error desconocido.");
            }
        }
    }
}