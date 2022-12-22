using HS.Utils;
using HS.Utils.Resource;

namespace HSServer.Web.Controller
{
    public interface IController
    {
        void Init(ref LanguageManager Language);
    }
}
