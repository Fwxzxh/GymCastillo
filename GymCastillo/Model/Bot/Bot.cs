using System;
using System.Diagnostics;
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
        public static bool Estado { get; set; }

        /// <summary>
        /// La contraseña para autenticar al usuario
        /// </summary>
        // public static string Pass { get; set; }
        public static string Pass = "1234";

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

            if (Estado) {
                Log.Warn("Se ha intentado crear dos veces el bot ");
                return;
            }

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

            Estado = true;
        }

        public void StopBot() {
            CancellationToken.Cancel();
            Estado = false;
            Log.Info("Se ha detenido el bot.");
        }

        private async Task HandleUpdateTask(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken) {
            if (update.Type != UpdateType.Message) return;
            if (update.Message!.Type != MessageType.Text) return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId.ToString()}.");
            Trace.WriteLine($"Received a '{messageText}' message in chat {chatId.ToString()}.");

            // Si esta registrado, averiguamos que nos mandaron.

            await MatchMsg(messageText, botClient, chatId, cancellationToken);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
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
        /// <param name="msg"></param>
        /// <param name="botClient"></param>
        /// <param name="chatId"></param>
        /// <param name="cancellationToken"></param>
        private static async Task MatchMsg(string msg, ITelegramBotClient botClient, long chatId,
            CancellationToken cancellationToken) {
            var command = msg.Split(" ");

            var key = command[0];

            Trace.WriteLine($"Recibida key {key}");

            switch (key) {
                case "/card":
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
                                "No he podido entender tu mensaje \nrecuerda que para obtener tu credencial " +
                                "solo tienes que escribir /card ",
                                cancellationToken: cancellationToken);
                        }
                    }
                    else {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Tu chat no se encuentra registrado, debes registrarte primero con el comando auth para poder usar el bot",
                            cancellationToken: cancellationToken);
                    }
                    break;

                case "/auth":
                    // auth idCliente pass
                    // Se tiene que actualizar la lista de clientes desde la GUI después de que se actualize esto.
                    var resAuth = await BotCommands.Auth(command, chatId, botClient, cancellationToken);

                    if (!resAuth) {
                        await botClient.SendTextMessageAsync(chatId,
                            "No he podido entender tu mensaje, recuerda que para registrarte tienes que mandar:",
                            cancellationToken: cancellationToken);
                        await botClient.SendTextMessageAsync(chatId,
                            "auth id código",
                            cancellationToken: cancellationToken);
                        await botClient.SendTextMessageAsync(chatId,
                            "Donde el id es tu identificador de registro, " +
                            "el cual puedes encontrar en tu credencial o preguntándole al operador.",
                            cancellationToken: cancellationToken);
                        await botClient.SendTextMessageAsync(chatId,
                            "y el código es un código corto que te va a proporcionar el operador.",
                            cancellationToken: cancellationToken);
                    }
                    break;

                case "/estado":
                    // Verificamos que el usuario este registrado en la db
                    if (command.Length != 1) {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Comando invalido, para obtener tu estado, solo tienes que escribir: estado" ,
                            cancellationToken: cancellationToken);
                        return;
                    }

                    if (InitInfo.ObCoClientes.Any(x => x.ChatId == chatId.ToString())) {
                        var resEstado = await BotCommands.Status(command, chatId, botClient, cancellationToken);

                        if (resEstado) {
                            await botClient.SendTextMessageAsync(chatId,
                                "No he podido entender tu mensaje \nrecuerda que para obtener tu credencial " +
                                "solo tienes que escribir /estado ",
                                cancellationToken: cancellationToken);
                        }
                    }
                    else {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Tu chat no se encuentra registrado, " +
                                  "debes registrarte primero con el comando auth para poder usar el bot" ,
                            cancellationToken: cancellationToken);
                    }
                    break;

                case "/debug":
                        await botClient.SendTextMessageAsync(chatId,
                            $"ChatId: {chatId.ToString()}",
                            cancellationToken: cancellationToken);
                    break;

                default:
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "No se encontró esta opción, recuerda que los comandos disponibles son:\n" +
                              "/card \n/auth id código \n/estado",
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
        public static async Task<bool> SendMessage(string mensaje, int id) {

            var cliente = InitInfo.ObCoClientes.First(x => x.Id == id);

            if (cliente.ChatId == "") {
                ShowPrettyMessages.WarningOk(
                    "El cliente seleccionado no esta registrado en el bot de telegram.",
                    "Cliente no registrado.");
                return false;
            }

            var chatId = cliente.ChatId;

            try {
                await BotClient.SendTextMessageAsync(
                    chatId: chatId,
                    mensaje,
                    cancellationToken: CancellationToken.Token);

                return true;
            }
            catch (Exception e) {
                Log.Error("ha ocurrido un error al mandar el mensaje.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido a la hora de mandar el mensaje. Error: {e.Message}",
                    "Error al mandar el mensaje.");
                return false;
            }
        }
    }
}