using System.Net.WebSockets;

namespace WebSocketSample.Interface
{
    public interface IWebSocketService
    {
        Task WebSocketProcess(WebSocket webSocket);
    }
}
