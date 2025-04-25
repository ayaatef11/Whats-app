namespace WhatsApp.Models
{
    public record UserMessage(
           User User,
           string Message,
           string Room,
           DateTimeOffset SentAt,
           OutputMessage? Output=null
       );
}
