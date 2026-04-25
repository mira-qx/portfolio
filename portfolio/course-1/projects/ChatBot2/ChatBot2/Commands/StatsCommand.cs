using ChatBot.Dtos;
using ChatBot.Repositories.Interfaces;
using Telegram.Bot;

namespace ChatBot.Commands
{
    public class StatsCommand : IBotCommand
    {
        private readonly IChatModelRepository _chatModelRepository;

        public StatsCommand(IChatModelRepository chatModelRepository)
        {
            _chatModelRepository = chatModelRepository;
        }

        public string Trigger => "/stats";

        public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
        {
            var stats = await _chatModelRepository.GetStatsAsync(chatId);

            var message = $"Статистика чат\n\n" +
            $"Сообщений от вас: {stats.UserMessages}\n" +
            $"Ответов бота: {stats.AssistantMessages}\n" +
            $"Всего сообщений: {stats.TotalMessages}\n\n" +
            $"Токенов (приблизительно):\n" +
            $" - От вас: ~{stats.EstimatedUserTokens}\n" +
            $" - От бота: ~{stats.EstimatedAssistantTokens}\n" +
            $" - Всего: ~{stats.EstimatedTotalTokens}";

            await bot.SendTextMessageAsync(chatId, message);
        }
    }
}