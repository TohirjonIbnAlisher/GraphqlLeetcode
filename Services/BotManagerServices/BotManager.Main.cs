
using GraphqlLeetcode.Repositories.Brokers;
using GraphqlLeetcode.Repositories.UserRepositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GraphqlLeetcode.Services.BotManagerServices;
    internal partial class BotManager
    {
        IUserRepository userRepository = new UserRepository();
        ILeetCodeBroker leetCodeBroker = new LeetcodeBroker();
        int messageID = 0;
        public async Task Menu(ITelegramBotClient botClient, Message sendMessage, CancellationToken token)
        {
            Message message = await botClient.SendTextMessageAsync(
                chatId: sendMessage.Chat.Id,
                text: "Jarayonni tanlang",
                parseMode: ParseMode.Markdown,
                cancellationToken: token,
                replyMarkup: new InlineKeyboardMarkup(
                    new InlineKeyboardButton[][]
                    {
                            new InlineKeyboardButton[]
                            {
                                InlineKeyboardButton.WithCallbackData("LeetCode shaxsiy natijalarini ko'rish", "info"),
                                InlineKeyboardButton.WithCallbackData("Kunlik vazifani ko'rish", "task"),
                            },
                            new InlineKeyboardButton[]
                            {
                                InlineKeyboardButton.WithCallbackData("Ma'lumotlarni yangilash", "update")
                            }
                    }));
            messageID = message.MessageId;
        }
        public async Task SendUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.CallbackQuery is not null)
            {
                await CallBackQueryAsync(botClient, update, cancellationToken);
            }

            else if (update.Message.Type is MessageType.Text)
            {
                await MessageTextAsync(botClient, update, cancellationToken);
            }
        }

        async Task DeleteMessageAsync(ITelegramBotClient client, long chatId, CancellationToken token)
        {
            if (messageID != 0)
            {
                await client.DeleteMessageAsync(
                    chatId: chatId,
                    messageId: messageID,
                    cancellationToken: token);
            }
        }
    }

