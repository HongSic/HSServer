using System;
using System.Threading.Tasks;

namespace HSServer.Web.Module
{
    public interface ModuleProc : IDisposable
    {
        Task<ModuleResultCode> Proc(ModuleData Data);
    }
}
