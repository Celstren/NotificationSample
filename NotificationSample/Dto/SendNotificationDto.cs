using Newtonsoft.Json;

namespace NotificationSample.Dto
{
    public class SendNotificationDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
