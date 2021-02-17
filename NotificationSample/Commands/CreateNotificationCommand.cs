using NotificationSample.Dto;
using MediatR;
using Newtonsoft.Json;

namespace NotificationSample.Commands
{
    public class CreateNotificationCommand : IRequest<SendNotificationDto>
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
