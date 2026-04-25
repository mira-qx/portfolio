using ChatBot.Dtos;
using ChatBot.Repositories.Interfaces;
using ChatBot.Repositories.Models;
using Telegram.Bot;

namespace ChatBot.Commands
{
    public class TelegramUpdateProcessor
    {
        private readonly IEnumerable<IBotCommand> _commands;
        private readonly ITelegramBotClient _botClient;
        private readonly IChatApiClient _chatClient;
        private readonly IChatModelRepository _chatModelRepository;

        public TelegramUpdateProcessor(
            IEnumerable<IBotCommand> commands,
            ITelegramBotClient botClient,
            IChatApiClient chatClient,
            IChatModelRepository chatModelRepository)
        {
            _commands = commands;
            _botClient = botClient;
            _chatClient = chatClient;
            _chatModelRepository = chatModelRepository;
        }

        public async Task HandleAsync(TelegramUpdate update)
        {
            if (update.Message == null) return;

            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text?.Trim();

            if (string.IsNullOrEmpty(text)) return;

            if (text.StartsWith("/"))
            {
                var cmd = text.Split(' ', 2)[0];
                var command = _commands.FirstOrDefault(c =>
                    c.Trigger.Equals(cmd, StringComparison.OrdinalIgnoreCase));

                if (command != null)
                {
                    await command.ExecuteAsync(update, _botClient, chatId);
                    return;
                }
                else
                {
                    await _botClient.SendTextMessageAsync(
                        chatId,
                        "Неизвестная команда. Используйте /help для списка команд.");
                    return;
                }
            }

            await HandleRegularMessageAsync(chatId, text);
        }

        private async Task HandleRegularMessageAsync(long chatId, string userMessage)
        {
            try
            {
                var userMsg = new OpenApiResponse.Message
                {
                    Role = "user",
                    Content = userMessage
                };
                await _chatModelRepository.AddMessageAsync(chatId, userMsg);

                var history = await _chatModelRepository.GetHistoryAsync(chatId);
                var response = await _chatClient.SendMessageAsync(userMessage, history);
                var assistantMsg = new OpenApiResponse.Message
                {
                    Role = "assistant",
                    Content = response
                };
                await _chatModelRepository.AddMessageAsync(chatId, assistantMsg);

                await _botClient.SendTextMessageAsync(chatId, response);
            }
            catch (Exception ex)
            {
                await _botClient.SendTextMessageAsync(
                    chatId,
                    "Произошла ошибка при обработке сообщения. Попробуйте позже.");

            }
        }
    }
}