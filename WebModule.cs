﻿using HS.Utils;
using HSServer.Settings;
using HSServer.Web;
using HSServer.Web.Middleware;
using HSServer.Web.Router;
using HSServer.Web.Socket;
using HSServer.Extension;
using System.Threading.Tasks;
using System.Reflection;
using System;
using HS.Log;
using HS.Utils.Resource;
using HSServer.Web.Controller;

namespace HSServer
{
    public static class WebModule
    {
        public static bool Init(ModuleSetting Setting, LanguageManager Manager, bool AddByAssembly = true) { return Init(Setting, ref Manager, AddByAssembly); }
        public static bool Init(ModuleSetting Setting, ref LanguageManager Manager, bool AddByAssembly = true)
        {
            //string json_file = SettingsHSSever.Settings.GetPath("WebRouter.json");
            bool Load;

            Controller.Init(ref Manager);
            Load = Controller.AddByAssembly(Setting.Load.Controller);

            if(Load)
            {
                //Logger.LogSYSTEM(LogLevel.INFO, LanguageManager.Language["STR_LOG_WEB_ROUTER_INITING"]);
                Middleware.Init(ref Manager);
                if (AddByAssembly) Load = Middleware.AddByAssembly(Setting.Load.MiddleWare);
            }

            if(Load)
            {
                Router.Init(ref Manager);
                if (AddByAssembly) Load = Router.AddByAssembly(Setting.Load.Module);
                //Logger.LogSYSTEM(LogLevel.INFO, LanguageManager.Language["STR_LOG_WEB_ROUTER_INITED"]);
            }

            if(Load)
            {
                WebSocketRouter.Init(ref Manager);
                if (AddByAssembly) Load = WebSocketRouter.AddByAssembly(Setting.Load.Module);
            }

            return Load;
        }

        public static async Task<ModuleResponseCode> RouteAsync(string Path, LanguageManager STR_LANG, WebHttpContextRaw ContextRaw)
        {
            string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            //Set HTTP Header to this
            ContextRaw.Response.SetHeader("X-Powered-By", $"HS Server Framework (Web)/{Version}");
            ContextRaw.Response.SetHeader("Server", $"HS Server (Web)/{Version}");

            MiddlewareData data = await Middleware.RouteAsync(new MiddlewareData(Path, STR_LANG, ContextRaw));
            ModuleResponseCode ResultCode = (ModuleResponseCode)data.StatusCode;
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
