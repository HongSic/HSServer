using HS.Utils;
using HSServer.Settings;
using HSServer.Web;
using HSServer.Web.MiddleWare;
using HSServer.Web.Module;
using System.Threading.Tasks;

namespace HSServer
{
    public static class WebRouter
    {
        public static bool Init(WebRouterSetting Setting, LanguageManager Manager) { return Init(Setting, ref Manager); }
        public static bool Init(WebRouterSetting Setting, ref LanguageManager Manager)
        {
            //string json_file = SettingsHSSever.Settings.GetPath("WebRouter.json");

            //Logger.LogSYSTEM(LogLevel.INFO, LanguageManager.Language["STR_LOG_WEB_ROUTER_INITING"]);
            MiddleWareRouter.Init(ref Manager);
            MiddleWareRouter.AddByAssembly(Setting.Load.MiddleWare);

            ModuleRouter.Init(ref Manager);
            ModuleRouter.AddByAssembly(Setting.Load.Module);
            //Logger.LogSYSTEM(LogLevel.INFO, LanguageManager.Language["STR_LOG_WEB_ROUTER_INITED"]);

            return true;
        }

        public static async Task<ModuleResultCode> RouteAsync(string Path, LanguageManager STR_LANG, WebHttpContextRaw ContextRaw)
        {
            MiddleWareData data = await MiddleWareRouter.RouteAsync(new MiddleWareData(Path, STR_LANG, ContextRaw));
            ModuleResultCode ResultCode = ModuleResultCode.OK;
            if (!data.IsClose && ContextRaw.Response.IsWritable) ResultCode = await ModuleRouter.RouteAsync(data);
            //MiddleWareData data_post = await MiddleWare_Post.RouteAsync(Path, Params, STR_LANG, ContextRaw);
            return ResultCode;
        }
    }
}
