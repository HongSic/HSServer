using HS.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace HSServer.Web.Module
{
    public delegate void ModuleRouterInitEventHandler(LanguageManager Language);
    /// <summary>
    /// Event occure when module is adding  
    /// </summary>
    /// <param name="Message">Debug Message</param>
    /// <param name="Error"></param>
    public delegate void ModuleRouterAddingEventHandler(string Message, Exception Error);
    //[ModulePath("HSERP.Web.Module.ERP")]
    public sealed class ModuleRouter
    {
        internal ModuleRouter() { }

        private static readonly Dictionary<string, ModuleProc> Modules = new Dictionary<string, ModuleProc>();

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
                ModuleRouter.Language = Language;
                return true;
            }

            //Logger.LogSYSTEM(LogLevel.INFO, string.Format(LanguageManager.Language["STR_LOG_WEB_MODULE_LOADED"]));
        }
        //private static void InitErrorLog(Exception ex, string ModuleName) { Logger.LogSYSTEM(LogLevel.ERROR, LanguageManager.Language["STR_LOG_WEB_MODULE_ERROR"], ex, ModuleName); }

        public static bool Add(string WebPath, ModuleProc Module, string Name = null)
        {
            string name = Name == null ? Module.GetType().Name : Name;
            if (!Modules.ContainsKey(WebPath))
            {
                Modules.Add(WebPath, Module);
                ModuleAdding?.Invoke(string.Format("[Loaded] WebModule: [{0}] {1} ({2})", WebPath, Name, Module.GetType().Name), null);
                return true;
            }
            else 
            {
                ModuleAdding?.Invoke(string.Format("[Exist!] WebModule: [{0}] {1} ({2})", WebPath, Name, Module.GetType().Name), null);
                return false;
            }

            //if (Modules.ContainsKey(Path)) Modules[Path] = Module;
            //else Modules.Add(Path, Module);
        }
        public static bool Add(ModuleProc Module)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(Module.GetType());
            foreach (Attribute attr in attrs)
                if (attr is ModulePathAttribute module) 
                    return Add(module.Path, Module, module.Name);
            return false;
        }
        public static void AddByAssembly(params string[] ModulePath)
        {
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
                        ModuleAdding(string.Format(Language["STR_LOG_WEB_MODULE_LOADING"], ModulePath[i]), null);

                        Assembly asm = Assembly.LoadFrom(path);

                        /*
                        IEnumerable<string> result = asm.GetTypes()
                            .Where(type => 
                            type.Namespace == NameSpace)
                            .Select(type => type.Name);
                        */

                        foreach (Type type in asm.GetTypes())
                        {
                            if(type is ModuleProc)
                            {
                                try
                                {
                                    Attribute[] attrs = Attribute.GetCustomAttributes(type);
                                    foreach (Attribute attr in attrs)
                                    {
                                        if (attr is ModulePathAttribute module)
                                        {
                                            try { Add(module.Path, (ModuleProc)Activator.CreateInstance(type), module.Name); }
                                            catch (Exception ex) { ModuleAdding(string.Format(Language["STR_LOG_WEB_MODULE_ERROR"], module.Name), ex); }
                                        }
                                    }
                                }
                                catch (Exception ex) { ModuleAdding(Language["STR_LOG_WEB_MODULE_ERROR"], ex); }
                            }
                        }
                    }
                }
                catch (Exception ex) { ModuleAdding(Language["STR_LOG_WEB_MODULE_ERROR"], ex); }
            }
        }


        public static void Remove(string Path) { if(Modules.ContainsKey(Path)) Modules.Remove(Path); }


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
                        int idx;
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
    }

    public class ModuleProcPack
    {
        internal Dictionary<string, ModuleProc> Modules = new Dictionary<string, ModuleProc>();
        public bool Exist(string Path) { return Modules.ContainsKey(Path); }
        public void Add(string Path, ModuleProc Module) { if (Module != null) Modules.Add(Path, Module); else throw new NullReferenceException("Module cannot be null"); }
        public void Add(ModuleProc Module)
        {
            if(Module == null) throw new NullReferenceException("Module cannot be null");

            Attribute[] attrs = Attribute.GetCustomAttributes(Module.GetType());
            foreach (Attribute attr in attrs)
            {
                if (attr is ModulePathAttribute module) Add(module.Path, Module);
            }
        }

        public int Count { get { return Modules.Count; } }

        public string[] Paths() 
        {
            string[] m = new string[Modules.Count];

            int i = 0;
            foreach(string path in Modules.Keys) { m[i] = path; i++; }

            return m;
        }
    }
}
