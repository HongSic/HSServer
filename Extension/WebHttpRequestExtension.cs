using HSServer.Web;

namespace HSServer.Extension
{
    public static class WebHttpRequestExtension
    {
        public static string GetParam(this IWebHttpRequest Request, string Key, string DefaultValue = null) => Request.Params.Exist(Key) ? Request.Params[Key][0] : DefaultValue;
        public static bool GetParam(this IWebHttpRequest Request, string Key, out string Value)
        {
            bool Exist =  Request.Params.Exist(Key);
            Value = Exist ? Request.Params[Key][0] : null;
            return Exist;
        }

        public static string GetHeader(this IWebHttpRequest Request, string Key, string DefaultValue = null) => Request.Headers.Exist(Key) ? Request.Headers[Key][0] : DefaultValue;
        public static bool GetHeader(this IWebHttpRequest Request, string Key, out string Value)
        {
            bool Exist = Request.Headers.Exist(Key);
            Value = Exist ? Request.Headers[Key][0] : null;
            return Exist;
        }

        /// <summary>
        /// GET, POST... 등등에 해당하는 값을 가져옵니다 
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Key"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        /// 
        public static string GetValue(this IWebHttpRequest Request, string Key, string DefaultValue = null)
        {
            string value = GetParam(Request, Key, null);
            return value == null && Request.FormAvailable ?
                (Request.Form.Exist(Key) ? Request.Form[Key][0] : DefaultValue) :
                value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool GetValue(this IWebHttpRequest Request, string Key, out string Value)
        {
            bool exist = GetParam(Request, Key, out Value);
            if (exist) return true;
            else if (Request.FormAvailable && Request.Form.Exist(Key))
            {
                Value = Request.Form[Key][0];
                return true;
            }
            return false;
        }
    }
}
