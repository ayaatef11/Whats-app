
namespace WhatsApp.Models
{
    public class Message
    {
        public string RecipientUsername { get; set; }
        public User Sender { get; set; }
        public string SenderUsername { get; set; }
        public string Content { get; set; }
        public int Id { get; set; }
        public int SenderId { get; set; }
        public User Recipient { get; set; }
        public int RecipientId { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    }                 
}                       
