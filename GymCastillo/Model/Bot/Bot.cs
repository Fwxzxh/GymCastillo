using System;
using System.Threading;
using System.Threading.Tasks;
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
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Indica si el bot esta corriendo o no.
        /// </summary>
        public static bool Estado { get; set; }

        /// <summary>
        /// instancia del bot
        /// </summary>
        private TelegramBotClient BotClient { init; get; }

        /// <summary>
        /// El token para cancelar el bot.
        /// </summary>
        private CancellationTokenSource CancellationToken { init; get; }

        // Funcionamiento bot
        // <- Mandar ticket de venta si esta registrado.
        // <- Mandar updates de horarios y cosas asi.
        // <- Avisarte cuando tu paquete esta por acabar.
        // <-> Mandar la credencial Digital o pedirla: card
        // -> Registrarte y autenticarte: auth {idCliente} {passRandom}
        // -> Pedir un estado de tu usuario: estado
        // - Limitar la cantidad

        /// <summary>
        /// Constructor de la
        /// </summary>
        /// <param name="apiKey"></param>
        public Bot(string apiKey) {

            if (Estado) {
                Log.Info("Se ha intentado crear dos veces el bot aaaaaaaa");
                return;
            }

            BotClient = new TelegramBotClient(apiKey);

            CancellationToken = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions() {
                AllowedUpdates = { }
            };

            BotClient.StartReceiving(
                HandleUpdateTask,
                HandleErrorAsync,
                receiverOptions,
                CancellationToken.Token);

            Log.Info("Se ha iniciado el bot.");

            Estado = true;
        }

        public void StopBot() {
            CancellationToken.Cancel();
            Estado = false;
            Log.Info("Se ha detenido el bot.");
        }

        private async Task HandleUpdateTask(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
            if (update.Type != UpdateType.Message) return;
            if (update.Message!.Type != MessageType.Text) return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId.ToString()}.");

            // Echo received message text
            var sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + messageText,
                cancellationToken: cancellationToken);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
            var errorMessage = exception switch {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode.ToString()}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}