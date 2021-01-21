using HSServer.Web;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HSServer.Utils
{
    public static class WebUtils
    {
        #region JSONException
        public static string JSONException(this Exception ex)
        {
            string JSONmsg = "{\"status\":\"fail\",\"message\":\"알 수 없는 오류로 실패하였습니다.\",\"exception\":\"" +
            HttpUtility.JavaScriptStringEncode(ex.Message) + "\",\"code\":-1}";
            return JSONmsg;
        }
        public static string JSONException(this Exception ex, string Message)
        {
            string JSONmsg = "{\"status\":\"fail\",\"message\":\""+ HttpUtility.JavaScriptStringEncode(Message) + "\",\"exception\":\"" +
            HttpUtility.JavaScriptStringEncode(ex.Message) + "\",\"code\":-1}";
            return JSONmsg;
        }
        #endregion

        public static string GetPassword(this string password, bool Upper = false)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes("@)&)!HS_ERP_PW_SALT!#%$^&@" + password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString(Upper ? "X2" : "x2"));
                }
                return builder.ToString();
            }
        }

        public static string GetParams(string Key, IWebHttpParams Params, IWebHttpResponseForm Form, bool ParamsFirst = true)
        {
            try
            {
                if (ParamsFirst)
                {
                    if (Params.Exist(Key)) return Params[Key];
                    else if (Form != null && Form.Exist(Key)) return Form[Key];
                    return null;
                }
                else
                {
                    if (Form != null && Form.Exist(Key)) return Form[Key];
                    else if (Params.Exist(Key)) return Params[Key];
                    return null;
                }
            }
            catch { return null; }
        }

        public static string GetRedirectHTML(string URL, int Delay = 0)
        {
            return string.Format("<meta http-equiv='refresh' content='{0};url={1}' />", Delay, URL);
        }

        public static string GetURLEncode(string URL) { return HttpUtility.UrlEncode(URL); }
    }
}
