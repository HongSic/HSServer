using HSServer.Web;

namespace HSServer.Extension
{
    public static class WebHttpRequestExtension
    {
        public static string GetParam(this IWebHttpRequest Request, string Key, string DefaultValue = null) => Request.Params.Exist(Key) ? Request.Params[Key][0] : DefaultValue;
        public static string GetHeader(this IWebHttpRequest Request, string Key, string DefaultValue = null) => Request.Headers.Exist(Key) ? Request.Headers[Key][0] : DefaultValue;

        /// <summary>
        /// GET, POST... 등등에 해당하는 값을 가져옵니다 
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Key"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public static string GetValue(this IWebHttpRequest Request, string Key, string DefaultValue = null)
        {
            string value = GetParam(Request, Key, DefaultValue);
            return value == null && Request.FormAvailable ?
                (Request.Form.Exist(Key) ? Request.Form[Key][0] : DefaultValue) :
                value;
        }
    }
}
