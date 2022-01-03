using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GymCastillo.Model.Database;
using GymCastillo.Model.Init;
using log4net;
using MySqlConnector;
using Telegram.Bot;

namespace GymCastillo.Model.Bot {
    /// <summary>
    /// Clase que se encarga de ejecutar y verificar los comandos del bot.
    /// </summary>
    public static class BotCommands {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

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

                if (InitInfo.ObCoClientes.Any(x => x.Id == id)) {
                    if (pass == Bot.Pass) {
                        // Damos de alta su chat id
                        await using var connection = new MySqlConnection(GetInitData.ConnString);
                        await connection.OpenAsync();
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
                        "No he podido encontrar tu id en la base de datos, verificalo.",
                        cancellationToken: cancellationToken);
                    return true;
                }

                return false;
            }
            catch (Exception e) {
                Log.Debug("BOT: Comando invalido para auth");
                Log.Debug($"BOT_ERROR: {e.Message}");
                return false;
            }
        }

        public static async Task<bool> Status(string[] args, long chatId, ITelegramBotClient botClient,
            CancellationToken cancellationToken) {

            try {

            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }

            return false;
        }
    }
}