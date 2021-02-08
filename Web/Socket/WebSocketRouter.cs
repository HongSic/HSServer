using HS.Utils;
using HSServer.Web.Module;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HSServer.Web.Socket
{
    public delegate void WebSocketRouterInitEventHandler(LanguageManager Language);
    /// <summary>
    /// Event occure when module is adding  
    /// </summary>
    /// <param name="Message">Debug Message</param>
    /// <param name="Error"></param>
    public delegate void WebSocketRouterAddingEventHandler(string Message, Exception Error);
    public class WebSocketRouter
    {
        internal WebSocketRouter() { }

        private static readonly Dictionary<string, WebSocketProc> Modules = new Dictionary<string, WebSocketProc>();

        public static event WebSocketRouterInitEventHandler WebSocketIniting;
        public static event WebSocketRouterAddingEventHandler WebSocketAdding;

        private static LanguageManager Language;


        /// <summary>
        /// 모듈 초기화 (처음 1회만 실행)
        /// </summary>
        /// <returns></returns>
        internal static bool Init(LanguageManager Language) { return Init(ref Language); }
        internal static bool Init(ref LanguageManager Language)
        {
            if (WebSocketIniting != null) try { WebSocketIniting.Invoke(Language); } catch { }

            if (Language == null) return false;
            else
            {
                WebSocketRouter.Language = Language;
                return true;
            }
        }

        public static bool Add(string WebPath, WebSocketProc Module)
        {
            string Name = Module.GetType().Name;
            if (!Modules.ContainsKey(WebPath))
            {
                Modules.Add(WebPath, Module);
                WebSocketAdding?.Invoke(string.Format("[Loaded] WebSocket: [{0}] {1}", WebPath, Name), null);
                return true;
            }
            else
            {
                WebSocketAdding?.Invoke(string.Format("[Exist!] WebSocket: [{0}] {1}", WebPath, Name), null);
                return false;
            }

            //if (Modules.ContainsKey(Path)) Modules[Path] = Module;
            //else Modules.Add(Path, Module);
        }
        public static void AddByAssembly(params string[] SocketPath)
        {
            for (int i = 0; i < SocketPath.Length; i++)
            {
                try
                {
                    if (SocketPath != null && SocketPath.Length > 0)
                    {
                        string path = SocketPath[i];
                        if (!File.Exists(path))
                        {
                            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                            path = StringUtils.PathMaker(dir, path);
                            if (!File.Exists(path)) continue;
                        }
                        WebSocketAdding(string.Format(Language["STR_LOG_WEB_SOCKET_LOADING"], SocketPath[i]), null);

                        Assembly asm = Assembly.LoadFrom(path);

                        /*
                        IEnumerable<string> result = asm.GetTypes()
                            .Where(type => 
                            type.Namespace == NameSpace)
                            .Select(type => type.Name);
                        */

                        foreach (Type type in asm.GetTypes())
                        {
                            try
                            {
                                Attribute[] attrs = Attribute.GetCustomAttributes(type);
                                foreach (Attribute attr in attrs)
                                {
                                    if (attr is ModulePathAttribute module)
                                    {
                                        try { Add(module.Path, (WebSocketProc)Activator.CreateInstance(type)); }
                                        catch (Exception ex) { WebSocketAdding(string.Format(Language["STR_LOG_WEB_SOCKET_ERROR"], type.Name), ex); }
                                    }
                                }
                            }
                            catch (Exception ex) { WebSocketAdding(Language["STR_LOG_WEB_SOCKET_ERROR"], ex); }
                        }
                    }
                }
                catch (Exception ex) { WebSocketAdding(Language["STR_LOG_WEB_SOCKET_ERROR"], ex); }
            }
        }


        public static void Remove(string Path) { if (Modules.ContainsKey(Path)) Modules.Remove(Path); }

        /*
        internal static async Task<ModuleResultCode> RouteAsync(ModuleData data)
        {
            if (data == null) return ModuleResultCode.Bypass;

            if (Modules.ContainsKey(data.Path)) return await Modules[data.Path].Proc(data);
            else
            {
                string wildcard = StringUtils.GetDirectoryName(data.Path);
                if (Modules.ContainsKey(wildcard)) return await Modules[wildcard].Proc(data);
                else
                {
                    wildcard += "/*";
                    if (Modules.ContainsKey(wildcard)) return await Modules[wildcard].Proc(data);
                    else
                    {
                        string path = data.Path;
                        int idx = 0;
                        while ((idx = path.LastIndexOf('/')) > -1)
                        {
                            path = path.Remove(idx);
                            string path_wildcard = path + "/**";
                            if (Modules.ContainsKey(path_wildcard)) return await Modules[path_wildcard].Proc(data);
                        }
                        return ModuleResultCode.NotFound;
                    }
                }
            }
        }
        */
    }
}
