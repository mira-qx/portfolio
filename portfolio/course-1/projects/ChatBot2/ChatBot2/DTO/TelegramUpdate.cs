using System.ComponentModel.DataAnnotations;

namespace ChatBot.Dtos
{
    public class TelegramUpdate
    {
        [Required]
        public int UpdateId { get; set; }
        public TelegramMessage? Message { get; set; }
        public TelegramMessage? EditedMessage { get; set; }
    }
}