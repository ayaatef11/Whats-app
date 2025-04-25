using Microsoft.AspNetCore.SignalR;

namespace WhatsApp.Hubs
{
    public class AuthHubFilter : IHubFilter
    {
        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext invocationContext,
            Func<HubInvocationContext, ValueTask<object?>> next)
        {
            if (!IsAuthorized(invocationContext))
            {
                throw new HubException("غير مصرح بالوصول");
            }

            return await next(invocationContext);
        }

        private bool IsAuthorized(HubInvocationContext context)
        {
            // منطق التحقق من الصلاحيات
            return context.Context.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}
