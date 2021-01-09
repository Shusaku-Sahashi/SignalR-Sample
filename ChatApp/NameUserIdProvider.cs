using Microsoft.AspNetCore.SignalR;

namespace ChatApp
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
            => connection.User?.Identity?.Name;
    }
}