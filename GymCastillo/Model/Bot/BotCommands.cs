using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;
using Telegram.Bot;
using File = System.IO.File;

namespace GymCastillo.Model.Bot {
    /// <summary>
    /// Clase que se encarga de ejecutar y verificar los comandos del bot.
    /// </summary>
    public static class BotCommands {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Método que se encarga de guardar el chatId de un usuario en telegram en la base de datos.
        /// </summary>
        /// <param name="args">Un array de strings con los argumentos del comando</param>
        /// <param name="chatId"> El id de chat del usuario que esta queriendo registrarse.</param>
        /// <param name="botClient"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><c>true</c> si el proceso se hizo satisfactoriamente.</returns>
        public static async Task<bool> Auth(string[] args, long chatId, ITelegramBotClient botClient,
            CancellationToken cancellationToken) {
            // en la primera posición esta la palabra auth
            // en la segunda debe de haber un id y en la tercera la contraseña.
            try {
                if (args.Length != 3) throw new Exception("No se introdujo el comando de manera correcta.");

                var id = int.Parse(args[1]);
                var pass = args[2];

                var clientesActuales = new List<Cliente>(InitInfo.ObCoClientes);

                if (clientesActuales.Any(x => x.Id == id)) {
                    if (pass == Bot.Pass) {

                        // Damos de alta su chat id
                        await using var connection = new MySqlConnection(GetInitData.ConnString);
                        await connection.OpenAsync(cancellationToken);
                        Log.Debug("Se ha creado la conexión.");

                        const string altaId = "update cliente set ChatID=@ChatId where IdCliente=@IdCliente";

                        await using var command = new MySqlCommand(altaId, connection);

                        command.Parameters.AddWithValue("@ChatId", chatId.ToString());
                        command.Parameters.AddWithValue("@IdCliente", id.ToString());

                        var res = await ExecSql.NonQuery(command, "Update ChatId Cliente");
                        Log.Debug("Se ha registrado el chatId de un cliente.");

                        if (res > 0) {
                            await botClient.SendTextMessageAsync(chatId,
                                "Se te ha dado de alta de manera exitosa en la base de datos.",
                                cancellationToken: cancellationToken);
                            return true;
                        }
                    }
                    else {
                        await botClient.SendTextMessageAsync(chatId,
                            "La el código dado es incorrecto, favor de verificarlo con el operador.",
                            cancellationToken: cancellationToken);
                        return true;
                    }
                }
                else {
                    await botClient.SendTextMessageAsync(chatId,
                        $"No he podido encontrar el id: {id.ToString()} en la base de datos, verificalo.",
                        cancellationToken: cancellationToken);
                    return true;
                }

                return false;
            }
            catch (Exception e) {
                Trace.WriteLine($"BOT: comando invalido para auth, Error: {e.Message};");
                Log.Debug("BOT_ERROR: Comando invalido para auth");
                Log.Debug($"BOT_ERROR: {e.Message}");
                return false;
            }
        }

        public static async Task<bool> Status(string[] args, long chatId, ITelegramBotClient botClient,
            CancellationToken cancellationToken) {

            try {
                // obtenemos el cliente
                var cliente = InitInfo.ObCoClientes.First(x => x.ChatId == chatId.ToString());


                Paquete paquete;
                if (cliente.IdPaquete == 0) {
                    paquete = new Paquete {
                        NombreClase = "",
                        NombrePaquete = "Sin paquete"
                    };
                }
                else {
                    paquete = InitInfo.ObCoDePaquetes.First(x => x.IdPaquete == cliente.IdPaquete);
                }

                var msg = "";

                msg += $"Id:{cliente.Id.ToString()} Nombre: {cliente.Nombre} {cliente.ApellidoPaterno}\n";
                msg += $"Paquete: {paquete.NombrePaquete} Clases: {paquete.NombreClase}\n";
                msg += $"Fecha vencimiento paquete: {cliente.FechaVencimientoPago:dd/MM/yyyy}\n";
                msg += $"Fecha de ultimo pago: {cliente.FechaUltimoPago:dd/MM/yyyy}\n";
                msg += $"Monto de ultimo pago: {cliente.MontoUltimoPago.ToString(CultureInfo.InvariantCulture)}\n";
                msg += $"Clases Semanales disponibles: {cliente.ClasesSemanaDisponibles.ToString()}\n";
                msg += $"Clases Totales Disponibles: {cliente.ClasesTotalesDisponibles.ToString()}\n";
                msg += $"Deuda: {cliente.DeudaCliente.ToString(CultureInfo.InvariantCulture)}";

                await botClient.SendTextMessageAsync(chatId,
                    msg,
                    cancellationToken: cancellationToken);

                return true;
            }
            catch (Exception e) {
                Trace.WriteLine($"BOT: Ha ocurrido un error en el comando status, Error: {e.Message};");
                Log.Error("BOT_ERROR: Ha ocurrido un error al obtener el status.");
                Log.Error($"BOT_ERROR: {e.Message}");
                return false;
            }
        }

        public static async Task<bool> Card(string[] args, long chatId, ITelegramBotClient botClient,
            CancellationToken cancellationToken) {

            try {
                var client = InitInfo.ObCoClientes.First(x => x.ChatId == chatId.ToString());

                var path = $"{client.ClienteDir}Card-{client.Id.ToString()}.png";

                if (File.Exists(path)) {
                        await using var stream = File.OpenRead(path);

                        await botClient.SendPhotoAsync(chatId,
                            stream,
                            cancellationToken: cancellationToken);
                    return true;
                }
                else {
                    DigitalCard.DrawCard(client, true);

                    await using var stream = File.OpenRead(path);

                    await botClient.SendPhotoAsync(chatId,
                        stream,
                        cancellationToken: cancellationToken);
                    return true;
                }
            }
            catch (Exception e) {
                Trace.WriteLine($"BOT: Ha ocurrido un error en el comando card, Error: {e.Message};");
                Log.Error("BOT_ERROR: Ha ocurrido un error al obtener el status.");
                Log.Error($"BOT_ERROR: {e.Message}");
                return false;
            }
        }


        /// <summary>
        /// Método que se encarga de obtener las clases y horarios disponibles del usuario que las necesita.
        /// </summary>
        /// <param name="chatId">El id del chat del usuario.</param>
        /// <param name="botClient"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><c>true</c> si el proceso se completo de manera correcta.</returns>
        public static async Task<bool> Clases(long chatId, ITelegramBotClient botClient,
            CancellationToken cancellationToken) {

            try {
                if (InitInfo.ObCoClientes.Any(x => x.ChatId == chatId.ToString())) {
                    var cliente = InitInfo.ObCoClientes.First(x => x.ChatId == chatId.ToString());

                    // no tiene paquete asignado
                    if (cliente.IdPaquete == 0) {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "No tienes registrado ningún paquete asi que no tienes clases disponibles.",
                            cancellationToken: cancellationToken);
                        return true;
                    }

                    // Checamos si tiene clases
                    // obtenemos las clases del paquete.
                    var clasesDelPaquete = InitInfo.ListPaquetesClases
                        .Where(x => x.IdPaquete == cliente.IdPaquete)
                        .Select(x => x.IdClase).AsParallel().ToList();

                    if (clasesDelPaquete.Count == 0) {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Tu paquete no tiene clases registradas.",
                            cancellationToken: cancellationToken);
                        return true;
                    }

                    var clases = InitInfo.ObCoClases.Where(x => clasesDelPaquete.Contains(x.IdClase));

                    var mensaje = "";
                    foreach (var clase in clases) {
                            mensaje += $"{clase.NombreClase}:\n";
                        var horarios = InitInfo.ObCoHorarios.
                            Where(x => x.IdClase == clase.IdClase).OrderBy(x => x.Dia);
                        foreach (var horario in horarios) {
                            mensaje +=
                                $"{((Dias) horario.Dia).ToString()} {horario.HoraInicio:h:mm tt} - {horario.HoraFin:h:mm tt} \n";
                        }
                    }
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: mensaje,
                        cancellationToken: cancellationToken);
                    return true;
                }
                else {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Tu chat no se encuentra registrado, " +
                              "debes registrarte primero con el comando auth para poder usar el bot" ,
                        cancellationToken: cancellationToken);
                    return true;
                }
            }
            catch (Exception e) {
                Trace.WriteLine(e);
                return false;
            }
        }
    }
}