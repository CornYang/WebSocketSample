using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using WebSocketSample.Interface;

namespace WebSocketSample.Services
{
    public class WebSocketService : IWebSocketService
    {
        ConcurrentDictionary<int, WebSocket> WebSockets = new ConcurrentDictionary<int, WebSocket>();

        public WebSocketService()
        {

        }

        public async Task WebSocketProcess(WebSocket webSocket)
        {
            WebSockets.TryAdd(webSocket.GetHashCode(), webSocket);

            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var userName = "CornServer";

            while (!receiveResult.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);

                var cmd = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                if (!string.IsNullOrEmpty(cmd))
                {
                    if (cmd.StartsWith("/USER "))
                        userName = cmd.Substring(6);
                    else
                        Broadcast($"{userName}:\t{cmd}");
                }

                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);

            WebSockets.TryRemove(webSocket.GetHashCode(), out var removed);
            Broadcast($"{userName} left the room.");
        }

        public void Broadcast(string message)
        {
            var buff = Encoding.UTF8.GetBytes($"{DateTime.Now:MM-dd HH:mm:ss}\t{message}");
            var data = new ArraySegment<byte>(buff, 0, buff.Length);

            Parallel.ForEach(WebSockets.Values, async (webSocket) =>
            {
                if (webSocket.State == WebSocketState.Open)
                    await webSocket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
            });
        }
    }
}
