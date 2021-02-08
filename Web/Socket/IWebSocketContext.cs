using System.Net.WebSockets;
using System.Threading.Tasks;

namespace HSServer.Web.Socket
{
    public interface IWebSocketContext
    {
        bool IsWebSocketConnection { get; }
        Task<WebSocket> ConnectAsync();
    }
}
