using ExpenseTracker.Bot.Options;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExpenseTracker.Bot.Services;
public class TelegramBotService
{
    public readonly TelegramBotClient _bot;
    public TelegramBotService(IOptions<ExpensesBotOptions> options)
    {
        _bot = new TelegramBotClient(options.Value.BotToken);

    }

    public async Task SendMessage(long chatId,string message,IReplyMarkup? reply = null)
    {
        await _bot.SendTextMessageAsync(chatId,message,replyMarkup:reply);
    }

    public async Task SendPhoto(long chatId,string message, Stream image,IReplyMarkup? reply = null)
    {
        await _bot.SendPhotoAsync(chatId, new InputOnlineFile(image), message,replyMarkup:reply);
    }

    public async Task EditMessageButtons(long chatId, int messageId, InlineKeyboardMarkup reply)
    {
        await _bot.EditMessageReplyMarkupAsync(chatId, messageId, replyMarkup: reply);
    }

    public ReplyKeyboardMarkup GetKeyboardButtons(List<string> buttonsText)
    {
        var buttons = new KeyboardButton[buttonsText.Count][];

        for (int i = 0; i < buttonsText.Count; i++)
        {
            buttons[i] = new KeyboardButton[] { new KeyboardButton(buttonsText[i]) };
        }

        return new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true };
    }
    public InlineKeyboardMarkup GetInlineKeyboard(List<string> buttonsText)
    {
        var buttons = new InlineKeyboardButton[buttonsText.Count][];

        for (var i = 0; i < buttonsText.Count; i++)
        {
            buttons[i] = new[] { InlineKeyboardButton.WithCallbackData(text: buttonsText[i], callbackData: buttonsText[i]) };
        }
        return new InlineKeyboardMarkup(buttons);
    }
}
