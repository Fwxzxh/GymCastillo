using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace GymCastillo.Model.Bot {
    /// <summary>
    /// Clase que se encarga de manejar el bot de telegram.
    /// </summary>
    public class Bot {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?
            .DeclaringType);

        /// <summary>
        /// Indica si el bot esta corriendo o no.
        /// </summary>
        public static bool Estado { get; private set; }

        /// <summary>
        /// La contraseña para autenticar al usuario
        /// </summary>
        public static string Pass { get; private set; }

        /// <summary>
        /// EL logger del bot.
        /// </summary>
        public static string LogBot { get; set; }

        /// <summary>
        /// instancia del bot
        /// </summary>
        private static TelegramBotClient BotClient { set; get; }

        /// <summary>
        /// El token para cancelar el bot.
        /// </summary>
        private static CancellationTokenSource CancellationToken { set; get; }

        // Funcionamiento bot
        // <- Mandar ticket de venta si esta registrado.
        // <- Mandar updates de horarios y cosas asi.
        // <- Avisarte cuando tu paquete esta por acabar.
        // <-> Mandar la credencial Digital o pedirla: card
        // -> Registrarte y autenticarte: auth {idCliente} {passRandom}
        // -> Pedir un estado de tu usuario: estado
        // - Limitar la cantidad de trafico que acepta el bot

        /// <summary>
        /// Constructor del bot.
        /// </summary>
        /// <param name="apiKey"></param>
        public Bot(string apiKey) {

            if (apiKey == "") {
                ShowPrettyMessages.ErrorOk(
                    "No se hay apiKey guardada, debes guardar una y después iniciar el bot manualmente",
                    "No se encontró una api key");
                return;
            }

            if (Estado) {
                Log.Warn("Se ha intentado crear dos veces el bot ");
                ShowPrettyMessages.WarningOk(
                    "El bot ya esta en ejecución",
                    "Bot en ejecución");
                return;
            }

            try {
                Log.Info("Iniciando el bot.");

                BotClient = new TelegramBotClient(apiKey);

                CancellationToken = new CancellationTokenSource();

                var receiverOptions = new ReceiverOptions() {
                    ThrowPendingUpdates = true,
                    AllowedUpdates = new[] {UpdateType.Message},
                };

                BotClient.StartReceiving(
                    HandleUpdateTask,
                    HandleErrorAsync,
                    receiverOptions,
                    CancellationToken.Token
                );

                Log.Info("Se ha iniciado el bot.");
                LogBot += "Se ha iniciado el bot.\n";

                Estado = true;
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido al iniciar el bot.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al iniciar el bot de telegram, contacte a los administradores. Error: {e.Message}",
                    "Error desconocido");
            }
        }

        public void StopBot() {
            CancellationToken.Cancel();
            Estado = false;
            Log.Info("Se ha detenido el bot.");
            LogBot += "Se ha detenido el bot.\n";
        }

        private static Task HandleUpdateTask(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken) {
            if (update.Type != UpdateType.Message) return Task.CompletedTask;
            if (update.Message!.Type != MessageType.Text) return Task.CompletedTask;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId.ToString()}.");
            Trace.WriteLine($"Received a '{messageText}' message in chat {chatId.ToString()}.");
            LogBot += $"Received a '{messageText}' message in chat {chatId.ToString()}.\n";

            // Si esta registrado, averiguamos que nos mandaron.

            MatchMsg(messageText, botClient, chatId, cancellationToken);
            return Task.CompletedTask;
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken) {
            var errorMessage = exception switch {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode.ToString()}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Método que se encarga de obtener el mensaje que le llego al bot y manejarlo.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="botClient"></param>
        /// <param name="chatId"></param>
        /// <param name="cancellationToken"></param>
        private static async void MatchMsg(string text, ITelegramBotClient botClient, long chatId,
            CancellationToken cancellationToken) {
            var command = text.Split(" ");

            var key = command[0];

            Trace.WriteLine($"Recibida key {key}");

            switch (key) {
                case "/credencial":
                    // Verificamos que el comando este bien
                    if (command.Length != 1) {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Comando invalido, para obtener tu tarjeta digital, solo tienes que escribir: card" ,
                            cancellationToken: cancellationToken);
                        return;
                    }

                    // Verificamos que el usuario este registrado en la db
                    if (InitInfo.ObCoClientes.Any(x => x.ChatId == chatId.ToString())) {
                        var resCard = await BotCommands.Card(command, chatId, botClient, cancellationToken);

                        if (!resCard) {
                            await botClient.SendTextMessageAsync(chatId,
                                "Lo siento, No he podido entender tu mensaje \n" +
                                "recuerda que para obtener tu credencial " +
                                "solo tienes que escribir /credencial ",
                                cancellationToken: cancellationToken);
                        }
                    }
                    else {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Lo siento, tu chat no se encuentra registrado, debes registrarte primero con el comando /registro para poder usar el bot",
                            cancellationToken: cancellationToken);
                    }
                    break;

                case "/registro":
                    // auth idCliente pass
                    // Se tiene que actualizar la lista de clientes desde la GUI después de que se actualize esto.
                    var resAuth = await BotCommands.Auth(command, chatId, botClient, cancellationToken);

                    if (!resAuth) {

                        var msg = "";
                        msg += "Lo siento, No he podido entender tu mensaje, \n" +
                               "recuerda que para registrarte tienes que mandar:\n";
                        msg += "/registro id código\n";
                        msg += "Donde el id es tu identificador de registro, " +
                               "el cual puedes encontrar en tu credencial o pidiéndoselo al operador.\n";
                        msg += "y el código es un código corto que te va a proporcionar el operador.";

                        await botClient.SendTextMessageAsync(chatId,
                            msg,
                            cancellationToken: cancellationToken);
                    }
                    break;

                case "/perfil":
                    // Verificamos que el usuario este registrado en la db
                    if (command.Length != 1) {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Lo siento, No entendí tu mensaje \n" +
                                  "recuerda que para obtener tu estado, solo tienes que escribir: /perfil" ,
                            cancellationToken: cancellationToken);
                        return;
                    }

                    if (InitInfo.ObCoClientes.Any(x => x.ChatId == chatId.ToString())) {
                        var resEstado = await BotCommands.Status(command, chatId, botClient, cancellationToken);

                        if (!resEstado) {
                            await botClient.SendTextMessageAsync(chatId,
                                "lo siento, No he podido entender tu mensaje \nrecuerda que para obtener tu credencial " +
                                "solo tienes que escribir /datos ",
                                cancellationToken: cancellationToken);
                        }
                    }
                    else {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Lo siento, Tu chat no se encuentra registrado \n" +
                                  "debes registrarte primero con el comando auth y tu identificador para poder usar el bot" ,
                            cancellationToken: cancellationToken);
                    }
                    break;

                case "/debug":
                        await botClient.SendTextMessageAsync(chatId,
                            $"ChatId: {chatId.ToString()}",
                            cancellationToken: cancellationToken);
                    break;

                case "/horario":
                    var resHorario = await BotCommands.Clases(chatId, botClient, cancellationToken);

                    if (!resHorario) {
                        await botClient.SendTextMessageAsync(chatId,
                            "Lo siento, no he podido entender tu mensaje \n" +
                            "recuerda que para obtener tu horario solo tienes que escribir /horario ",
                            cancellationToken: cancellationToken);
                    }
                    break;

                default:
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Lo siento, no he podido entender tu mensaje\n" +
                              "recuerda que los comandos disponibles son:\n" +
                              "/credencial Para obtener tu credencial digital.\n" +
                              "/registro id pass Para registrarte.\n" +
                              "/perfil Para obtener más información sobre tus perfil.\n" +
                              "/horario Para conocer los horarios de tus clases.",
                        cancellationToken: cancellationToken);
                    break;
            }
        }

        /// <summary>
        /// Método que se encarga de mandar un mensaje a un usuario particular.
        /// </summary>
        /// <param name="mensaje">El mensaje a enviar.</param>
        /// <param name="id">El id del cliente al cual le debe de llegar el mensaje.</param>
        /// <returns><c>true</c> si el mensaje se mando con éxito</returns>
        public static async Task SendMessage(string mensaje, int id) {

            var cliente = InitInfo.ObCoClientes.First(x => x.Id == id);

            if (cliente.ChatId == "") {
                ShowPrettyMessages.WarningOk(
                    "El cliente seleccionado no esta registrado en el bot de telegram.",
                    "Cliente no registrado.");
                return;
            }

            var chatId = cliente.ChatId;

            try {
                await BotClient.SendTextMessageAsync(
                    chatId: chatId,
                    mensaje,
                    cancellationToken: CancellationToken.Token);
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al mandar el mensaje.");
                Log.Error($"Error: {e.Message}");
                LogBot += $"Ha ocurrido un error al mandar el mensaje. Error: {e.Message}\n";
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido a la hora de mandar el mensaje. Error: {e.Message}",
                    "Error al mandar el mensaje.");
            }
        }

        /// <summary>
        /// Método que se encarga de mandar un mensaje a todos los usuarios registrados en el bot.
        /// </summary>
        /// <param name="mensaje">El texto del mensaje a enviar.</param>
        /// <param name="idPaquete">El id del paquete para enviar el mensaje</param>
        public static async Task SendMassiveMessage(string mensaje, int idPaquete=0) {
            Log.Debug("Se ha iniciado el proceso de mandar un lote de mensajes masivos.");

            var clientesTelegram = idPaquete != 0
                ? InitInfo.ObCoClientes.Where(x => x.ChatId != "" && x.IdPaquete == idPaquete)
                : InitInfo.ObCoClientes.Where(x => x.ChatId != "");

            try {
                var count = 0;
                foreach (var cliente in clientesTelegram) {
                    await BotClient.SendTextMessageAsync(
                        chatId: cliente.ChatId,
                        mensaje,
                        cancellationToken: CancellationToken.Token);
                    LogBot += $"Se ha enviado el mensaje con éxito a {cliente.Id.ToString()} {cliente.Nombre} {cliente.ApellidoPaterno} {cliente.ChatId}";
                    count += 1;
                }

                LogBot += $"Se ha completado el proceso de mandar los mensajes masivos, Se han enviado: {count.ToString()}.";
                Log.Debug("Se ha terminado el proceso de mandar mensajes masivos.");

                ShowPrettyMessages.NiceMessageOk(
                    $"Se ha enviado el lote de mensajes con éxito, se han mandado: {count.ToString()} mensajes.",
                    "Operación exitosa");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido al mandar los mensajes masivos.");
                Log.Error($"Error: {e.Message}");
                LogBot += $"Ha ocurrido un error desconocido al mandar los mensajes masivos.\nError: {e.Message}\n";
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido a la hora de mandar los mensajes masivos. Error: {e.Message}",
                    "Error al mandar el mensaje.");
            }
        }


        /// <summary>
        /// Método que se encarga de mandar un mensaje a todos los usuarios registrados en el bot
        /// </summary>
        /// <param name="path">La ruta al recurso a subir.</param>
        /// <param name="description">Descripción de la foto a subir</param>
        /// <param name="idPaquete">El id del paquete al cual mandar el mensaje</param>
        public static async Task SendMassiveMessageMultimedia(string path, string description, int idPaquete=0) {
            Log.Debug("Se ha iniciado el proceso de mandar un lote de mensajes masivos.");

            var clientesTelegram = idPaquete != 0
                ? InitInfo.ObCoClientes.Where(x => x.ChatId != "" && x.IdPaquete == idPaquete)
                : InitInfo.ObCoClientes.Where(x => x.ChatId != "");

            await using var stream = File.OpenRead(path);

            try {
                var count = 0;
                foreach (var cliente in clientesTelegram) {
                    await BotClient.SendPhotoAsync(
                        chatId: cliente.ChatId,
                        stream,
                        caption: description,
                        cancellationToken: CancellationToken.Token);

                    LogBot += $"Se ha enviado el mensaje con éxito a {cliente.Id.ToString()} {cliente.Nombre} {cliente.ApellidoPaterno} {cliente.ChatId}";
                    count += 1;
                }

                LogBot += $"Se ha completado el proceso de mandar los mensajes masivos, Se han enviado: {count.ToString()}.";
                Log.Debug("Se ha terminado el proceso de mandar mensajes masivos.");

                ShowPrettyMessages.NiceMessageOk(
                    $"Se ha enviado el lote de mensajes con éxito, se han mandado: {count.ToString()} mensajes.",
                    "Operación exitosa");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido al mandar los mensajes masivos.");
                Log.Error($"Error: {e.Message}");
                LogBot += $"Ha ocurrido un error desconocido al mandar los mensajes masivos.\nError: {e.Message}\n";
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido a la hora de mandar los mensajes masivos. Error: {e.Message}",
                    "Error al mandar el mensaje.");
            }
        }

        /// <summary>
        /// Método que genera un nuevo password para el bot de telegram.
        /// </summary>
        /// <returns>Un <c>string</c> con el nuevo password.</returns>
        public static string GenPassword() {
            var path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            var pass = path.Substring(0, 7);
            Pass = pass;

            return pass;
        }
    }
}