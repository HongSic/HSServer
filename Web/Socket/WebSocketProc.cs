using System;
using System.Threading.Tasks;

namespace HSServer.Web.Socket
{
    public interface WebSocketProc : IDisposable
    {
        Task Proc(WebSocketParam Data);
    }
}
