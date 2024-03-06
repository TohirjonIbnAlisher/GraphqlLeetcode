using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using GraphqlLeetcode.Models;
using Microsoft.VisualBasic;
using System.Threading;

namespace GraphqlLeetcode.Services.BotManagerServices;

internal partial class BotManager
{
    async Task CallBackQueryAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        await DeleteMessageAsync(client, update.CallbackQuery.Message.Chat.Id, cancellationToken);
        if (update.CallbackQuery.Data is "info")
        {
            var users = await userRepository.ReadFromFile();
            var currentLeetCodeUsername = users.Where(user => user.Id == update?.CallbackQuery?.From?.Id).First().LeetcodeUserName;

            List<Submission> submissions = await leetCodeBroker.GetTotalSolvedProblemsCountAsync(currentLeetCodeUsername);

            var markdownResult = $@"| difficulty | Count | Submissions |
| ------------ | ------ | ---------- |
|      {submissions[0].Difficulty}      |   {submissions[0].Count}   |      {submissions[0].Submissions}     |
|      {submissions[1].Difficulty}      |   {submissions[0].Count}   |      {submissions[0].Submissions}     |
|      {submissions[2].Difficulty}      |   {submissions[0].Count}   |      {submissions[0].Submissions}     |
|      {submissions[3].Difficulty}      |   {submissions[0].Count}   |      {submissions[0].Submissions}     |";

            Message message = await client.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: markdownResult,
                replyMarkup: new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("⬅️ Asosiy menu", "menu")),
                parseMode: ParseMode.Markdown);

            messageID = message.MessageId;
        }

        else if (update.CallbackQuery.Data is "menu")
        {
            await Menu(client, update.CallbackQuery.Message, cancellationToken);
        }
        else if (update.CallbackQuery.Data is "task")
        {
            await CommonButtonsAsync(client, update, cancellationToken);
            //await CommonAsync(client, update, cancellationToken);
        }
        else if (update.CallbackQuery.Data is "update")
        {
            await UpdateInfoAsync(client, update.CallbackQuery.Message, cancellationToken);
        }

       
    }

    async Task CommonButtonsAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        await DeleteMessageAsync(client, update.CallbackQuery.Message.Chat.Id, token);
        
        DailyProblem dailyProblem = await leetCodeBroker.GetDailyProblemAsync();
        Message returnMessage = await client.SendTextMessageAsync(
            chatId: update.CallbackQuery.Message.Chat.Id,
            text: $"Title : {dailyProblem.Title}\n" +
                  $"Difficulty : {dailyProblem.Difficulty}\n" +
                  $"Date : {dailyProblem.Date}\n" +
                  $"Tags : {dailyProblem.Tags}\n" +
                  $"Link : <a href=\"https://leetcode.com{dailyProblem.Link}\">Visit</a>",
            parseMode : ParseMode.Html
             );
    }

    async Task UpdateInfoAsync(ITelegramBotClient client, Message message, CancellationToken token)
    {
        Message updateMessage = await client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Leetcode ma'lumotlarini yangilash",
            replyMarkup: new ReplyKeyboardMarkup(
                new KeyboardButton[]
                {
                        new KeyboardButton("⬅️ Ortga")
                })
            { OneTimeKeyboard = true, ResizeKeyboard = true });

    }

    async Task CommonAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        if (update.CallbackQuery.Data is "easy")
        {
            Message mess = await client.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: "2",
                replyMarkup: new ReplyKeyboardMarkup(
                    new KeyboardButton[]
                    {
                            new KeyboardButton("⬅️ Ortga")
                    }));
            messageID = mess.MessageId;
        }
        else if (update.CallbackQuery.Data is "medium")
        {
            Message mess1 = await client.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: "3",
                replyMarkup: new ReplyKeyboardMarkup(
                    new KeyboardButton[][]
                    {
                            new KeyboardButton[]
                            {
                                new KeyboardButton("⬅️ Ortga")
                            }
                    }));
            messageID = mess1.MessageId;
        }
        else if (update.CallbackQuery.Data is "hard")
        {
            Message mess3 = await client.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: "4",
                replyMarkup: new ReplyKeyboardMarkup(
                    new KeyboardButton[][]
                    {
                            new KeyboardButton[]
                            {
                                new KeyboardButton("⬅️ Ortga")
                            }
                    }));
            messageID = mess3.MessageId;
        }

    }
}