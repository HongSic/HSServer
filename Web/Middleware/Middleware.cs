﻿using HS.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using HS.Utils.Resource;

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
        internal static Dictionary<MiddlewarePriority, List<IMiddleware>> Middlewares = new Dictionary<MiddlewarePriority, List<IMiddleware>>();

        public static event MiddlewareRouterInitEventHandler Initing;
        public static event MiddlewareRouterAddingEventHandler Adding;

        private static LanguageManager Language;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Language"></param>
        /// <returns></returns>
        internal static bool Init(LanguageManager Language) { return Init(ref Language); }
        internal static bool Init(ref LanguageManager Language)
        {
            if (Initing != null) try { Initing.Invoke(Language); } catch { }

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
                if (!Middlewares.ContainsKey(Priority)) Middlewares.Add(Priority, new List<IMiddleware>());
                Middlewares[Priority].Add(MiddleWare);

                Adding?.Invoke(string.Format("[{0}] MiddleWare: {{ {1} ({2}) }}, Priority={3} }}", Language["STR_SUCCESS"], name, MiddleWare.GetType().Name, Priority), null);
                return true;
            }
            catch(Exception ex)
            {
                Adding?.Invoke(string.Format("[{0}] MiddleWare: {{ {1} ({2}) }}, Priority={3} }} ({4})", Language["STR_ERROR"], name, MiddleWare.GetType().Name, Priority, ex.Message), null);
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

        internal static Type MiddlwareType = typeof(IMiddleware);
        internal static Type AttributeType = typeof(MiddlewareAttribute);
        public static bool AddByAssembly(params string[] ModulePath)
        {
            if (ModulePath != null && ModulePath.Length > 0)
            {
                for(int i = 0; i < ModulePath.Length; i++)
                {
                    try
                    {
                        //나중에 함수 하나 만들기
                        Assembly asm = Assembly.LoadFrom(ModulePath[i]);

                        Adding?.Invoke(string.Format(Language["STR_LOG_WEB_MIDDLEWARE_LOADING"], ModulePath[i]), null);
                        foreach (Type type in asm.GetTypes())
                        {
                            //Console.WriteLine("======");
                            //Console.WriteLine("TYPE: " + type.GetType());
                            //Console.WriteLine("NAME: " + type.Name);

                            if (type.IsImplement(MiddlwareType))
                            {
                                foreach (var attr in type.GetCustomAttributes(AttributeType))
                                {
                                    if (attr is MiddlewareAttribute module)
                                    {
                                        //Console.WriteLine("ATTR_NAME: " + mw.Name);
                                        //Console.WriteLine("ATTR_AUTO: " + mw.AutoRegister); 
                                        if (module.AutoRegister)
                                        {
                                            try { Add((IMiddleware)Activator.CreateInstance(type), module.Name, module.Priority); }
                                            catch (Exception ex) { Adding(string.Format(Language["STR_LOG_WEB_MIDDLEWARE_ERROR"], type.Name), ex); return false; }
                                            break;
                                        }
                                        else Adding(string.Format("[Unload] WebMiddleWare: {{ {0} ({1}) }}, Priority={2}", module.Name, type.Name, module.Priority), null);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { Adding(Language["STR_LOG_WEB_MIDDLEWARE_ERROR"], ex); return false; }
                }

                Adding?.Invoke(string.Format(Language["STR_LOG_WEB_MIDDLEWARE_LOADED"]), null);
            }

            return true;
        }

        public static async Task Remove(IMiddleware MiddleWare)
        {
            await Task.Run(() =>
            {
                foreach (var pr in Middlewares.Keys)
                {
                    var mw = Middlewares[pr];
                    for (int i = 0; i < Middlewares.Count; i++)
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
            for (int j = (int)MiddlewarePriority.Crital; j <= (int)MiddlewarePriority.Low; j++)
            {
                MiddlewarePriority p = (MiddlewarePriority)j;
                if (Middlewares.ContainsKey(p))
                {
                    for (int i = 0; i < Middlewares[p].Count; i++)
                    {
                        var Middleware = Middlewares[p][i];
                        try
                        {
                            var data = await Middleware.Start(Data);
                            if (data != null) Data = data;
                            if (data.IsAbort) return data;
                        }
                        catch (Exception ex) { throw new MiddlewareException(Middleware, ex); }
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
