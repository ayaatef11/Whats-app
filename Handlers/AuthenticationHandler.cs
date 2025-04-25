using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WhatsApp.Handlers
{
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var result = await HandleAuthenticateAsync();
            if (result.Succeeded)
            {
                return result;
            }

            var guid = Guid.NewGuid().ToString();
            var claim = new Claim("UserId", guid);

            ClaimsIdentity identity = new ClaimsIdentity(new[] { claim }, "Default");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            await Context.SignInAsync("Default", claimsPrincipal);
            AuthenticationTicket ticket = new AuthenticationTicket(claimsPrincipal, "Default");
            return AuthenticateResult.Success(ticket);
        }
    }
}
