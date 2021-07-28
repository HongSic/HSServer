using System.Collections.Generic;
using System.IO;

namespace HSServer.Web
{
    public interface IWebHttpRequest
    {
        Stream Body { get; }
        string URL { get; }
        string Method { get; }
        string Scheme { get; }
        long ContentLength { get; }
        string ContentType { get; }
        string Protocol { get; }
        bool IsHttps { get; }

        bool FormAvailable { get; }
        string ParamsString { get; }
        string Host { get; }

        IWebHttpParams Params { get; }
        IWebHttpResponseForm Form { get; }
        IWebHttpHeaders Headers { get; }
        IWebHttpRequestCookies Cookies { get; }
        //IWebCollection Host { get; }
    }
}
