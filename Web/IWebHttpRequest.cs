﻿using System.Collections.Generic;

namespace HSServer.Web
{
    public interface IWebHttpRequest
    {
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
        IWebHttpCookies Cookies { get; }
        //IWebCollection Host { get; }
    }
}
