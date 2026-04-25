using ChatBot.Repositories.Interfaces;
using ChatBot.Repositories.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatTestController(IChatApiClient chatApiClient) : ControllerBase
    {
        [HttpPost("test")]
        public async Task<IActionResult> Test([FromBody] ChatTestRequest request)
        {
            var answer = await chatApiClient.SendMessageAsync(
            request.Message,
            new List<OpenApiResponse.Message>
            { 
                new OpenApiResponse.Message
            {
                Role = "user",
                Content = request.Message
            }
            });
            return Ok(new { answer });
        }
    }

    public record ChatTestRequest(string Message);

}