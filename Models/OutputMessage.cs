namespace WhatsApp.Models
{
    public record OutputMessage(
            string Message,
            string UserName,
            string Room,
            DateTimeOffset SentAt
        );
     

       
        
            //public OutputMessage Output => new(Message, User.UserName, Room, SentAt);
}

