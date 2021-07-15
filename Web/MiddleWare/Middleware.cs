using HS.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace HSServer.Web.Middleware
{
    public delegate void MiddlewareRouterInitEventHandler(LanguageManager Language);
    /// <summary>
    /// Event occure when module is adding  
    /// </summary>
    /// <param name="Message">Debug Message</param>
    /// <param name="Error"></param>
    public delegate void MiddlewareRouterAddingEventHandler(string Message, Exception Error);
    public static class Middleware
    {
        internal static Dictionary<MiddlewarePriority, List<IMiddleware>> MiddleWares = new Dictionary<MiddlewarePriority, List<IMiddleware>>();

        public static event MiddlewareRouterInitEventHandler MiddleWareIniting;
        public static event MiddlewareRouterAddingEventHandler MiddleWareAdding;

        private static LanguageManager Language;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Language"></param>
        /// <returns></returns>
        internal static bool Init(LanguageManager Language) { return Init(ref Language); }
        internal static bool Init(ref LanguageManager Language)
        {
            if (MiddleWareIniting != null) try { MiddleWareIniting.Invoke(Language); } catch { }

            if (Language == null) return false;
            else
            {
                Middleware.Language = Language;
                return true;
            }
        }

        public static bool Add(IMiddleware MiddleWare, string Name, MiddlewarePriority Priority = MiddlewarePriority.Normal)
        {
            string name = Name == null ? MiddleWare.GetType().Name : Name;
            try
            {
                MiddleWare.Attach(Language);
                if (!MiddleWares.ContainsKey(Priority)) MiddleWares.Add(Priority, new List<IMiddleware>());
                MiddleWares[Priority].Add(MiddleWare);

                MiddleWareAdding?.Invoke(string.Format("[Loaded Success] WebMiddleWare: {{ {0} ({1}) }}, Priority={2}", name, MiddleWare.GetType().Name, Priority), null);
                return true;
            }
            catch(Exception ex)
            {
                MiddleWareAdding?.Invoke(string.Format("[Loaded Error!!] WebMiddleWare: {{ {0} ({1}) }}, Priority={2}", name, MiddleWare.GetType().Name, Priority), null);
                return false;
            }
        }
        public static void Add(IMiddleware MiddleWare)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(MiddleWare.GetType());
            foreach (Attribute attr in attrs)
                if (attr is MiddlewareAttribute middle)
                    Add(MiddleWare, middle.Name, middle.Priority);
        }

        public static void AddByAssembly(params string[] ModulePath)
        {
            Type MiddlwareType = typeof(IMiddleware);
            Type AttributeIType = typeof(MiddlewareAttribute);
            if (ModulePath != null && ModulePath.Length > 0)
            {
                for(int i = 0; i < ModulePath.Length; i++)
                {
                    try
                    {
                        //나중에 함수 하나 만들기
                        Assembly asm = Assembly.LoadFrom(ModulePath[i]);

                        MiddleWareAdding?.Invoke(string.Format(Language["STR_LOG_WEB_MIDDLEWARE_LOADING"], ModulePath[i]), null);
                        foreach (Type type in asm.GetTypes())
                        {
                            //Console.WriteLine("======");
                            //Console.WriteLine("TYPE: " + type.GetType());
                            //Console.WriteLine("NAME: " + type.Name);
                            try
                            {
                                if(type.IsImplement(MiddlwareType))
                                {
                                    if (type.GetCustomAttribute(AttributeIType) is MiddlewareAttribute mw)
                                    {
                                        //Console.WriteLine("ATTR_NAME: " + mw.Name);
                                        //Console.WriteLine("ATTR_AUTO: " + mw.AutoRegister); 
                                        if (mw.AutoRegister)
                                        {
                                            try { Add((IMiddleware)Activator.CreateInstance(type), mw.Name, mw.Priority); }
                                            catch (Exception ex) { MiddleWareAdding(string.Format(Language["STR_LOG_WEB_MIDDLEWARE_ERROR"], type.Name), ex); }
                                            break;
                                        }
                                        else MiddleWareAdding(string.Format("[UnLoad] WebMiddleWare: {{ {0} ({1}) }}, Priority={2}", mw.Name, type.Name, mw.Priority), null);
                                    }
                                }
                            }
                            catch (Exception ex) { MiddleWareAdding(Language["STR_LOG_WEB_MIDDLEWARE_ERROR"], ex); }
                        }
                    }
                    catch (Exception ex) { MiddleWareAdding(Language["STR_LOG_WEB_MIDDLEWARE_ERROR"], ex); }
                }
                MiddleWareAdding?.Invoke(string.Format(Language["STR_LOG_WEB_MIDDLEWARE_COMPLETE"]), null);
            }
        }

        public static async Task Remove(IMiddleware MiddleWare)
        {
            await Task.Run(() =>
            {
                foreach (var pr in MiddleWares.Keys)
                {
                    var mw = MiddleWares[pr];
                    for (int i = 0; i < MiddleWares.Count; i++)
                    {
                        uint? ID1 = ID(mw[i]);
                        uint? ID2 = ID(MiddleWare);
                        if (ID2 != null && ID1 == ID2) { mw[i].Detach(); mw.RemoveAt(i); return; }
                    }
                }
            });
        }
        private static uint? ID(IMiddleware MiddleWare)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(MiddleWare.GetType());
            foreach (Attribute attr in attrs)
            {
                if (attr is MiddlewareAttribute)
                {
                    MiddlewareAttribute mw = (MiddlewareAttribute)attr;
                    return mw.ID;
                }
            }
            return null;
        }


        internal static async Task<MiddlewareData> RouteAsync(MiddlewareData Data)
        {
            for (int j = (int)MiddlewarePriority.Crital; j < (int)MiddlewarePriority.Low; j++)
            {
                MiddlewarePriority p = (MiddlewarePriority)j;
                if(MiddleWares.ContainsKey(p))
                {
                    for (int i = 0; i < MiddleWares[p].Count; i++)
                    {
                        var data = await MiddleWares[p][i].Start(Data);
                        if (data != null) Data = data;
                        if (data.IsAbort) return data;
                    }
                }
            }
            return Data;
        }
    }

    class MiddlewareRouterPack
    {
        internal List<IMiddleware> Middlewares = new List<IMiddleware>(10);
        public void Add(IMiddleware MiddleWare)
        {
            if (MiddleWare == null) throw new NullReferenceException("MiddleWare cannot be null");
            else if(!Exist(MiddleWare)) Middlewares.Add(MiddleWare);
        }

        //public MiddleWareProc this[int Index] { get { return MiddleWares[Index]; } }

        public int Count { get { return Middlewares.Count; } }

        public bool Exist(IMiddleware MiddleWare)
        {
            for (int i = 0; i < Middlewares.Count; i++)
                if(Middlewares[i].Equals(MiddleWare)) return true;
            return false;
        }

        public IMiddleware[] GetMiddleWares() { return Middlewares.ToArray(); }
    }
}
