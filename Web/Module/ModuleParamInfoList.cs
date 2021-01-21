using System.Collections.Generic;

namespace HSServer.Web.Module
{
    public class ModuleParamInfoList : Dictionary<string, ModuleParamType>
    {
        public ModuleParamInfoList() : base() { }
        public ModuleParamInfoList Add(ModuleParamAttribute ParamInfo) { return Add(ParamInfo.Name, ParamInfo.Type); }
        public new ModuleParamInfoList Add(string Name, ModuleParamType Type)
        {
            if (ContainsKey(Name)) base[Name] = Type;
            else base.Add(Name, Type);
            return this;
        }
    }
}
