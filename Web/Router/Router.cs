using HS.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace HSServer.Web.Router
{
    public delegate void ModuleRouterInitEventHandler(LanguageManager Language);
    /// <summary>
    /// Event occure when module is adding  
    /// </summary>
    /// <param name="Message">Debug Message</param>
    /// <param name="Error"></param>
    public delegate void ModuleRouterAddingEventHandler(string Message, Exception Error);
    public static class Router
    {
        private static readonly Dictionary<string, IRouter> Modules = new Dictionary<string, IRouter>();

        public static event ModuleRouterInitEventHandler ModuleIniting;
        public static event ModuleRouterAddingEventHandler ModuleAdding;

        private static LanguageManager Language;

        /// <summary>
        /// 모듈 초기화 (처음 1회만 실행)
        /// </summary>
        /// <returns></returns>
        internal static bool Init(LanguageManager Language) { return Init(ref Language); }
        internal static bool Init(ref LanguageManager Language)
        {

            if (ModuleIniting != null) try { ModuleIniting.Invoke(Language); } catch { }

            if (Language == null) return false;
            else
            {
                Router.Language = Language;
                return true;
            }

            //Logger.LogSYSTEM(LogLevel.INFO, string.Format(LanguageManager.Language["STR_LOG_WEB_MODULE_LOADED"]));
        }
        //private static void InitErrorLog(Exception ex, string ModuleName) { Logger.LogSYSTEM(LogLevel.ERROR, LanguageManager.Language["STR_LOG_WEB_MODULE_ERROR"], ex, ModuleName); }

        public static bool Add(string WebPath, IRouter Module, string Name = null)
        {
            string name = Name ?? Module.GetType().Name;
            if (!Modules.ContainsKey(WebPath))
            {
                try 
                { 
                    Module.Attach(Language);
                    Modules.Add(WebPath, Module);
                    ModuleAdding?.Invoke(string.Format("[Loaded Success] Router: [{0}] {1} ({2})", WebPath, Name, Module.GetType().Name), null);
                    return true;
                }
                catch(Exception ex)
                {
                    ModuleAdding?.Invoke(string.Format("[Loaded Error!!] Router: [{0}] {1} ({2})", WebPath, Name, Module.GetType().Name), ex);
                    return false;
                }
            }
            else 
            {
                ModuleAdding?.Invoke(string.Format("[Exist!] Router: [{0}] {1} ({2})", WebPath, Name, Module.GetType().Name), null);
                return false;
            }

            //if (Modules.ContainsKey(Path)) Modules[Path] = Module;
            //else Modules.Add(Path, Module);
        }
        public static bool Add(IRouter Module)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(Module.GetType());
            foreach (Attribute attr in attrs)
                if (attr is RouterAttribute module)
                    return Add(module.Path, Module, module.Name);
            return false;
        }
        public static void AddByAssembly(params string[] ModulePath)
        {
            Type RouterType = typeof(IRouter);
            Type AttributeIType = typeof(RouterAttribute);
            for (int i = 0; i < ModulePath.Length; i++)
            {
                try
                {
                    if (ModulePath != null && ModulePath.Length > 0)
                    {
                        string path = ModulePath[i];
                        if (!File.Exists(path))
                        {
                            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                            path = StringUtils.PathMaker(dir, path);
                            if (!File.Exists(path)) continue;
                        }
                        ModuleAdding?.Invoke(string.Format(Language["STR_LOG_WEB_MODULE_LOADING"], ModulePath[i]), null);

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
                                if (type.IsImplement(RouterType))
                                {
                                    if (type.GetCustomAttribute(AttributeIType) is RouterAttribute module)
                                    {
                                        if(module.AutoRegister)
                                        {
                                            try { Add(module.Path, (IRouter)Activator.CreateInstance(type), module.Name); }
                                            catch (Exception ex) { ModuleAdding?.Invoke(string.Format(Language["STR_LOG_WEB_MODULE_ERROR"], module.Name), ex); }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex) { ModuleAdding?.Invoke(Language["STR_LOG_WEB_MODULE_ERROR"], ex); }
                        }
                    }
                }
                catch (Exception ex) { ModuleAdding?.Invoke(Language["STR_LOG_WEB_MODULE_ERROR"], ex); }
            }
        }


        public static void Remove(string Path) { if (Modules.ContainsKey(Path)) { Modules[Path].Detach(); Modules.Remove(Path); } }
        public static void RemoveAll() { foreach (string Path in Modules.Keys) { Modules[Path].Detach(); Modules.Remove(Path); } }


        internal static async Task<RouterResponseCode> RouteAsync(RouterData data)
        {
            if (data == null) return RouterResponseCode.Bypass;

            if (Modules.ContainsKey(data.Path)) return await Modules[data.Path].Route(data);
            else
            {
                string wildcard = StringUtils.GetDirectoryName(data.Path);
                if (Modules.ContainsKey(wildcard)) return await Modules[wildcard].Route(data);
                else
                {
                    wildcard += "/*";
                    if (Modules.ContainsKey(wildcard)) return await Modules[wildcard].Route(data);
                    else
                    {
                        string path = data.Path;
                        int idx;
                        while ((idx = path.LastIndexOf('/')) > -1)
                        {
                            path = path.Remove(idx);
                            string path_wildcard = path + "/**";
                            if (Modules.ContainsKey(path_wildcard)) return await Modules[path_wildcard].Route(data);
                        }
                        return RouterResponseCode.NotFound;
                    }
                }
            }
        }
    }

    class RouterProcPack
    {
        internal Dictionary<string, IRouter> Routers = new Dictionary<string, IRouter>();
        public bool Exist(string Path) { return Routers.ContainsKey(Path); }
        public void Add(string Path, IRouter Module) { if (Module != null) Routers.Add(Path, Module); else throw new NullReferenceException("Router cannot be null"); }
        public void Add(IRouter Module)
        {
            if(Module == null) throw new NullReferenceException("Module cannot be null");

            Attribute[] attrs = Attribute.GetCustomAttributes(Module.GetType());
            foreach (Attribute attr in attrs)
            {
                if (attr is RouterAttribute module) Add(module.Path, Module);
            }
        }

        public int Count { get { return Routers.Count; } }

        public string[] Paths() 
        {
            string[] m = new string[Routers.Count];

            int i = 0;
            foreach(string path in Routers.Keys) { m[i] = path; i++; }

            return m;
        }
    }
}
