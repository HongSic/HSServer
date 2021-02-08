using HS.Utils;

namespace HSServer.Web.Socket
{
    public class WebSocketParam
    {
        string Path { get; }
        LanguageManager STR_LANG { get; }
        IWebSocketContext Context { get; }
    }
}
