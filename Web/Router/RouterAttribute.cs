using System;

namespace HSServer.Web.Router
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RouterAttribute : Attribute
    {
        public string Path { get; private set; }
        public string Name { get; private set; }
        public bool AutoRegister { get; private set; }

        public RouterAttribute(string Path, bool AutoRegister = true)
        {
            this.Path = Path;
            this.AutoRegister = AutoRegister;
        }
        public RouterAttribute(string Path, string Name, bool AutoRegister = true) : this(Path, AutoRegister)
        {
            this.Name = Name;
        }


        /*
        public RouterParamInfoList ParamsInfo { get; private set; }

        public void RouterInfoAttribute(string Path, params RouterInfo[] ParamInfos)
        {
            ParamsInfo = new RouterParamInfoList();
            this.Path = Path;
            for(int i = 0; i < ParamsInfo.Count; i++)
                ParamsInfo.Add(ParamInfos[i]);
        }
        */
    }
}
