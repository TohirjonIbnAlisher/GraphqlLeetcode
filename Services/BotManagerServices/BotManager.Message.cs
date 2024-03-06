using GraphqlLeetcode.Repositories.UserRepositories;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace GraphqlLeetcode.Services.BotManagerServices;

internal partial class BotManager
{
    async Task MessageTextAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        string message = update.Message.Text;

        if (message == "/start")
        {
            await OnSendStartMessageAsync(botClient, update, cancellationToken);
        }

        else
        {
            await RegisterUserAsync(botClient, update, cancellationToken);
            await Menu(botClient, update.Message, cancellationToken);
        }
    }

    private async Task OnSendStartMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (userRepository.ReadFromFile().Result.Find(user => user.Id == update.Message.Chat.Id) is null)
        {
            string message = "Ro'yxatdan o'tish uchun Leetcode usernameni kiriting";
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: message,
                cancellationToken: cancellationToken);
        }
        else
        {
            await Menu(botClient, update.Message, cancellationToken);
        }
    }
}