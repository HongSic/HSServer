using System;
using System.Threading.Tasks;

namespace HSServer.Web.MiddleWare
{
    public interface MiddleWareProc : IDisposable
    {
        Task<MiddleWareData> Proc(MiddleWareData Data);
    }
}
