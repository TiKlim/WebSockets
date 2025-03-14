using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace AvaloniaApplication2.Data
{
    public class ClientWebSocketHandler : IDisposable
    {
        public delegate void ClientWebSocketReceiveMessage(string message);
        public event ClientWebSocketReceiveMessage? OnReceiveMessage;

        private readonly Uri uri = new Uri("ws://217.114.2.102:8088/reviews");
        private ClientWebSocket webSocket = new ();

        public async Task SendMessages(string message)
        {
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }

        public ClientWebSocketHandler()
        {
            _ = InitWebSocket();
        }


        private async Task InitWebSocket()
        {
            webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(uri, CancellationToken.None);
            while (webSocket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024 * 4]);
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                OnReceiveMessage?.Invoke(Encoding.UTF8.GetString(buffer.Array, 0, result.Count));
            }
        }

        public void Dispose()
        {
            webSocket.Dispose();
        }
    }
}
