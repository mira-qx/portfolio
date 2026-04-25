using ChatBot.Commands;
using ChatBot.Dtos;
using ChatBot.Repositories.Interfaces;
using Telegram.Bot;

namespace ChatBot.Commands
{
    public class ClearCommand : IBotCommand
    {
        private readonly IChatModelRepository _chatModelRepository;

        public ClearCommand(IChatModelRepository chatModelRepository)
        {
            _chatModelRepository = chatModelRepository;
        }

        public string Trigger => "/clear";

        public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
        {
            await _chatModelRepository.ClearHistoryAsync(chatId);

            var message = "История очищена\n\n" +
            "Начните новый диалог -- я буду отвечать с чистого листа.";

            await bot.SendTextMessageAsync(chatId, message);
        }
    }
}