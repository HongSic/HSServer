#if FALSE
using System;

namespace HSServer.Web.Module
{
    public class ModuleResult
    {
        public int ResultCode { get; private set; }
        public Exception Exception { get; private set; }
        public string ExtraData { get; private set; }

        public ModuleResult(ModuleResultCode ResultCode, Exception Exception = null, string ExtraData = null) : this((int)ResultCode, Exception, ExtraData) { }
        public ModuleResult(int ResultCode, Exception Exception = null, string ExtraData = null)
        {
            this.ResultCode = ResultCode;
            this.Exception = Exception;
            this.ExtraData = ExtraData;
        }

        public static explicit operator ModuleResult(ModuleResultCode Kind) { return new ModuleResult(Kind); }
        public static explicit operator ModuleResultCode(ModuleResult Result) { return (ModuleResultCode)Result.ResultCode; }
        public static explicit operator int(ModuleResult Result) { return Result.ResultCode; }
    }
}
#endif