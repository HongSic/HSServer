using HS.Utils;
using HSServer.Settings;
using HSServer.Web;
using HSServer.Web.MiddleWare;
using HSServer.Web.Module;
using HSServer.Web.Socket;
using HSServer.Extension;
using System.Threading.Tasks;
using System.Reflection;

namespace HSServer
{
    public static class WebRouter
    {
        public static bool Init(WebRouterSetting Setting, LanguageManager Manager, bool AddByAssembly = true) { return Init(Setting, ref Manager, AddByAssembly); }
        public static bool Init(WebRouterSetting Setting, ref LanguageManager Manager, bool AddByAssembly = true)
        {
            //string json_file = SettingsHSSever.Settings.GetPath("WebRouter.json");

            //Logger.LogSYSTEM(LogLevel.INFO, LanguageManager.Language["STR_LOG_WEB_ROUTER_INITING"]);
            MiddleWareRouter.Init(ref Manager);
            if(AddByAssembly) MiddleWareRouter.AddByAssembly(Setting.Load.MiddleWare);

            ModuleRouter.Init(ref Manager);
            if (AddByAssembly) ModuleRouter.AddByAssembly(Setting.Load.Module);
            //Logger.LogSYSTEM(LogLevel.INFO, LanguageManager.Language["STR_LOG_WEB_ROUTER_INITED"]);

            WebSocketRouter.Init(ref Manager);
            if (AddByAssembly) WebSocketRouter.AddByAssembly(Setting.Load.Module);

            return true;
        }

        public static async Task<ModuleResponseCode> RouteAsync(string Path, LanguageManager STR_LANG, WebHttpContextRaw ContextRaw)
        {
            string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            //Set HTTP Header to this
            ContextRaw.Response.SetHeader("X-Powered-By", $"HS Server Framework (Web)/{Version}");
            if(!ContextRaw.Response.Headers.Exist("Server")) ContextRaw.Response.SetHeader("Server", $"HS Server (Web)/{Version}");

            MiddleWareData data = await MiddleWareRouter.RouteAsync(new MiddleWareData(Path, STR_LANG, ContextRaw));
            ModuleResponseCode ResultCode = ModuleResponseCode.OK;
            if (!data.IsClose && ContextRaw.Response.IsWritable) ResultCode = await ModuleRouter.RouteAsync(data);
            //MiddleWareData data_post = await MiddleWare_Post.RouteAsync(Path, Params, STR_LANG, ContextRaw);
            return ResultCode;
        }
    }
}
