using System.Collections.Generic;

namespace HSServer.Web.Router
{
    public class RouterParamInfoList : Dictionary<string, RouterParamType>
    {
        public RouterParamInfoList() : base() { }
        public RouterParamInfoList Add(RouterParamAttribute ParamInfo) { return Add(ParamInfo.Name, ParamInfo.Type); }
        public new RouterParamInfoList Add(string Name, RouterParamType Type)
        {
            if (ContainsKey(Name)) base[Name] = Type;
            else base.Add(Name, Type);
            return this;
        }
    }
}
