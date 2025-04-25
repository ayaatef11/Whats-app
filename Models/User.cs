namespace WhatsApp.Models
{
    public record User(string UserId, string UserName)
    {
        public object KnownAs { get; internal set; }
    }
}
