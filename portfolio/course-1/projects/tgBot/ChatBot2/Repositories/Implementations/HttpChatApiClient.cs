using ChatBot.Repositories.Interfaces;
using ChatBot.Repositories.Models;
using ChatBot.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace ChatBot.Repositories.Implementations
{
    public class HttpChatApiClient : IChatApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ChatApiSettings _chatSettings;

        public HttpChatApiClient(HttpClient httpClient, IOptions<ChatApiSettings> chatOptions)
        {
            _chatSettings = chatOptions.Value;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(_chatSettings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _chatSettings.ApiKey);
        }

        public async Task<string> SendMessageAsync(string userMessage, IEnumerable<OpenApiResponse.Message> history)
        {
            var payload = new OpenApiRequest()
            {
                Model = _chatSettings.DefaultModel,
                Messages = history.ToList(),
                MaxTokens = 1000
            };

            var response = await _httpClient.PostAsJsonAsync("", payload);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadFromJsonAsync<OpenApiResponse?>();
            if (body?.Choices != null && body.Choices.Length > 0)
            {
                return body.Choices[0].Message.Content;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}