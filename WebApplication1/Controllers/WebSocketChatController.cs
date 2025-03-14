using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace WebApplication1.Controllers;
[ApiController]

public class WebSocketChatController : ControllerBase
{
    private static List<WebSocket> webSockets = new();
    [Route("/chat")]
    public async Task InitChat()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            webSockets.Add(webSocket);
            await ResendMessages(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task ResendMessages(WebSocket senderWebSocket)
    {
        while (senderWebSocket.State == WebSocketState.Open)
        {
            ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024 * 4]);
            var result = await senderWebSocket.ReceiveAsync(buffer, CancellationToken.None);
            foreach (var webSocket in webSockets)
            {
                if (senderWebSocket == webSocket) continue;
                await webSocket.SendAsync(buffer.Array, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}

