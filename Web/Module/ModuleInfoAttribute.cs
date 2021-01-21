#if FALSE
using System;

namespace HSServer.Web.Module
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple =true)]
    public class ModuleInfoAttribute : Attribute
    {
        public ModuleInfoAttribute(string Path) { this.Path = Path; }
        public string Path { get; private set; }

        /*
        public WebParamInfoList ParamsInfo { get; private set; }

        public WebModuleInfoAttribute(string Path, params WebParamInfo[] ParamInfos)
        {
            ParamsInfo = new WebParamInfoList();
            this.Path = Path;
            for(int i = 0; i < ParamsInfo.Count; i++)
                ParamsInfo.Add(ParamInfos[i]);
        }
        */
    }
}
#endif