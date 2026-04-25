using ChatBot.Dtos;
using ChatBot.Repositories.Interfaces;
using Telegram.Bot;

namespace ChatBot.Commands
{
    public class HistoryCommand : IBotCommand
    {
        private readonly IChatModelRepository _chatModelRepository;

        public HistoryCommand(IChatModelRepository chatModelRepository)
        {
            _chatModelRepository = chatModelRepository;
        }

        public string Trigger => "/history";

        public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
        {
            var text = update.Message.Text.Trim();
            var parts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int count = 5;

            if (parts.Length > 1 && int.TryParse(parts[1], out var parsed))
            {
                count = Math.Clamp(parsed, 1, 20);
            }

            var history = await _chatModelRepository.GetHistoryAsync(chatId);

            if (history.Count == 0)
            {
                await bot.SendTextMessageAsync(chatId, "История пуста.");
                return;
            }

            var lastMessages = history.TakeLast(count);
            var result = $"Последние {lastMessages.Count()} сообщений:\n\n";

            foreach (var msg in lastMessages)
            {
                var role = msg.Role switch
                {
                    "user" => "Вы",
                    "assistant" => "Бот",
                    "system" => "Система",
                    _ => msg.Role
                };
                var preview = msg.Content.Length > 100
                    ? msg.Content[..100] + "..."
                    : msg.Content;
                result += $"[{role}]: {preview}\n\n";
            }

            await bot.SendTextMessageAsync(chatId, result);
        }
    }
}