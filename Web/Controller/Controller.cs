using HS.Utils;
using System;
using System.Reflection;

namespace HSServer.Web.Controller
{
    public delegate void ControllerInitEventHandler(LanguageManager Language);
    /// <summary>
    /// Event occure when module is adding  
    /// </summary>
    /// <param name="Message">Debug Message</param>
    /// <param name="Error"></param>
    public delegate void ControllerAddingEventHandler(string Message, Exception Error);

    public class Controller
    {

        public static event ControllerInitEventHandler Initing;
        public static event ControllerAddingEventHandler Adding;

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
                Controller.Language = Language;
                return true;
            }
        }

        internal static Type ControllerType = typeof(IController);
        public static bool AddByAssembly(params string[] ModulePath)
        {
            if (ModulePath?.Length > 0)
            {
                Adding?.Invoke(Language["STR_LOG_CONTROLLER_INITING"], null);

                try
                {
                    for (int i = 0; i < ModulePath.Length; i++)
                    {
                        //나중에 함수 하나 만들기
                        Assembly asm = Assembly.LoadFrom(ModulePath[i]);

                        Adding?.Invoke(string.Format(Language["STR_LOG_CONTROLLER_LOADING"], ModulePath[i]), null);

                        foreach (Type type in asm.GetTypes())
                        {
                            if (type.IsImplement(ControllerType))
                            {
                                //컨트롤러 초기화
                                ((IController)Activator.CreateInstance(type)).Init(ref Language);
                                Adding?.Invoke($"[{Language["STR_SUCCESS"]}] Controller: {type.Name}", null);
                            }
                        }
                    }
                    Adding?.Invoke(string.Format(Language["STR_LOG_CONTROLLER_INITED"]), null);
                }
                catch (Exception ex) { Adding?.Invoke(Language["STR_LOG_CONTROLLER_ERROR"], ex); return false; }
            }

            return true;
        }
    }
}
