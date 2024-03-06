using GraphqlLeetcode.Repositories.Brokers;
using GraphqlLeetcode.Services.BotManagerServices;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GraphqlLeetcode;

internal class Program
{
    static DateTime dateTime = DateTime.Now;
    static ReceiverOptions receiverOptions = new ReceiverOptions()
    {
        AllowedUpdates = { }
    };

    static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    const string BOT_TOKEN = "6899842772:AAH4cdFppexrcB8eKvBYPfvA159gizrtYd0";

    static ITelegramBotClient botClient = new TelegramBotClient(BOT_TOKEN);
    static async Task Main(string[] args)
    {
        ILeetCodeBroker leetCodeBroker = new LeetcodeBroker();
        leetCodeBroker.GetDailyProblemAsync();
        //botClient.StartReceiving(
        //    updateHandler: UpdateHandlerAsync,
        //    receiverOptions: receiverOptions,
        //    cancellationToken: cancellationTokenSource.Token,
        //    pollingErrorHandler: ErrorHandlerAsync);

        Console.ReadKey();
    }

    static Task ErrorHandlerAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    static async Task UpdateHandlerAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        //;
        if (update == null)
        {
            return;
        }
        else
        {
            BotManager botManager = new BotManager();
            await botManager.SendUpdateAsync(client, update, token);
        }
    }
}