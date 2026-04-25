using ChatBot.Dtos;
using ChatBot.Repositories.Interfaces;
using System.Collections.Concurrent;
using ChatBot.Repositories.Models;

namespace ChatBot.Repositories.Implementations
{
    public class ChatModelRepository : IChatModelRepository
    {
        private readonly ConcurrentDictionary<long, List<OpenApiResponse.Message>> _store = new();
        private readonly ILogger<ChatModelRepository> _logger;

        public ChatModelRepository(ILogger<ChatModelRepository> logger)
        {
            _logger = logger;
        }

        public Task AddMessageAsync(long chatId, OpenApiResponse.Message message)
        {
            _logger.LogInformation("Сохранение сообщения от чата {ChatId}: {Role}", chatId, message.Role);
            var list = _store.GetOrAdd(chatId, _ => new List<OpenApiResponse.Message>());
            lock (list)
            {
                list.Add(message);
            }
            return Task.CompletedTask;
        }

        public Task<List<OpenApiResponse.Message>> GetHistoryAsync(long chatId)
        {
            _logger.LogInformation("Загрузка истории для чата {ChatId}", chatId);
            _store.TryGetValue(chatId, out var list);
            return Task.FromResult(list == null ? new List<OpenApiResponse.Message>() : new List<OpenApiResponse.Message>(list));
        }

        public Task<ChatStats> GetStatsAsync(long chatId)
        {
            _logger.LogInformation("Подсчёт статистики для чата {ChatId}", chatId);
            _store.TryGetValue(chatId, out var list);

            var stats = new ChatStats();

            if (list != null)
            {
                foreach (var msg in list)
                {
                    if (msg.Role == "user")
                    {
                        stats.UserMessages++;
                        // Приблизительный подсчёт: 1 токен ≈ 4 символа (англ.) или ~1.5 (рус.)
                        // Используем усреднённое: 1 токен ≈ 3 символа
                        stats.EstimatedUserTokens += (int)Math.Ceiling(msg.Content.Length / 3.0);
                    }
                    else if (msg.Role == "assistant")
                    {
                        stats.AssistantMessages++;
                        stats.EstimatedAssistantTokens += (int)Math.Ceiling(msg.Content.Length / 3.0);
                    }
                }
            }

            return Task.FromResult(stats);
        }

        public Task ClearHistoryAsync(long chatId)
        {
            _logger.LogInformation("Очистка истории для чата {ChatId}", chatId);
            _store.TryRemove(chatId, out _);
            return Task.CompletedTask;
        }

        public Task<bool> RemoveLastMessageAsync(long chatId)
        {
            _logger.LogInformation("Удаление последнего сообщения для чата {ChatId}", chatId);
            _store.TryGetValue(chatId, out var list);

            if (list != null && list.Count > 0)
            {
                lock (list)
                {
                    if (list.Count > 0)
                    {
                        list.RemoveAt(list.Count - 1);
                        return Task.FromResult(true);
                    }
                }
            }

            return Task.FromResult(false);
        }
    }
}