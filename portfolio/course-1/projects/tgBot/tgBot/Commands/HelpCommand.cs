using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScheduleBot.Commands;

public class HelpCommand : ICommand
{
    public async Task ExecuteAsync(Update update, ITelegramBotClient botClient, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        string text = "Доступные команды:\n" +
         "/start - запуск бота\n" +
         "/help - список команд\n" +
         "/week [группа] - расписание на неделю\n" +
         "/today [группа] - расписание на сегодня\n";

        await botClient.SendTextMessageAsync(chatId, text, cancellationToken: ct);
    }
}
