using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using WhatsApp.Handlers;
using WhatsApp.Hubs;
using WhatsApp.Models;
using WhatsApp.Providers;
using WhatsApp.Services;

var builder = WebApplication.CreateBuilder(args);

// ≈÷«›… «·Œœ„« 
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddSingleton<ChatRegistry>();
builder.Services.AddSingleton<RoomManager>(); 
builder.Services.AddSignalR(o =>
{
    o.AddFilter<AuthHubFilter>();
});

// ≈÷«›… „“Êœ „⁄—› «·„” Œœ„ (UserIdProvider)
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
builder.Services.AddSingleton<RoomManager>();
builder.Services.AddSingleton<State>();

//  ⁄—Ì› „Œÿÿ«  «· ÊÀÌﬁ
// «·„Œÿÿ «·√Ê·: CustomCookieScheme (··ﬂÊﬂÌ“)
const string CustomCookieScheme = nameof(CustomCookieScheme);
const string CustomTokenScheme = nameof(CustomTokenScheme);

//builder.Services.AddAuthentication()
//    .AddScheme<AuthenticationSchemeOptions, CustomCookie>(CustomCookieScheme, _ => { })
//    .AddJwtBearer(CustomTokenScheme, o =>
//    {
//        o.Events = new()
//        {
//            OnMessageReceived = (context) =>
//            {
//                var path = context.HttpContext.Request.Path;
//                if (path.StartsWithSegments("/protected") || path.StartsWithSegments("/token"))
//                {
//                    var accessToken = context.Request.Query["access_token"];
//                    if (!string.IsNullOrWhiteSpace(accessToken))
//                    {
//                        var claims = new Claim[]
//                        {
//                            new("user_id", accessToken),
//                            new("token", "token_claim"),
//                        };
//                        var identity = new ClaimsIdentity(claims, CustomTokenScheme);
//                        context.Principal = new ClaimsPrincipal(identity);
//                        context.Success();
//                    }
//                }
//                return Task.CompletedTask;
//            }
//        };
//    });

// ≈⁄œ«œ ”Ì«”«  «· ›ÊÌ÷ (Authorization Policies)
builder.Services.AddAuthorization(c =>
{
    c.AddPolicy("Cookie", pb => pb
        .AddAuthenticationSchemes(CustomCookieScheme)
        .RequireAuthenticatedUser());

    c.AddPolicy("Token", pb => pb
        .AddAuthenticationSchemes(CustomTokenScheme)
        .RequireClaim("token")
        .RequireAuthenticatedUser());
});

// ≈⁄œ«œ «·‹ Cookie Authentication («Œ Ì«—Ì ≈–« ﬂ‰   ” Œœ„Â)
//builder.Services.TryAddEnumerable(
//    ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());
//builder.Services.AddAuthentication("Default")
//    .AddScheme<CookieAuthenticationOptions, AuthenticationHandler>("Default", options =>
//    {
//        options.Cookie.HttpOnly = false;
//        options.Cookie.Name = "AuthCookie";
//    });

var app = builder.Build();

// «·‹ Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

//  ⁄ÌÌ‰ ‰ﬁ«ÿ «·‰Â«Ì… (Endpoints)
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ProtectedHub>("/protected");

    endpoints.Map("/get-cookie", ctx =>
    {
        ctx.Response.StatusCode = 200;
        ctx.Response.Cookies.Append("signalr-auth-cookie", Guid.NewGuid().ToString(), new()
        {
            Expires = DateTimeOffset.UtcNow.AddSeconds(30)
        });
        return ctx.Response.WriteAsync("");
    });

    endpoints.Map("/token", ctx =>
    {
        ctx.Response.StatusCode = 200;
        return ctx.Response.WriteAsync(ctx.User?.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value);
    }).RequireAuthorization("Token");

    endpoints.Map("/cookie", ctx =>
    {
        ctx.Response.StatusCode = 200;
        return ctx.Response.WriteAsync(ctx.User?.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value);
    }).RequireAuthorization("Cookie");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();
});



app.Run();

//  ⁄—Ì› ›∆… CustomCookie ·· ÊÀÌﬁ »«” Œœ«„ «·ﬂÊﬂÌ“
public class CustomCookie : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public CustomCookie(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    ) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.Request.Cookies.TryGetValue("signalr-auth-cookie", out var cookie))
        {
            var claims = new Claim[]
            {
                new("user_id", cookie),
                new("cookie", "cookie_claim"),
            };
            var identity = new ClaimsIdentity(claims, /*CustomCookieScheme*/"");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, new(), /*CustomCookieScheme*/"");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        return Task.FromResult(AuthenticateResult.Fail("signalr-auth-cookie not found"));
    }
}