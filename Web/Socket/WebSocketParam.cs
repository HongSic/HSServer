using HS.Utils;
using HS.Utils.Resource;

namespace HSServer.Web.Socket
{
    public class WebSocketParam
    {
        string Path { get; }
        LanguageManager STR_LANG { get; }
        IWebSocketContext Context { get; }
    }
}
