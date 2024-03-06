using GraphqlLeetcode.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GraphqlLeetcode.Services.BotManagerServices;

internal partial class BotManager
{
    async Task RegisterUserAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationtoken)
    {
        List<TelegramUser> users = await userRepository.ReadFromFile();
        TelegramUser user = new TelegramUser()
        {
            Id = update?.Message?.From?.Id,
            UserName = update?.Message?.From?.Username,
            LeetcodeUserName = update?.Message?.Text
        };
        users.Add(user);
        await userRepository.WriteToFileAsync(users);

        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Registratsiya  muvaffaqqiyatli amalga oshirildi",
            cancellationToken: cancellationtoken
            );
    }
}