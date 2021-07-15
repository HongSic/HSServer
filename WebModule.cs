using HS.Utils;
using HSServer.Settings;
using HSServer.Web;
using HSServer.Web.Middleware;
using HSServer.Web.Router;
using HSServer.Web.Socket;
using HSServer.Extension;
using System.Threading.Tasks;
using System.Reflection;
using System;

namespace HSServer
{
    public static class WebModule
    {
        public static bool Init(ModuleSetting Setting, LanguageManager Manager, bool AddByAssembly = true) { return Init(Setting, ref Manager, AddByAssembly); }
        public static bool Init(ModuleSetting Setting, ref LanguageManager Manager, bool AddByAssembly = true)
        {
            //string json_file = SettingsHSSever.Settings.GetPath("WebRouter.json");

            //Logger.LogSYSTEM(LogLevel.INFO, LanguageManager.Language["STR_LOG_WEB_ROUTER_INITING"]);
            Middleware.Init(ref Manager);
            if(AddByAssembly) Middleware.AddByAssembly(Setting.Load.MiddleWare);

            Router.Init(ref Manager);
            if (AddByAssembly) Router.AddByAssembly(Setting.Load.Module);
            //Logger.LogSYSTEM(LogLevel.INFO, LanguageManager.Language["STR_LOG_WEB_ROUTER_INITED"]);

            WebSocketRouter.Init(ref Manager);
            if (AddByAssembly) WebSocketRouter.AddByAssembly(Setting.Load.Module);

            return true;
        }

        public static async Task<RouterResponseCode> RouteAsync(string Path, LanguageManager STR_LANG, WebHttpContextRaw ContextRaw)
        {
            string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            //Set HTTP Header to this
            ContextRaw.Response.SetHeader("X-Powered-By", $"HS Server Framework (Web)/{Version}");
            if(!ContextRaw.Response.Headers.Exist("Server")) ContextRaw.Response.SetHeader("Server", $"HS Server (Web)/{Version}");

            MiddlewareData data = await Middleware.RouteAsync(new MiddlewareData(Path, STR_LANG, ContextRaw));
            RouterResponseCode ResultCode = RouterResponseCode.OK;
            if (!data.IsClose && ContextRaw.Response.IsWritable) ResultCode = await Router.RouteAsync(data);
            //MiddleWareData data_post = await MiddleWare_Post.RouteAsync(Path, Params, STR_LANG, ContextRaw);
            return ResultCode;
        }



        #region Extension
        public static bool IsImplement(this Type Type1, Type Type2)
        {
            bool IsClass = Type1.BaseType != null && Type1.BaseType.Equals(Type2);
            return IsClass ? IsClass : Type1.GetInterface(Type2.FullName) != null;
        }
        #endregion
    }
}
