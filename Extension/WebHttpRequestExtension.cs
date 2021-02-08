using HSServer.Web;

namespace HSServer.Extension
{
    public static class WebHttpRequestExtension
    {
        public static string GetParam(this IWebHttpRequest Request, string Key, string DefaultValue = null) => Request.Params.Exist(Key) ? Request.Params[Key] : DefaultValue;
        public static string GetHeader(this IWebHttpRequest Request, string Key, string DefaultValue = null) => Request.Headers.Exist(Key) ? Request.Headers[Key] : DefaultValue;
    }
}
