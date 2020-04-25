using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hub
{
    public static class ConnectionHub
    {
        private static List<WebSocket> _webSockets = new List<WebSocket>();

        public static async Task SendToAll(string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            var result = new WebSocketReceiveResult(message.Length, WebSocketMessageType.Text, true);

            foreach (var currentSocket in _webSockets)
            {
                await currentSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
            }
        }

        public static async Task Echo(WebSocket webSocket)
        {
            _webSockets.Add(webSocket);
            byte[] buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            _webSockets.Remove(webSocket);
        }
    }
}
