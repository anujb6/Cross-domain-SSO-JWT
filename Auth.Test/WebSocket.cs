using Microsoft.AspNetCore.SignalR;

namespace Auth.Test
{
    public class WebSocket : Hub
    {
        private static readonly Stack<string> _stack = new();

        public void AddToken(string token)
        {
            _stack.Push(token);
        }

        public async Task SendTokenToClient()
        {
            var token = _stack.Pop();
            await Clients.All.SendAsync("ReceiveToken", token);
        }
    }
}
