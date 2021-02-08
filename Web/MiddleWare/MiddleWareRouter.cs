using HS.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace HSServer.Web.MiddleWare
{
    public delegate void MiddleWareRouterInitEventHandler(LanguageManager Language);
    /// <summary>
    /// Event occure when module is adding  
    /// </summary>
    /// <param name="Message">Debug Message</param>
    /// <param name="Error"></param>
    public delegate void MiddleWareRouterAddingEventHandler(string Message, Exception Error);
    public static class MiddleWareRouter
    {
        internal static Dictionary<MiddleWarePriority, List<MiddleWareProc>> MiddleWares = new Dictionary<MiddleWarePriority, List<MiddleWareProc>>();

        public static event MiddleWareRouterInitEventHandler MiddleWareIniting;
        public static event MiddleWareRouterAddingEventHandler MiddleWareAdding;

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
                MiddleWareRouter.Language = Language;
                return true;
            }
        }

        public static void Add(MiddleWareProc MiddleWare, string Name, MiddleWarePriority Priority = MiddleWarePriority.Normal) 
        {
            if (!MiddleWares.ContainsKey(Priority)) MiddleWares.Add(Priority, new List<MiddleWareProc>());
            MiddleWares[Priority].Add(MiddleWare);

            string name = Name == null ? MiddleWare.GetType().Name : Name;
            MiddleWareAdding(string.Format("[Loaded] WebMiddleWare: {{ {0} ({1}) }}, Priority={2}", name, MiddleWare.GetType().Name, Priority), null);
        }
        public static void Add(MiddleWareProc MiddleWare)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(MiddleWare.GetType());
            foreach (Attribute attr in attrs)
                if (attr is MiddleWareInfoAttribute middle)
                    Add(MiddleWare, middle.Name, middle.Priority);
        }
        public static void AddByAssembly(params string[] ModulePath)
        {
            if (ModulePath != null && ModulePath.Length > 0)
            {
                for (int i = 0; i < ModulePath.Length; i++)
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
                                Attribute[] attrs = Attribute.GetCustomAttributes(type);
                                foreach (Attribute attr in attrs)
                                {
                                    //Console.WriteLine("ATTR_TYPE: " + attr.GetType());
                                    MiddleWareInfoAttribute mw = attr as MiddleWareInfoAttribute;
                                    if (mw != null)
                                    {
                                        //Console.WriteLine("ATTR_NAME: " + mw.Name);
                                        //Console.WriteLine("ATTR_AUTO: " + mw.AutoRegister); 
                                        if (mw.AutoRegister)
                                        {
                                            try { Add((MiddleWareProc)Activator.CreateInstance(type), mw.Name, mw.Priority); }
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
                MiddleWareAdding(string.Format(Language["STR_LOG_WEB_MIDDLEWARE_COMPLETE"]), null);
            }
        }

        public static void Remove(MiddleWareProc MiddleWare)
        {
            foreach(var pr in MiddleWares.Keys)
            {
                var mw = MiddleWares[pr];
                for (int i = 0; i < MiddleWares.Count; i++)
                {
                    uint? ID1 = ID(mw[i]);
                    uint? ID2 = ID(MiddleWare);
                    if (ID2 != null && ID1 == ID2) { mw[i].Dispose(); mw.RemoveAt(i); return; }
                }
            }
        }
        private static uint? ID(MiddleWareProc MiddleWare)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(MiddleWare.GetType());
            foreach (Attribute attr in attrs)
            {
                if (attr is MiddleWareInfoAttribute)
                {
                    MiddleWareInfoAttribute mw = (MiddleWareInfoAttribute)attr;
                    return mw.ID;
                }
            }
            return null;
        }


        internal static async Task<MiddleWareData> RouteAsync(MiddleWareData Data)
        {
            for (int j = 0; j < 3; j++)
            {
                MiddleWarePriority p = (MiddleWarePriority)j;
                if(MiddleWares.ContainsKey(p))
                {
                    for (int i = 0; i < MiddleWares[p].Count; i++)
                    {
                        var data = await MiddleWares[p][i].Proc(Data);
                        if (data != null) Data = data;
                        if (data.IsAbort) return data;
                    }
                }
            }
            return Data;
        }
    }

    public class MiddleWareRouterPack
    {
        internal List<MiddleWareProc> MiddleWares = new List<MiddleWareProc>(10);
        public void Add(MiddleWareProc MiddleWare)
        {
            if (MiddleWare == null) throw new NullReferenceException("MiddleWare cannot be null");
            else if(!Exist(MiddleWare)) MiddleWares.Add(MiddleWare);
        }

        //public MiddleWareProc this[int Index] { get { return MiddleWares[Index]; } }

        public int Count { get { return MiddleWares.Count; } }

        public bool Exist(MiddleWareProc MiddleWare)
        {
            for (int i = 0; i < MiddleWares.Count; i++)
                if(MiddleWares[i].Equals(MiddleWare)) return true;
            return false;
        }

        public MiddleWareProc[] GetMiddleWares() { return MiddleWares.ToArray(); }
    }
}
