// 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

namespace ChatBot.Settings
{
    public class ChatApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; } = string.Empty;
        public string DefaultModel { get; } = "openrouter/hunter-alpha";

    }
}