using System;

namespace HSServer.Web.Router
{
    [AttributeUsage(AttributeTargets.All)]
    public class RouterInfoAttribute : Attribute
    {
        public RouterInfoAttribute(string Name) { this.Name = Name; }
        public string Name { get; private set; }

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