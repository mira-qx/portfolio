using ChatBot.Commands;
using ChatBot.Dtos;
using Telegram.Bot;

public class StartCommand : IBotCommand 
{ 
    public string Trigger => "/start"; 
    public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId) 
    { 
        await bot.SendTextMessageAsync(chatId, "Привет! Я OpenAI-бот. Отправь сообщение -- я передам его сторонней модели и верну ответ.\n/help для списка команд."); 
    } 
}
