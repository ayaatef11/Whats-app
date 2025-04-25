namespace WhatsApp.Extensions
{
    public static class HttpContextExtensions
    {
        public static string UserId(this HttpContext ctx)
        {
            return ctx.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
        }
    }
}
